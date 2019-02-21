using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Coding.Demo.Generics
{
	public class DataItem:IKeyValue<int>
	{
		public DataItem()
		{

		}
		public int ID { get; set; }
		public string Name { get; set; }
		public string Company { get; set; }
		public string EmailAddress { get; set; }

		public int KeyValue => ID;
	}

	public class ProductItem:IKeyValue<Guid>
	{
		public ProductItem()
		{
		}

		public Guid ProductID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		public Guid KeyValue => ProductID;
	}

	public interface IKeyValue<TKeyType>
		where TKeyType : IEquatable<TKeyType>
	{
		TKeyType KeyValue { get; }
	}

	public abstract class DataStore<TKeyType, TDataType>
		where TKeyType: IEquatable<TKeyType>
		where TDataType: IKeyValue<TKeyType>, new()

	{
		private readonly List<TDataType> items = new List<TDataType>();

		public DataStore(IEnumerable<TDataType> data)
		{
			items.AddRange(data);
		}
		public string GenericTypeName {  get { return typeof(TDataType).Name; } }

		public IEnumerable<TDataType> Query(Predicate<TDataType> criteria)
		{
			var r = Execute((s) => {
				return items.FindAll(criteria);
			});
				
			return r;
		}
		public TDataType GetByID(TKeyType id)
		{
			var result =Execute((s) => {
				var r = from n in items
						where n.KeyValue.Equals(id)
						select n;

				return r.FirstOrDefault();
			});
			return result;
		}

		public bool Add(TDataType item)
		{
			items.Add(item);
			return true;
		}
	
		public bool Update(TDataType item)
		{
			TDataType oldItem = GetByID(item.KeyValue);
			//copy items props into the oldItem
			Assign(item, oldItem);


			return true;
		}

		static readonly object padLock = new object();
		public T Execute<T>(Func<DataStore<TKeyType, TDataType>, T> work)
		{
			T result = default(T);
			lock (padLock)
			{
				result = work(this);
			}
			return result;
		}

		public abstract void Assign(TDataType source, TDataType target);
	}

	public class PersonStore : DataStore<int, DataItem>
	{
		public PersonStore(IEnumerable<DataItem> data) : base(data)
		{
		}

		public override void Assign(DataItem source, DataItem target)
		{
			target.Company = source.Company;
			target.EmailAddress = source.EmailAddress;
			target.Name = source.Name;

		}
	}

	public class ProductStore:DataStore<Guid, ProductItem>
	{
		public ProductStore(IEnumerable<ProductItem> data) : base(data)
		{
		}

		public override void Assign(ProductItem source, ProductItem target)
		{
			target.Name = source.Name;
			target.Price = source.Price;
		}
	}




	public class C {
		internal void DoSomething() { }
	}

	public class C<T>:C
		where T: C<T>
	{
		public virtual void DoSomething(T item)
		{
			item.DoSomething();
		}
	}

	public class B : C<B>
	{
		public override void DoSomething(B item)
		{
			base.DoSomething(item);
		}
	}
}
