using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
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
    public class SetupController : Controller
    {
        //private readonly IBankService _bankService;
        private readonly IGatePassService _GatePassService;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        Loggers log = new Loggers();
        public SetupController(IGatePassService GatePassService, IMapper mapper, IConverter converter)
        {
            // _bankService = bankService;
            _GatePassService = GatePassService;
            _mapper = mapper;
            _converter = converter;
        }

        public IActionResult GatePass()
        {
            log.LogWriter("GatePass");
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetBanks()
        //{
        //	List<VMBank> vmBankList = _mapper.Map<List<VMBank>>(await _bankService.List());
        //	return StatusCode(StatusCodes.Status200OK, new { data = vmBankList });
        //}


        [HttpGet]
        public async Task<IActionResult> GetGatePass()
        {
            log.LogWriter("GetGatePass");
            try
            {
                List<VMGatePass> vmGatePassList = _mapper.Map<List<VMGatePass>>(await _GatePassService.List());
                return StatusCode(StatusCodes.Status200OK, new { data = vmGatePassList });
            }
            catch (Exception ex)
            {

                log.LogWriter("GetGatePass ex:" + ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGatePass([FromForm] IFormFile photo, [FromForm] string model)
        {

            log.LogWriter("CreateGatePass");
            GenericResponse<VMGatePass> gResponse = new GenericResponse<VMGatePass>();
            try
            {
                VMGatePass vmGatePass = JsonConvert.DeserializeObject<VMGatePass>(model);

                //vmGatePass.userID = HttpContext.Session.GetInt32("UserID");
                GatePass GatePass_created = await _GatePassService.Add(_mapper.Map<GatePass>(vmGatePass));

                vmGatePass = _mapper.Map<VMGatePass>(GatePass_created);

                gResponse.State = true;
                gResponse.Object = vmGatePass;

                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;

                log.LogWriter("CreateGatePass Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);


        }

        [HttpPut]
        public async Task<IActionResult> EditGatePass([FromForm] IFormFile photo, [FromForm] string model)
        {
            log.LogWriter("EditGatePass");
            GenericResponse<VMGatePass> gResponse = new GenericResponse<VMGatePass>();
            try
            {
                VMGatePass vmGatePass = JsonConvert.DeserializeObject<VMGatePass>(model);
                //vmGatePass.userID = HttpContext.Session.GetInt32("UserID"); 

                GatePass GatePass_edited = await _GatePassService.Edit(_mapper.Map<GatePass>(vmGatePass));

                vmGatePass = _mapper.Map<VMGatePass>(GatePass_edited);

                gResponse.State = true;
                gResponse.Object = vmGatePass;

                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("EditGatePass Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGatePass(int idGatePass)
        {
            log.LogWriter("DeleteGatePass");
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _GatePassService.Delete(idGatePass);
                log.LogWriter("gResponse.State: " + gResponse.State);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                log.LogWriter("DeleteGatePass Exception: " + ex);
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        public IActionResult ShowPDFSale(int saleNumber)
        {
            try
            {
                string urlTemplateView = $"{this.Request.Scheme}://{this.Request.Host}/Template/PDFGatePass?recordID={saleNumber}";

                log.LogWriter("ShowPDFSale url: " + urlTemplateView);

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = new GlobalSettings()
                    {
                        PaperSize = PaperKind.A4,
                        Orientation = Orientation.Portrait
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
