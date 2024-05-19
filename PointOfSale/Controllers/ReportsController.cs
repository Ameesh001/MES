using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PointOfSale.Business.Contracts;
using PointOfSale.Models;
using PointOfSale.Utilities.Logger;

namespace PointOfSale.Controllers
{
	[Authorize]
	public class ReportsController : Controller
	{
		private readonly ISaleService _saleService;
		private readonly IGatePassService _gateService;
		private readonly IMapper _mapper;
        Loggers log = new Loggers(); 
		public ReportsController(ISaleService saleService, IMapper mapper, IGatePassService gateService)
		{
			_saleService = saleService;
			_gateService = gateService;
			_mapper = mapper;
		}
		public IActionResult SalesReport()
		{
			return View();
		}
		public IActionResult GateReport()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ReportSale(string startDate, string endDate)
		{
			List<VMSalesReport> vmList = _mapper.Map<List<VMSalesReport>>(await _saleService.Report(startDate, endDate));
			return StatusCode(StatusCodes.Status200OK, new { data = vmList });
		}

		[HttpGet]
		public async Task<IActionResult> GatePassReport(string startDate, string endDate)
		{
			//var name = HttpContext.Session.GetString("UserName");
			//var age = HttpContext.Session.GetInt32("UserID").ToString();
			try
            {
                List<VMGatePass> vmList = _mapper.Map<List<VMGatePass>>(await _gateService.Report(startDate, endDate));

                log.LogWriter("GatePassReport StatusCodes" + StatusCodes.Status200OK);
                return StatusCode(StatusCodes.Status200OK, new { data = vmList });
			}
			catch (Exception ex)
            {
                log.LogWriter("GatePassReport Exception" + ex);
                throw;
            }
		}
	}
}
