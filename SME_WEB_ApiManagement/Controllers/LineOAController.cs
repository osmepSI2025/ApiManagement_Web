using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models; // สมมติว่า Project Name คือ LINE_OA_Login_MVC
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace SME_WEB_ApiManagement.Controllers
{
    public class LineOAController : Controller
    {
        private readonly LineLoginSettings _lineSettings;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LineOAController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
      
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        // Dependency Injection สำหรับ LineLoginSettings และ HttpClient
        public LineOAController(IOptions<LineLoginSettings> lineSettings, HttpClient httpClient, ILogger<LineOAController> logger, IConfiguration configuration)
        {
            _lineSettings = lineSettings.Value;
            _httpClient = httpClient;
            _logger = logger; // Assign the logger parameter to the _logger field
            _configuration = configuration; // Assign the configuration parameter to the _configuration field
            API_Path_Main = _configuration.GetValue<string>("API_Path_Main");
            API_Path_Sub = _configuration.GetValue<string>("API_Path_Sub");
            API_Path_Trigger = _configuration.GetValue<string>("API_Path_Trigger");
            API_Path_Sub_Trigger = _configuration.GetValue<string>("API_Path_Sub_Trigger");
            PageSize = _configuration.GetValue<Int32>("PageSize");
            PageSizMedium = _configuration.GetValue<Int32>("PageSizMedium");
            currentPageNumber = 1;
        }
        // Action สำหรับเริ่มต้นการ Login
        public IActionResult LineLogin()
        {
            // สร้าง URL สำหรับ Redirect ไปยัง LINE Login
            var redirectUri = _lineSettings.CallbackUrl;
            var channelId = _lineSettings.ChannelId;
            var state = Guid.NewGuid().ToString(); // สร้าง state เพื่อป้องกัน CSRF
            var scope = "profile openid"; // ขอสิทธิ์เข้าถึงโปรไฟล์และ ID Token

            // เก็บ state ไว้ใน Session เพื่อตรวจสอบภายหลัง
            HttpContext.Session.SetString("LineLoginState", state);

            var lineAuthUrl = $"https://access.line.me/oauth2/v2.1/authorize?" +
                              $"response_type=code&" +
                              $"client_id={channelId}&" +
                              $"redirect_uri={redirectUri}&" +
                              $"state={state}&" +
                              $"scope={scope}";

            return Redirect(lineAuthUrl);
        }
        // Action สำหรับ Callback จาก LINE
        public async Task<IActionResult> LineCallback(string code, string state, string error, string error_description)
        {
            // ตรวจสอบ Error จาก LINE (เช่น ผู้ใช้ไม่อนุญาต)
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = $"LINE Error: {error} - {error_description}";
                return View("Error"); // หรือ redirect ไปหน้า Error ที่คุณสร้างไว้
            }

            // ตรวจสอบ State เพื่อป้องกัน CSRF Attack
            var storedState = HttpContext.Session.GetString("LineLoginState");
            if (string.IsNullOrEmpty(state) || state != storedState)
            {
                ViewBag.Error = "Invalid state parameter. Possible CSRF attack.";
                return View("Error");
            }

            // 1. แลกเปลี่ยน 'code' เป็น Access Token
            var tokenRequestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _lineSettings.CallbackUrl),
                new KeyValuePair<string, string>("client_id", _lineSettings.ChannelId),
                new KeyValuePair<string, string>("client_secret", _lineSettings.ChannelSecret)
            });

            var tokenResponse = await _httpClient.PostAsync("https://api.line.me/oauth2/v2.1/token", tokenRequestBody);
            tokenResponse.EnsureSuccessStatusCode(); // ตรวจสอบว่า API call สำเร็จ
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            var lineToken = JsonSerializer.Deserialize<LineAccessTokenResponse>(tokenContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (lineToken == null || string.IsNullOrEmpty(lineToken.Access_token))
            {
                ViewBag.Error = "Failed to get access token from LINE.";
                return View("Error");
            }

            // 2. ดึงข้อมูลโปรไฟล์ผู้ใช้ด้วย Access Token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", lineToken.Access_token);
            var profileResponse = await _httpClient.GetAsync("https://api.line.me/v2/profile");
            profileResponse.EnsureSuccessStatusCode();
            var profileContent = await profileResponse.Content.ReadAsStringAsync();
            var lineProfile = JsonSerializer.Deserialize<LineProfileResponse>(profileContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (lineProfile == null || string.IsNullOrEmpty(lineProfile.UserId))
            {
                ViewBag.Error = "Failed to get user profile from LINE.";
                return View("Error");
            }

            var member = new EmployeeLineOAModels
            {
                LineOaUserId = lineProfile.UserId,
                LineOaDisplayName = lineProfile.DisplayName,
                LineOaPictureUrl = lineProfile.PictureUrl,
                LineOaAccessToken = lineToken.Access_token,
                LineOaRefreshToken = lineToken.Refresh_token, // ถ้ามี refresh token
                LineOaDateJoined = DateTime.Now,
                LineOaChannelId = _lineSettings.ChannelId,
                EmployeeId = HttpContext.Session.GetString("EmployeeId"), // สมมติว่าเก็บ EmployeeId ไว้ใน Session ก่อนหน้านี้
            };

            await LineOaDAO.InsrtLineOaEmp(member, _lineSettings.DBEmployeeLineOa, null);
            // ตัวอย่าง: เก็บข้อมูลสมาชิกใน Session (ไม่แนะนำสำหรับ Production จริง)
            HttpContext.Session.SetString("LineUserId", member.LineOaUserId);
            HttpContext.Session.SetString("DisplayName", member.LineOaDisplayName);
            HttpContext.Session.SetString("PictureUrl", member.LineOaPictureUrl);
            HttpContext.Session.SetString("IsLoggedIn", "true"); // ตั้งค่าว่าล็อกอินแล้ว

         

            ViewBag.Member = member; // ส่งข้อมูลให้ View แสดงผล
            return View("Login/LoginSuccess"); // Redirect ไปยังหน้าแสดงผล Login สำเร็จ
        }

    }
}
