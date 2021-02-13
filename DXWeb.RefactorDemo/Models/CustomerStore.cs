using AutoMapper;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models
{
	public class CustomerStore : EFDataStore<ChinookContext, int, DTOCustomer, Customer>
	{
		public CustomerStore(ChinookContext context, IMapper mapper) : base(context, mapper)
		{

		}
		protected override int DBModelKey(Customer model) => model.CustomerId;

		public override int ModelKey(DTOCustomer model) => model.CustomerId;

		public override void SetModelKey(DTOCustomer model, int key) => model.CustomerId = key;
	}
}
