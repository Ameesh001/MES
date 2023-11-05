using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PointOfSale.Business.Contracts;
using PointOfSale.Business.Services;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Response;
using System.Data;

namespace PointOfSale.Controllers
{
    [Authorize]
    public class SetupController : Controller
    {
		private readonly IBankService _bankService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
		public SetupController(ICustomerService customerService, IBankService bankService,IMapper mapper)
		{
            _bankService = bankService;
            _customerService = customerService;
			_mapper = mapper;
		}

		public IActionResult Customers()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetBanks()
		{
			List<VMBank> vmBankList = _mapper.Map<List<VMBank>>(await _bankService.List());
			return StatusCode(StatusCodes.Status200OK, new { data = vmBankList });
		}

		
		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			List<VMCustomer> vmCustomerList = _mapper.Map<List<VMCustomer>>(await _customerService.List());
			return StatusCode(StatusCodes.Status200OK, new { data = vmCustomerList });
		}

		[HttpPost]
		public async Task<IActionResult> CreateCustomer([FromForm] IFormFile photo, [FromForm] string model)
		{
			GenericResponse<VMCustomer> gResponse = new GenericResponse<VMCustomer>();
			try
			{
				VMCustomer vmCustomer = JsonConvert.DeserializeObject<VMCustomer>(model);

				if (photo != null)
				{
					using (var ms = new MemoryStream())
					{
						photo.CopyTo(ms);
						var fileBytes = ms.ToArray();
						vmCustomer.Photo = fileBytes;
					}
				}
				else
					vmCustomer.Photo = null;

				Customer Customer_created = await _customerService.Add(_mapper.Map<Customer>(vmCustomer));

				vmCustomer = _mapper.Map<VMCustomer>(Customer_created);

				gResponse.State = true;
				gResponse.Object = vmCustomer;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}

		[HttpPut]
		public async Task<IActionResult> EditCustomer([FromForm] IFormFile photo, [FromForm] string model)
		{
			GenericResponse<VMCustomer> gResponse = new GenericResponse<VMCustomer>();
			try
			{
				VMCustomer vmCustomer = JsonConvert.DeserializeObject<VMCustomer>(model);

				if (photo != null)
				{
					using (var ms = new MemoryStream())
					{
						photo.CopyTo(ms);
						var fileBytes = ms.ToArray();
						vmCustomer.Photo = fileBytes;
					}
				}
				else
					vmCustomer.Photo = null;

				Customer Customer_edited = await _customerService.Edit(_mapper.Map<Customer>(vmCustomer));

				vmCustomer = _mapper.Map<VMCustomer>(Customer_edited);

				gResponse.State = true;
				gResponse.Object = vmCustomer;
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteCustomer(int IdProduct)
		{
			GenericResponse<string> gResponse = new GenericResponse<string>();
			try
			{
				gResponse.State = await _customerService.Delete(IdProduct);
			}
			catch (Exception ex)
			{
				gResponse.State = false;
				gResponse.Message = ex.Message;
			}

			return StatusCode(StatusCodes.Status200OK, gResponse);
		}

	}
}
