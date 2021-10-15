using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models
{
	//create_interface

	public class EFResult
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}

	//create_efclass

	}
