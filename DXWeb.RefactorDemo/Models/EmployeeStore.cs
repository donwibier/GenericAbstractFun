using AutoMapper;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;

namespace DXWeb.RefactorDemo.Models
{
	public class EmployeeStore : EFDataStore<ChinookContext, int, DTOEmployee, Employee>
	{
		public EmployeeStore(ChinookContext context, IMapper mapper) : base(context, mapper)
		{

		}

        public override int ModelKey(DTOEmployee model) => model.EmployeeId;

        public override void SetModelKey(DTOEmployee model, int key) => model.EmployeeId = key;

        protected override int DBModelKey(Employee model) => model.EmployeeId;
    }
}
