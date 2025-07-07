using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SME_WEB_ApiManagement.DAO;
using SME_WEB_ApiManagement.Models;
using SME_WEB_ApiManagement.Services;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SME_WEB_ApiManagement.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserManagementController> _logger;
        protected static string API_Path_Main;
        protected static string API_Path_Sub;
        protected static string API_Path_Trigger;
        protected static string API_Path_Sub_Trigger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CallAPIService _callAPIService;
        protected int currentPageNumber;
        protected static int PageSize;
        protected static int PageSizMedium;
        public UserManagementController(ILogger<UserManagementController> logger, IConfiguration configuration,
 IWebHostEnvironment webHostEnvironment
 , CallAPIService callAPIService)
        {
            _logger = logger;
            _configuration = configuration;
            API_Path_Main = _configuration.GetValue<string>("API_Path_Main");
            API_Path_Sub = _configuration.GetValue<string>("API_Path_Sub");
            API_Path_Trigger = _configuration.GetValue<string>("API_Path_Trigger");
            API_Path_Sub_Trigger = _configuration.GetValue<string>("API_Path_Sub_Trigger");
            PageSize = _configuration.GetValue<Int32>("PageSize");
            PageSizMedium = _configuration.GetValue<Int32>("PageSizMedium");
            currentPageNumber = 1;
            _webHostEnvironment = webHostEnvironment;
            _callAPIService = callAPIService;

        }
        // For demonstration, use static lists. Replace with your DAO or database logic.


        public async Task<IActionResult> Index()
        {
            ViewBag.EmpDetail = HttpContext.Session.GetString("EmpDetail");
            var empDetailJson = HttpContext.Session.GetString("EmpDetail");

            if (!string.IsNullOrEmpty(empDetailJson))
            {
                var empDetailObj = JsonSerializer.Deserialize<EmployeeRoleModels>(empDetailJson);
                if (empDetailObj == null || string.IsNullOrEmpty(empDetailObj.RoleCode))
                {
                    if (empDetailObj.RoleCode == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ViewEmployeeRoleModels();
            try
            {
                var EmpRolelist = EmployeeDAO.GetEmpAllRole(API_Path_Main + API_Path_Sub, null);

                if (EmpRolelist == null || !EmpRolelist.Any())
                {
                    _logger.LogWarning("No employee roles found.");
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }
                else
                {
                    model.AdminUsers = EmpRolelist.Where(u => u.EmployeeRole == "ADMIN").ToList();
                    model.SuperAdminUsers = EmpRolelist.Where(u => u.EmployeeRole == "SUPERADMIN").ToList();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the User Management page.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(int id)
        {
            // ลบข้อมูลจากฐานข้อมูลหรือ DAO
            // สมมติ EmployeeDAO.DeleteUserById(id);
            bool result = await EmployeeDAO.DeleteUserById(id, API_Path_Main + API_Path_Sub, null);
            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "ไม่พบผู้ใช้งานหรือเกิดข้อผิดพลาด" });
        }


        [HttpPost]
        public async Task<IActionResult> GetAllUserPopup(searchEmployeeRoleModels model)
        {
            var vm = new ViewEmployeeRoleModels();
            var serviceCenter = new ServiceCenter(_configuration, _callAPIService);
            vm.DDLDepartment = await serviceCenter.GetDdlDepartment(API_Path_Main + API_Path_Sub, "business-units");
            ViewBag.DDLDepartment = new SelectList(vm.DDLDepartment.DropdownList.OrderBy(x => x.Code), "Code", "Name");
        
            try
            {
                var EmpRolelist = await EmployeeDAO.GetEmpByBu(model, API_Path_Main + API_Path_Sub, null);
                vm=EmpRolelist;
                return PartialView("_UserSelectModal", vm);
           //     return PartialView("_TestInsertUserModal", vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return Json(new { success = false, message = "An error occurred while retrieving users." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> insertUserPopup([FromBody] List<EmployeeRoleModels> model)
        {
            var vm = new ViewEmployeeRoleModels();
            try
            {
                if (model != null && model.Count > 0)
                {
                    foreach (var emp in model)
                    {
                        await EmployeeDAO.InsrtRoleEmpByBu(emp, API_Path_Main + API_Path_Sub, null);
                    }
                }
                // โหลดข้อมูลใหม่หลัง insert ถ้าต้องการ
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting users.");
                return Json(new { success = false, message = "An error occurred while inserting users." });
            }
        }
        // GET: UserManagement/Setting/{employeeId}
       
        public async Task<ActionResult> Setting(string employeeId)
        {
            //decode base64

            string DecodeEmpId = Service_CenterDAO.DecryptBase64(employeeId);
            EmployeeModels empid = new EmployeeModels
            {
                EmployeeId = DecodeEmpId
            };
         var empdetail=   EmployeeDAO.GetDetaiEmp(empid, API_Path_Main + API_Path_Sub);
            var allSystems = SystemDAO.GetSystem(API_Path_Main + API_Path_Sub, null)
                .Select(system => new TEmployeeMapSystemModels
                {
                    SystemApiId = system.Id,
                    EmployeeId = DecodeEmpId,
                    FlagActive = system.FlagActive,
                    CreateBy = HttpContext.Session.GetString("EmployeeId"),
                    CreateDate = system.CreateDate,
                    UpdateBy =  HttpContext.Session.GetString("EmployeeId"),
                    UpdateDate = system.UpdateDate
                    ,SystemApiName = system.SystemName
                }).ToList(); var mappedSystems =  SystemDAO.GetTEmpSystemByempId(DecodeEmpId, API_Path_Main + API_Path_Sub); // List<TEmployeeMapSystemModels>

            var model = new UserSystemSettingViewModel
            {
                EmployeeId = DecodeEmpId,
                FirstNameTh = empdetail.Result.FirstNameTh,
                LastNameTh = empdetail.Result.LastNameTh,
              
                AllSystems = allSystems,
                SelectedSystemIds = mappedSystems.Where(m => m.SystemApiId.HasValue).Select(m => m.SystemApiId.Value).ToList()
            };

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
   
        public async Task<ActionResult> Setting(UserSystemSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Remove old mappings
                //await _employeeService.RemoveAllEmployeeMapSystemsAsync(model.EmployeeId);

                // Add new mappings
                string DecodeEmpId = Service_CenterDAO.DecryptBase64(model.EmployeeId);
                List<TEmployeeMapSystemModels> Mdata = new List<TEmployeeMapSystemModels>();
                if (model.SelectedSystemIds != null)
                {
                    foreach (var systemId in model.SelectedSystemIds)
                    {
                        var map = new TEmployeeMapSystemModels
                        {
                            EmployeeId = DecodeEmpId,
                            SystemApiId = systemId,
                            FlagActive = true,
                            CreateBy = User.Identity?.Name,
                            CreateDate = DateTime.Now
                        };
                        Mdata.Add(map);
                       
                    }
                }
                var result = SystemDAO.AddEmployeeMapSystemAsync(Mdata, API_Path_Main + API_Path_Sub, null);
                if (result.HasValue && result.Value > 0)
                {
                    TempData["Message"] = "บันทึกสำเร็จ";
                    // Reload the page by redirecting to the same Setting action
                    return RedirectToAction("Setting", new { employeeId = model.EmployeeId });
                }
                else
                {
                    _logger.LogError("Failed to add employee map system.");
                    ModelState.AddModelError("", "An error occurred while saving the data.");
                }
            }

            // Reload systems if model state is invalid
         //   model.AllSystems = await _systemApiService.GetAllSystemsAsync();


            return View(model);
        }
    }
}
