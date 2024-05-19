using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Business.Contracts;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Logger;

namespace PointOfSale.Controllers
{
    public class TemplateController : Controller
    {
        //private readonly ISaleService _saleService;
        private readonly IGatePassService _gatePassService;
        private readonly IChequeService _chequeService;
        private readonly IMapper _mapper;
        Loggers log = new Loggers();
        public TemplateController(IMapper mapper, IGatePassService gatePassService, IChequeService chequeService)
        {
            //_saleService = saleService;
            _mapper = mapper;
            _gatePassService = gatePassService;
            _chequeService = chequeService;
        }
        //public async Task<IActionResult> PDFSale(string saleNumber)
        //{
        //    VMSale vmVenta = _mapper.Map<VMSale>(await _saleService.Detail(saleNumber));

        //    return View(vmVenta);
        //}

        public async Task<IActionResult> PDFGatePass(int recordID)
        {
            try
            {
                log.LogWriter("PDFGatePass");
                VMGatePass vmVenta = _mapper.Map<VMGatePass>(await _gatePassService.GetSingle(recordID));
                return View(vmVenta);
            }
            catch (Exception ex)
            {
                log.LogWriter("PDFGatePass Exception: " + ex);
                throw;
            }

        }

        public async Task<IActionResult> PDFCheque(int recordID)
        {
            try
            {
                log.LogWriter("PDFCheque");
                VMCheque vmVenta = _mapper.Map<VMCheque>(await _chequeService.GetSinglePDF(recordID));
                return View(vmVenta);
            }
            catch (Exception ex)
            {
                log.LogWriter("PDFCheque Exception: " + ex);
                throw;
            }

        }
    }
}
