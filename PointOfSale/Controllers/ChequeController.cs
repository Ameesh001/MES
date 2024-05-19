using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PointOfSale.Business.Contracts;
using PointOfSale.Business.Services;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Logger;
using PointOfSale.Utilities.Response;

namespace PointOfSale.Controllers
{
    [Authorize]
    public class ChequeController : Controller
    {
        private readonly IBankService _bankService;
        private readonly IChequeService _ChequeService;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        Loggers log = new Loggers();
        public ChequeController(IChequeService ChequeService, IBankService bankService, IMapper mapper, IConverter converter)
        {
            _ChequeService = ChequeService;
            _mapper = mapper;
            _converter = converter;
            _bankService = bankService;
        }

        public IActionResult Cheque()
        {
            log.LogWriter("Cheque");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBanks()
        {
            List<VMBank> vmBankList = _mapper.Map<List<VMBank>>(await _bankService.ListActive());
            return StatusCode(StatusCodes.Status200OK, new { data = vmBankList });
        }


        [HttpGet]
        public async Task<IActionResult> GetCheque()
        {
            log.LogWriter("GetCheque");
            try
            {
                List<VMCheque> vmChequeList = _mapper.Map<List<VMCheque>>(await _ChequeService.List());
                return StatusCode(StatusCodes.Status200OK, new { data = vmChequeList });
            }
            catch (Exception ex)
            {

                log.LogWriter("GetCheque ex:" + ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheque([FromForm] IFormFile photo, [FromForm] string model)
        {

            log.LogWriter("CreateCheque");
            GenericResponse<VMCheque> gResponse = new GenericResponse<VMCheque>();
            try
            {
                VMCheque vmCheque = JsonConvert.DeserializeObject<VMCheque>(model);

                //vmCheque.userID = HttpContext.Session.GetInt32("UserID");
                Cheque Cheque_created = await _ChequeService.Add(_mapper.Map<Cheque>(vmCheque));

                vmCheque = _mapper.Map<VMCheque>(Cheque_created);

                gResponse.State = true;
                gResponse.Object = vmCheque;

                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;

                log.LogWriter("CreateCheque Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);


        }

        [HttpPut]
        public async Task<IActionResult> EditCheque([FromForm] IFormFile photo, [FromForm] string model)
        {
            log.LogWriter("EditCheque");
            GenericResponse<VMCheque> gResponse = new GenericResponse<VMCheque>();
            try
            {
                VMCheque vmCheque = JsonConvert.DeserializeObject<VMCheque>(model);
                //vmCheque.userID = HttpContext.Session.GetInt32("UserID"); 

                Cheque Cheque_edited = await _ChequeService.Edit(_mapper.Map<Cheque>(vmCheque));

                vmCheque = _mapper.Map<VMCheque>(Cheque_edited);

                gResponse.State = true;
                gResponse.Object = vmCheque;

                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("EditCheque Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCheque(int idCheque)
        {
            log.LogWriter("DeleteCheque");
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _ChequeService.Delete(idCheque);
                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("DeleteCheque Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public IActionResult GetCoordinates(string idBank)
        {
            var VMBankList = _ChequeService.GetSingle(idBank);
           // var VMBankList = _ChequeService.GetSingleData(idBank);
            return StatusCode(StatusCodes.Status200OK, new { data = VMBankList });
        }
        public IActionResult ShowPDFCheque(int chequeID)
        {
            try
            {
                string urlTemplateView = $"{this.Request.Scheme}://{this.Request.Host}/Template/PDFCheque?recordID={chequeID}";

                log.LogWriter("ShowPDF url: " + urlTemplateView);

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = new GlobalSettings()
                    {
                        PaperSize = PaperKind.A4,
                        Orientation = Orientation.Portrait,
                        Margins = new MarginSettings() { Top = 0, Left = 0, Right = 0, Bottom = 0}
                    },
                    Objects = {
                    new ObjectSettings(){
                        Page = urlTemplateView
                    }
                }

                };
                var archivoPDF = _converter.Convert(pdf);
                return File(archivoPDF, "application/pdf");
            }
            catch (Exception ex)
            {
                log.LogWriter("ShowPDFSale Exception: " + ex);
                throw;
            }
        }
    }
}
