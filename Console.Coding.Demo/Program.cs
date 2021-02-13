using Console.Coding.Demo.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			System.Console.ReadKey();
		}
	}
}
