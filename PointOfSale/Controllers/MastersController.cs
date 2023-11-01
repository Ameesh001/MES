﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PointOfSale.Business.Contracts;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using PointOfSale.Business.Services;
using System.Data;


namespace PointOfSale.Controllers
{
    [Authorize]
    public class MastersController : Controller
    {
        private readonly IStyleService _styleService;
		private readonly ISizeService _sizeService;
		private readonly IColourService _colourService;
        private readonly IDesignService _designService;
        private readonly IBankService _bankService;

        private readonly IArticalService _articalService;
        private readonly IMapper _mapper;
        public MastersController(ISizeService sizeService,IBankService bankService, IDesignService designService ,IStyleService categoryService, IColourService colourService, IArticalService articalService, IMapper mapper)
        {
            _styleService = categoryService;
			_colourService = colourService;
            _articalService = articalService;
            _designService = designService;
            _bankService = bankService;
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public IActionResult Style()
        {
            return View();
        }

		

		[HttpGet]
        public async Task<IActionResult> GetStyles()
        {

            List<VMStyle> VMStyleList = _mapper.Map<List<VMStyle>>(await _styleService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = VMStyleList });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStyle([FromBody] VMStyle model)
        {
            GenericResponse<VMStyle> gResponse = new GenericResponse<VMStyle>();
            try
            {
                Style category_created = await _styleService.Add(_mapper.Map<Style>(model));

                model = _mapper.Map<VMStyle>(category_created);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStyle([FromBody] VMStyle model)
        {
            GenericResponse<VMStyle> gResponse = new GenericResponse<VMStyle>();
            try
            {

                Style edited_category = await _styleService.Edit(_mapper.Map<Style>(model));

                model = _mapper.Map<VMStyle>(edited_category);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteStyle(int idCategory)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _styleService.Delete(idCategory);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


		///////////////////////////////////<Colour Working>//////////////////////////////////////

		public IActionResult Colour()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetColours()
		{

			List<VMColour> VMColourList = _mapper.Map<List<VMColour>>(await _colourService.List());
			return StatusCode(StatusCodes.Status200OK, new { data = VMColourList });
		}

		[HttpPost]
		public async Task<IActionResult> CreateColour([FromBody] VMColour model)
		{
			GenericResponse<VMColour> gResponse = new GenericResponse<VMColour>();
			try
			{
				Colour category_created = await _colourService.Add(_mapper.Map<Colour>(model));

				model = _mapper.Map<VMColour>(category_created);

				gResponse.State = true;
				gResponse.Object = model;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateColour([FromBody] VMColour model)
		{
			GenericResponse<VMColour> gResponse = new GenericResponse<VMColour>();
			try
			{

				Colour edited_category = await _colourService.Edit(_mapper.Map<Colour>(model));

				model = _mapper.Map<VMColour>(edited_category);

				gResponse.State = true;
				gResponse.Object = model;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}


		[HttpDelete]
		public async Task<IActionResult> DeleteColour(int idCategory)
		{
			GenericResponse<string> gResponse = new GenericResponse<string>();
			try
			{
				gResponse.State = await _colourService.Delete(idCategory);
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}
        ///////////////////////////////////</Colour Working>//////////////////////////////////////
        

        ///////////////////////////////////<Artical Working>//////////////////////////////////////

        public IActionResult Artical()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetArticals()
        {
            List<VMArtical> VMArticalList = _mapper.Map<List<VMArtical>>(await _articalService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = VMArticalList });
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtical([FromBody] VMArtical model)
        {
            GenericResponse<VMArtical> gResponse = new GenericResponse<VMArtical>();
            try
            {
                Artical category_created = await _articalService.Add(_mapper.Map<Artical>(model));

                model = _mapper.Map<VMArtical>(category_created);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArtical([FromBody] VMArtical model)
        {
            GenericResponse<VMArtical> gResponse = new GenericResponse<VMArtical>();
            try
            {

                Artical edited_category = await _articalService.Edit(_mapper.Map<Artical>(model));

                model = _mapper.Map<VMArtical>(edited_category);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteArtical(int idCategory)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _articalService.Delete(idCategory);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        ///////////////////////////////////</Artical Working>//////////////////////////////////////


        ///////////////////////////////////<Design Working>//////////////////////////////////////

        public IActionResult Design()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDesigns()
        {
            List<VMDesign> VMDesignList = _mapper.Map<List<VMDesign>>(await _designService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = VMDesignList });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDesign([FromBody] VMDesign model)
        {
            GenericResponse<VMDesign> gResponse = new GenericResponse<VMDesign>();
            try
            {
                Design category_created = await _designService.Add(_mapper.Map<Design>(model));

                model = _mapper.Map<VMDesign>(category_created);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDesign([FromBody] VMDesign model)
        {
            GenericResponse<VMDesign> gResponse = new GenericResponse<VMDesign>();
            try
            {

                Design edited_category = await _designService.Edit(_mapper.Map<Design>(model));

                model = _mapper.Map<VMDesign>(edited_category);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteDesign(int idCategory)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _designService.Delete(idCategory);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
		///////////////////////////////////</Design Working>//////////////////////////////////////
		///////////////////////////////////<Bank Working>//////////////////////////////////////

		public IActionResult Bank()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetBanks()
		{
			List<VMBank> VMBankList = _mapper.Map<List<VMBank>>(await _bankService.List());
			return StatusCode(StatusCodes.Status200OK, new { data = VMBankList });
		}

		[HttpPost]
		public async Task<IActionResult> CreateBank([FromBody] VMBank model)
		{
			GenericResponse<VMBank> gResponse = new GenericResponse<VMBank>();
			try
			{
				Bank category_created = await _bankService.Add(_mapper.Map<Bank>(model));

				model = _mapper.Map<VMBank>(category_created);

				gResponse.State = true;
				gResponse.Object = model;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateBank([FromBody] VMBank model)
		{
			GenericResponse<VMBank> gResponse = new GenericResponse<VMBank>();
			try
			{

				Bank edited_category = await _bankService.Edit(_mapper.Map<Bank>(model));

				model = _mapper.Map<VMBank>(edited_category);

				gResponse.State = true;
				gResponse.Object = model;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}


		[HttpDelete]
		public async Task<IActionResult> DeleteBank(int idCategory)
		{
			GenericResponse<string> gResponse = new GenericResponse<string>();
			try
			{
				gResponse.State = await _bankService.Delete(idCategory);
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}
		///////////////////////////////////</Bank Working>//////////////////////////////////////

		///////////////////////////////////<Bank Working>//////////////////////////////////////

		public IActionResult Size()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSizes()
        {
            List<VMSize> VMSizeList = _mapper.Map<List<VMSize>>(await _sizeService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = VMSizeList });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] VMSize model)
        {
            GenericResponse<VMSize> gResponse = new GenericResponse<VMSize>();
            try
            {
                Size category_created = await _sizeService.Add(_mapper.Map<Size>(model));

                model = _mapper.Map<VMSize>(category_created);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSize([FromBody] VMSize model)
        {
            GenericResponse<VMSize> gResponse = new GenericResponse<VMSize>();
            try
            {

                Size edited_category = await _sizeService.Edit(_mapper.Map<Size>(model));

                model = _mapper.Map<VMSize>(edited_category);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteSize(int idCategory)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.State = await _sizeService.Delete(idCategory);
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        ///////////////////////////////////</Bank Working>//////////////////////////////////////

    }
}
