using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
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
using DXWeb.RefactorDemo.Models.EF;

namespace DXWeb.RefactorDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    public class InvoicesController : Controller
    {
        private ChinookContext _context;

        public InvoicesController(ChinookContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var invoices = _context.Invoices.Select(i => new {
                i.InvoiceId,
                i.CustomerId,
                i.InvoiceDate,
                i.BillingAddress,
                i.BillingCity,
                i.BillingState,
                i.BillingCountry,
                i.BillingPostalCode,
                i.Total
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "InvoiceId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(invoices, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Invoice();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Invoices.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.InvoiceId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Invoices.FirstOrDefaultAsync(item => item.InvoiceId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Invoices.FirstOrDefaultAsync(item => item.InvoiceId == key);

            _context.Invoices.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Customers
                         orderby i.FirstName
                         select new {
                             Value = i.CustomerId,
                             Text = i.FirstName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Invoice model, IDictionary values) {
            string INVOICE_ID = nameof(Invoice.InvoiceId);
            string CUSTOMER_ID = nameof(Invoice.CustomerId);
            string INVOICE_DATE = nameof(Invoice.InvoiceDate);
            string BILLING_ADDRESS = nameof(Invoice.BillingAddress);
            string BILLING_CITY = nameof(Invoice.BillingCity);
            string BILLING_STATE = nameof(Invoice.BillingState);
            string BILLING_COUNTRY = nameof(Invoice.BillingCountry);
            string BILLING_POSTAL_CODE = nameof(Invoice.BillingPostalCode);
            string TOTAL = nameof(Invoice.Total);

            if(values.Contains(INVOICE_ID)) {
                model.InvoiceId = Convert.ToInt32(values[INVOICE_ID]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(INVOICE_DATE)) {
                model.InvoiceDate = Convert.ToDateTime(values[INVOICE_DATE]);
            }

            if(values.Contains(BILLING_ADDRESS)) {
                model.BillingAddress = Convert.ToString(values[BILLING_ADDRESS]);
            }

            if(values.Contains(BILLING_CITY)) {
                model.BillingCity = Convert.ToString(values[BILLING_CITY]);
            }

            if(values.Contains(BILLING_STATE)) {
                model.BillingState = Convert.ToString(values[BILLING_STATE]);
            }

            if(values.Contains(BILLING_COUNTRY)) {
                model.BillingCountry = Convert.ToString(values[BILLING_COUNTRY]);
            }

            if(values.Contains(BILLING_POSTAL_CODE)) {
                model.BillingPostalCode = Convert.ToString(values[BILLING_POSTAL_CODE]);
            }

            if(values.Contains(TOTAL)) {
                model.Total = Convert.ToDecimal(values[TOTAL], CultureInfo.InvariantCulture);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}