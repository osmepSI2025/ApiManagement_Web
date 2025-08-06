using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.WebSso;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Serilog FIRST for early logging ---
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.File(
        path: "Logs/app-log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
);

// --- Add all services to the container ONCE ---

// Core MVC services
builder.Services.AddControllersWithViews(); // Call this only once

// Line OA Specific Services
builder.Services.Configure<LineLoginSettings>(builder.Configuration.GetSection("LineLoginSettings"));
// Session services (unified)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// SAML2 Authentication Services
var saml2Section = builder.Configuration.GetSection("Saml2:Saml2");
var entityId = saml2Section["EntityId"];
var idpEntityId = saml2Section["IdpEntityId"];
var ssoUrl = saml2Section["SingleSignOnServiceUrl"];
var sloUrl = saml2Section["SingleLogoutServiceUrl"];
var signingCertBase64 = saml2Section["SigningCertificate"];
var AcsUrl = saml2Section["AcsUrl"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Saml2";
})
.AddCookie()
.AddSaml2(options =>
{
    builder.Configuration.GetSection("SustainsysSaml2").Bind(options);
    options.SPOptions.EntityId = new EntityId(entityId);
    options.SPOptions.ReturnUrl = new Uri(AcsUrl);

    var idp = new IdentityProvider(
        new EntityId(idpEntityId),
        options.SPOptions)
    {
        SingleSignOnServiceUrl = new Uri(ssoUrl),
        SingleLogoutServiceUrl = new Uri(sloUrl),
        Binding = Saml2BindingType.HttpRedirect,
        AllowUnsolicitedAuthnResponse = true
    };

    idp.SigningKeys.AddConfiguredKey(new X509Certificate2(Convert.FromBase64String(signingCertBase64)));
    options.IdentityProviders.Add(idp);
});

// Custom Services
builder.Services.AddScoped<ICallAPIService, CallAPIService>();
builder.Services.AddHttpClient<CallAPIService>();

// Authorization services
builder.Services.AddAuthorization(); // Call this only once
// builder.Services.AddControllers(); // This is typically included by AddControllersWithViews(), can be removed if not explicitly needed for API controllers only
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// --- Configure the HTTP request pipeline (Middleware Order is CRITICAL) ---

// Development environment specific settings
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Order of middleware:
// 1. Static Files (if any)
app.UseStaticFiles();

// 2. Routing (defines how requests are matched to endpoints)
app.UseRouting();

// 3. Session (must be before authentication/authorization if claims are stored in session)
app.UseSession();

// 4. Authentication (verifies who the user is)
app.UseAuthentication();

// 5. Authorization (verifies what the user can do)
app.UseAuthorization();

// 6. Endpoint mappings (MapGet, MapControllerRoute)
app.MapGet("Account/Login", async context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        try
        {
            var claimsDict = context.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            context.Session.SetString("UserClaims", System.Text.Json.JsonSerializer.Serialize(claimsDict));

            foreach (var claim in context.User.Claims)
            {
                Log.Information("Claim: {Type} = {Value}", claim.Type, claim.Value);
            }
            context.Response.Redirect("/");
            return;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in SAMLLogin during claim processing.");
            context.Response.Redirect("/account/login?error=" + Uri.EscapeDataString("SAML login failed: " + ex.Message));
            return;
        }
    }
    await context.ChallengeAsync("Saml2");
});

app.MapGet("account/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- Application Run ---
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

// --- Class definition (if not in a separate file) ---
