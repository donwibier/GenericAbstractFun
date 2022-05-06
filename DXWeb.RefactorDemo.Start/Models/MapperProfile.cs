using AutoMapper;
using DXWeb.RefactorDemo.Models.DTO;
using DXWeb.RefactorDemo.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXWeb.RefactorDemo.Models
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Invoice, DTOInvoice>()
				.ForMember(d => d.InvoiceId, o => o.MapFrom(s => s.InvoiceId))
				.ForMember(d => d.CustomerId, o => o.MapFrom(s => s.CustomerId))
				.ForMember(d => d.InvoiceDate, o => o.MapFrom(s => s.InvoiceDate))
				.ForMember(d => d.BillingAddress, o => o.MapFrom(s => s.BillingAddress))
				.ForMember(d => d.BillingCity, o => o.MapFrom(s => s.BillingCity))
				.ForMember(d => d.BillingState, o => o.MapFrom(s => s.BillingState))
				.ForMember(d => d.BillingCountry, o => o.MapFrom(s => s.BillingCountry))
				.ForMember(d => d.BillingPostalCode, o => o.MapFrom(s => s.BillingPostalCode))
				.ForMember(d => d.Total, o => o.MapFrom(s => s.Total))
				.ForMember(d => d.ItemCount, opt => opt.MapFrom(src => src.InvoiceLines.Count()))
				.ReverseMap()
					.ForMember(q => q.Customer, option => option.Ignore());
			CreateMap<Customer, DTOCustomer>()
				.ReverseMap()
						.ForMember(q => q.SupportRep, option => option.Ignore());
			CreateMap<Customer, DTOCustomerLookup>()
				.ForMember(d => d.Value, o => o.MapFrom(s => s.CustomerId))
				.ForMember(d => d.Text,
						opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Company)
														? $"{src.LastName}, {src.FirstName}"
														: src.Company));

			CreateMap<Employee, DTOEmployeeLookup>()
					.ForMember(d => d.Value, o => o.MapFrom(s => s.EmployeeId))
					.ForMember(d => d.Text, o => o.MapFrom(s => $"{s.LastName}, {s.FirstName}"));

			CreateMap<Employee, DTOEmployee>()
					.ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.EmployeeId))
					.ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
					.ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
					.ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
					.ForMember(d => d.ReportsTo, o => o.MapFrom(s => s.ReportsTo))
					.ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
					.ForMember(d => d.HireDate, o => o.MapFrom(s => s.HireDate))
					.ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
					.ForMember(d => d.City, o => o.MapFrom(s => s.City))
					.ForMember(d => d.State, o => o.MapFrom(s => s.State))
					.ForMember(d => d.Country, o => o.MapFrom(s => s.Country))
					.ForMember(d => d.PostalCode, o => o.MapFrom(s => s.PostalCode))
					.ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
					.ForMember(d => d.Fax, o => o.MapFrom(s => s.Fax))
					.ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
					

			//CreateMap<InvoiceLine, DTOInvoiceLine>()
			//	.ForMember(d => d.TrackName, o => o.MapFrom(s => s.Track.Name))
			//	.ForMember(d => d.Composer, o => o.MapFrom(s => s.Track.Composer))
			//	.ForMember(d => d.MediaType, o => o.MapFrom(s => s.Track.MediaType.Name))
			//	.ForMember(d => d.MediaTypeId, o => o.MapFrom(s => s.Track.MediaTypeId))
			//	.ForMember(d => d.AlbumId, o => o.MapFrom(s => s.Track.Album.AlbumId))
			//	.ForMember(d => d.AlbumName, o => o.MapFrom(s => s.Track.Album.Title))
			//	.ForMember(d => d.ArtistName, o => o.MapFrom(s => s.Track.Album.Artist.Name))
			//	.ForMember(d => d.Genre, o => o.MapFrom(s => s.Track.Genre.Name))
			//	.ForMember(d => d.GenreId, o => o.MapFrom(s => s.Track.GenreId))
			//	.ForMember(d => d.QuantityPrice, o => o.MapFrom(s => Convert.ToDecimal(s.Quantity * s.UnitPrice)));

		}
	}
}
