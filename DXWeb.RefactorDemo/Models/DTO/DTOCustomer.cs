using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models.DTO
{
	public class DTOCustomerLookup
	{
		public int Value { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Company { get; set; }
		public string Text { get; set; }
	}
	public class DTOCustomer
	{
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string Company { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string City { get; set; }
		public string State { get; set; }
		[Required]
		public string Country { get; set; }
		[Required]
		public string PostalCode { get; set; }
		[Phone]
		public string Phone { get; set; }
		[Phone]
		public string Fax { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public int? SupportRepId { get; set; }
	}
}
