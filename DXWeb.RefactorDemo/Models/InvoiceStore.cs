using AutoMapper;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models
{
	public class InvoiceStore : EFDataStore<ChinookContext, int, DTOInvoice, Invoice>
	{
		public InvoiceStore(ChinookContext context, IMapper mapper) : base(context, mapper)
		{

		}

		public override int ModelKey(DTOInvoice model)
		{
			return model.InvoiceId;
		}

		public override void SetModelKey(DTOInvoice model, int key)
		{
			model.InvoiceId = key;
		}

		protected override int DBModelKey(Invoice model)
		{
			return model.InvoiceId;
		}
	}
}
