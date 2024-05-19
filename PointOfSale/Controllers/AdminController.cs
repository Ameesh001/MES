using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PointOfSale.Business.Contracts;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Logger;
using PointOfSale.Utilities.Response;

namespace PointOfSale.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRolService _rolService;
        private readonly IDashBoardService _dashboardService;
        private readonly IMapper _mapper;
        Loggers log = new Loggers();

        public AdminController(IDashBoardService dashboardService, IUserService userService, IRolService rolService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _userService = userService;
            _rolService = rolService;
            _mapper = mapper;
        }

        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult GateDashBoard()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public async Task<IActionResult> GetGateSummary()
        {
            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();

            try
            {
                VMDashBoard vmDashboard = new VMDashBoard();

                //vmDashboard.TotalSales = await _dashboardService.TotalSalesLastWeek();
                vmDashboard.TotalSales = await _dashboardService.SameDayPasses();
                vmDashboard.TotalIncome = await _dashboardService.SuccessReturned();
                //vmDashboard.TotalIncome = await _dashboardService.TotalIncomeLastWeek();
                vmDashboard.TotalProducts = await _dashboardService.TodayPending();
                //vmDashboard.TotalCategories = await _dashboardService.TotalCategories();
                vmDashboard.TotalCategories = await _dashboardService.TotalPending();

                List<VMSalesWeek> listSalesWeek = new List<VMSalesWeek>();
                List<VMProductsWeek> ProductListWeek = new List<VMProductsWeek>();

                foreach (KeyValuePair<string, int> item in await _dashboardService.PassesLastWeek())
                {
                    listSalesWeek.Add(new VMSalesWeek()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }
                //foreach (KeyValuePair<string, int> item in await _dashboardService.SalesLastWeek())
                //{
                //    listSalesWeek.Add(new VMSalesWeek()
                //    {
                //        Date = item.Key,
                //        Total = item.Value
                //    });
                //}
                foreach (KeyValuePair<string, int> item in await _dashboardService.TopStatuses())
                {
                    ProductListWeek.Add(new VMProductsWeek()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }
                //foreach (KeyValuePair<string, int> item in await _dashboardService.ProductsTopLastWeek())
                //{
                //    ProductListWeek.Add(new VMProductsWeek()
                //    {
                //        Product = item.Key,
                //        Quantity = item.Value
                //    });
                //}

                vmDashboard.SalesLastWeek = listSalesWeek;
                vmDashboard.ProductsTopLastWeek = ProductListWeek;

                gResponse.State = true;
                gResponse.Object = vmDashboard;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;

                log.LogWriter("GetGateSummary Exception:" + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();

            try
            {
                VMDashBoard vmDashboard = new VMDashBoard();

                vmDashboard.TotalSales = await _dashboardService.TotalSalesLastWeek();
                // vmDashboard.TotalSales = await _dashboardService.SameDayPasses();
                // vmDashboard.TotalIncome = await _dashboardService.TotalIncomeLastWeek();
                vmDashboard.TotalProducts = await _dashboardService.TotalProducts();
                vmDashboard.TotalCategories = await _dashboardService.TotalCategories();

                List<VMSalesWeek> listSalesWeek = new List<VMSalesWeek>();
                List<VMProductsWeek> ProductListWeek = new List<VMProductsWeek>();

                foreach (KeyValuePair<string, int> item in await _dashboardService.SalesLastWeek())
                {
                    listSalesWeek.Add(new VMSalesWeek()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }
                foreach (KeyValuePair<string, int> item in await _dashboardService.ProductsTopLastWeek())
                {
                    ProductListWeek.Add(new VMProductsWeek()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }

                vmDashboard.SalesLastWeek = listSalesWeek;
                vmDashboard.ProductsTopLastWeek = ProductListWeek;

                gResponse.State = true;
                gResponse.Object = vmDashboard;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;

                log.LogWriter("GetSummary Exception:" + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                List<VMRol> listRoles = _mapper.Map<List<VMRol>>(await _rolService.List());
                return StatusCode(StatusCodes.Status200OK, listRoles);
            }
            catch (Exception ex)
            {
                log.LogWriter("GetRoles Exception:" + ex);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<VMUser> listUsers = _mapper.Map<List<VMUser>>(await _userService.List());
                return StatusCode(StatusCodes.Status200OK, new { data = listUsers });
            }
            catch (Exception ex)
            {
                log.LogWriter("GetUsers Exception:" + ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<VMUser> gResponse = new GenericResponse<VMUser>();
            try
            {
                VMUser vmUser = JsonConvert.DeserializeObject<VMUser>(model);

                if (photo != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        photo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        vmUser.Photo = fileBytes;
                    }
                }
                else
                    vmUser.Photo = null;


                User usuario_creado = await _userService.Add(_mapper.Map<User>(vmUser));

                vmUser = _mapper.Map<VMUser>(usuario_creado);

                gResponse.State = true;
                gResponse.Object = vmUser;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("CreateUser Exception:" + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] IFormFile photo, [FromForm] string model)
        {
            GenericResponse<VMUser> gResponse = new GenericResponse<VMUser>();
            try
            {
                VMUser vmUser = JsonConvert.DeserializeObject<VMUser>(model);

                if (photo != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        photo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        vmUser.Photo = fileBytes;
                    }
                }

                User user_edited = await _userService.Edit(_mapper.Map<User>(vmUser));

                vmUser = _mapper.Map<VMUser>(user_edited);

                gResponse.State = true;
                gResponse.Object = vmUser;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("UpdateUser Exception:" + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int IdUser)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _userService.Delete(IdUser);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("DeleteUser Exception:" + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


    }
}
