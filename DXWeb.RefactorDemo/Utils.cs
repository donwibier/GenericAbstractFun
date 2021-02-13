using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo
{
	/// <summary>
	/// Extension methods for use with DevExpress Blazor DataGrid
	/// (Probably for DevExtreme MVC / ASP.NET Core Datagrid as well)
	/// </summary>
	public static class PropertyExtensions
	{
		static readonly ConcurrentDictionary<Type, PropertyInfo[]> cache =
			new ConcurrentDictionary<Type, PropertyInfo[]>();

		static Action CleanCache => () =>
		{
			if (cache.Count > 1000)
			{
				while (cache.Count > 1000)
					if (!cache.TryRemove(cache.Keys.Last(), out PropertyInfo[] r))
						return; // break if can't be removed
			}
		};

		static readonly BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
		static PropertyInfo[] GetInfo(Type objectType)
		{
			PropertyInfo[] props = cache.GetOrAdd(objectType, (key) => objectType.GetProperties(flags));
			Task.Run(CleanCache);
			return props;
		}
		static PropertyInfo GetInfo(Type objectType, string propertyName)
		{
			PropertyInfo[] props = cache.GetOrAdd(objectType, (key) => objectType.GetProperties(flags));
			var result = GetInfo(objectType).Where(p => p.Name == propertyName).FirstOrDefault();
			return result;
		}

		public static bool HasProperty(this object obj, string propertyName)
		{
			return GetInfo(obj.GetType(), propertyName) != null;
		}

		public static T GetPropertyValue<T>(this object obj, string propertyName)
		{
			var propType = GetInfo(obj.GetType(), propertyName);
			var result = (T)propType?.GetValue(obj);
			return result;
		}

		public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
		{
			SetPropertyValue(obj, propertyName, (object)value);
		}
		// special types which need checking
		static readonly Dictionary<Type, Func<PropertyInfo, object, object>> converter = new Dictionary<Type, Func<PropertyInfo, object, object>>
		{
			{ typeof(decimal), (pi, v) => Convert.ToDecimal(v) },
			{ typeof(double), (pi, v) => Convert.ToDouble(v) },
			{ typeof(DateTime), (pi, v) => Convert.ToDateTime(v) },
			{ typeof(string), (pi, v) => Convert.ToString(v) },
			{ typeof(long), (pi, v) => v?.GetType() == typeof(int) ? Convert.ToInt32(v) : v },
			{ typeof(Int32), (pi, v) => v?.GetType() == typeof(Int64) ? Convert.ToInt32(v.ToString()) : v },

		};

		public static void SetPropertyValue(this object obj, string propertyName, object value)
		{
			var propType = GetInfo(obj.GetType(), propertyName);
			if (converter.ContainsKey(propType.PropertyType))
				propType.SetValue(obj, converter[propType.PropertyType](propType, value));
			else
				propType.SetValue(obj, value);
		}

		public static T AssignToObject<T>(this IDictionary<string, object> dictionary, T dataItem)
		{
			foreach (var field in dictionary.Keys)
			{
				if (dataItem.HasProperty(field))
					dataItem.SetPropertyValue(field, dictionary[field]);
			}
			return dataItem;
		}
		public static T AssignToObject<T>(this IDictionary dictionary, T dataItem)
		{
			foreach (var field in dictionary.Keys)
			{
				if (dataItem.HasProperty((string)field))
					dataItem.SetPropertyValue((string)field, dictionary[field]);
			}
			return dataItem;
		}

		public static TDestinationType Assign<TSourceType, TDestinationType>(this TSourceType source,
			TDestinationType destination,
			params string[] excludeProperties)
			where TSourceType : class
			where TDestinationType : class
		{
			var propsInfo = GetInfo(source.GetType());
			bool skipExcl = excludeProperties == null;
			foreach (var p in propsInfo)
			{
				if (!skipExcl && !excludeProperties.Contains(p.Name) && destination.HasProperty(p.Name))
					destination.SetPropertyValue(p.Name, p.GetValue(source));
			}
			return destination;
		}
	}

	public static class ListExtensions
	{
		public static int RemoveAll<T>(this List<T> list, Predicate<T> predicate, Action<T> action)
		{
			return list.RemoveAll(item =>
			{
				if (predicate(item))
				{
					action(item);
					return true;
				}
				return false;
			});
		}
	}
}
