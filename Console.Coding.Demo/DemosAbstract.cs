using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Coding.Demo.Abstracts
{
	public abstract class Shape
	{
		public abstract double Area { get; }
		public string ShapeName { get; set; }
		public Shape(string shapeName)
		{
			ShapeName = shapeName;
		}
		public override string ToString()
		{
			return $"{base.ToString()} ({Area})";
		}

	}

	public class Rectangle : Shape
	{
		public double Width { get; set; }
		public double Height { get; set; }

		public override double Area => Width*Height;

		public Rectangle(string shapeName, double width, double height) : base(shapeName)
		{
			Width = width;
			Height = height;
		}
	}

	public class Circle : Shape
	{

		public Circle(string shapeName, double radius) : base(shapeName)
		{
			Radius = radius;
		}

		public double Radius { get; set; }

		public override double Area => Radius * Radius * System.Math.PI;


	}
}
