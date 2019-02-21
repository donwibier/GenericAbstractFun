using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console.Coding.Demo.Abstracts;
using Console.Coding.Demo.Generics;

namespace Console.Coding.Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Abstract Demo
			System.Console.WriteLine("Abstract samples =========================");
			Shape[] shapes = {
					new Circle("Circle #1", 3),
					new Rectangle( "Rectangle #1",4, 5)
			};

			System.Console.WriteLine("Shapes Collection");
			foreach (Shape s in shapes)
			{
				System.Console.WriteLine(s);
			}

			#endregion

			#region Generics Demo
			System.Console.WriteLine("Generic samples =========================");
			PersonStore persons = new PersonStore(new DataItem[] {
				new DataItem{ID = 1,Name="Don", Company="DevExpress", EmailAddress = "donw@devexpress.com"},
				new DataItem{ID = 2,Name="Oliver", Company="DevExpress", EmailAddress = "olivers@devexpress.com"},
				new DataItem{ID = 3,Name="John", Company="DevExpress", EmailAddress = "johnm@devexpress.com"}
			});
			System.Console.WriteLine($"Persons generic type: {persons.GenericTypeName}");
			System.Console.WriteLine($"Person with ID: 2 = {persons.GetByID(2).Name}");

			Guid g = Guid.NewGuid();
			ProductStore products = new ProductStore(new ProductItem[] {
				new ProductItem{ProductID = Guid.NewGuid(),Name="Big Mac", Price = 2},
				new ProductItem{ProductID = Guid.NewGuid(),Name="Whopper", Price = 3},
				new ProductItem{ProductID = g, Name="Hot Wing", Price = 1}
			});
			System.Console.WriteLine($"Products generic type: {products.GenericTypeName}");
			System.Console.WriteLine($"Product with ID: {g} = {products.GetByID(g).Name}");

			//DataStore<string, string> store1 = new DataStore<string, string>(new string[] {
			//	"Item A",
			//	"Item B",
			//	"Item C"
			//});

			//var item = persons.Query((x) => x.Name == "Don").ToList();
			//var p = products.Query((x) => x.Price == 3).ToList();
			//var item1 = store1.Query((x) => x.ToUpperInvariant().StartsWith("ITEM")).ToList();



			#endregion

			System.Console.ReadKey();
		}
	}
}
