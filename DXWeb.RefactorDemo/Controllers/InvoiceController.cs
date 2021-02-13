using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DXWeb.RefactorDemo.Models;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Controllers
{
	[Route("api/[controller]/[action]")]
	public class InvoiceController : BaseController<int, DTOInvoice, InvoiceStore>
	{
		readonly IDataStore<int, DTOCustomer> customerStore;
		public InvoiceController(IDataStore<int, DTOInvoice> mainDataStore, IDataStore<int, DTOCustomer> customerStore) : base(mainDataStore)
		{
			this.customerStore = customerStore;
		}

		[HttpGet]
		public async override Task<IActionResult> Get(DataSourceLoadOptions loadOptions) => await base.Get(loadOptions);

		[HttpPost]
		public async override Task<IActionResult> Post(string values) => await base.Post(values);

		[HttpPut]
		public async override Task<IActionResult> Put(int key, string values) => await base.Put(key, values);

		[HttpDelete]
		public async override Task Delete(int key) => await base.Delete(key);


		[HttpGet]
		public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions)
		{
			var lookup = customerStore.Query<DTOCustomerLookup>();
			return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
		}

	}
}