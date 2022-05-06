using DXWeb.RefactorDemo.Models;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connStr = Configuration.GetConnectionString("ChinookConnection");
			services.AddDbContext<ChinookContext>(options => options.UseSqlServer(connStr), ServiceLifetime.Transient);
			services.AddAutoMapper(typeof(Startup).Assembly);
			// Add framework services.
			services
				.AddRazorPages()
				.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

			services.AddScoped<IDataStore<int, DTOInvoice>, InvoiceStore>();
			services.AddScoped<IDataStore<int, DTOCustomer>, CustomerStore>();
			services.AddScoped<IDataStore<int, DTOEmployee>, EmployeeStore>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();
			});
		}
	}
}
