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
    public class CustomersController : Controller
    {
        private ChinookContext _context;

        public CustomersController(ChinookContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var customers = _context.Customers.Select(i => new {
                i.CustomerId,
                i.FirstName,
                i.LastName,
                i.Company,
                i.Address,
                i.City,
                i.State,
                i.Country,
                i.PostalCode,
                i.Phone,
                i.Fax,
                i.Email,
                i.SupportRepId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CustomerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(customers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Customer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CustomerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);
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
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);

            _context.Customers.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> EmployeesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Employees
                         orderby i.Title
                         select new {
                             Value = i.EmployeeId,
                             Text = i.Title
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Customer model, IDictionary values) {
            string CUSTOMER_ID = nameof(Customer.CustomerId);
            string FIRST_NAME = nameof(Customer.FirstName);
            string LAST_NAME = nameof(Customer.LastName);
            string COMPANY = nameof(Customer.Company);
            string ADDRESS = nameof(Customer.Address);
            string CITY = nameof(Customer.City);
            string STATE = nameof(Customer.State);
            string COUNTRY = nameof(Customer.Country);
            string POSTAL_CODE = nameof(Customer.PostalCode);
            string PHONE = nameof(Customer.Phone);
            string FAX = nameof(Customer.Fax);
            string EMAIL = nameof(Customer.Email);
            string SUPPORT_REP_ID = nameof(Customer.SupportRepId);

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(FIRST_NAME)) {
                model.FirstName = Convert.ToString(values[FIRST_NAME]);
            }

            if(values.Contains(LAST_NAME)) {
                model.LastName = Convert.ToString(values[LAST_NAME]);
            }

            if(values.Contains(COMPANY)) {
                model.Company = Convert.ToString(values[COMPANY]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(CITY)) {
                model.City = Convert.ToString(values[CITY]);
            }

            if(values.Contains(STATE)) {
                model.State = Convert.ToString(values[STATE]);
            }

            if(values.Contains(COUNTRY)) {
                model.Country = Convert.ToString(values[COUNTRY]);
            }

            if(values.Contains(POSTAL_CODE)) {
                model.PostalCode = Convert.ToString(values[POSTAL_CODE]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(FAX)) {
                model.Fax = Convert.ToString(values[FAX]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(SUPPORT_REP_ID)) {
                model.SupportRepId = values[SUPPORT_REP_ID] != null ? Convert.ToInt32(values[SUPPORT_REP_ID]) : (int?)null;
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