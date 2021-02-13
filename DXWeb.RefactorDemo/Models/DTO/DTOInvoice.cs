using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models.DTO
{
	public class DTOInvoice
	{
		[Key]
		[Display(Name = "#", AutoGenerateField = true)]
		[Editable(false)]
		public int InvoiceId { get; set; }
		[Required]
		public int CustomerId { get; set; }
		public DateTime InvoiceDate { get; set; }
		public string BillingAddress { get; set; }
		public string BillingCity { get; set; }
		public string BillingState { get; set; }
		public string BillingCountry { get; set; }
		public string BillingPostalCode { get; set; }
		public decimal Total { get; set; }
		[Editable(false)]
		public int? ItemCount { get; set; }

	}
}
