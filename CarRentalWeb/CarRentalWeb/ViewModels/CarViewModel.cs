using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalWeb.ViewModels
{
	public class CarViewModel
	{
		[Display(Name = "ID")]
		public string Id { get; set; }
		public string Make { get; set; }
		[Display(Name = "Rental fee per day")]
		public decimal DailyRentalFee { get; set; }
		[Display(Name = "Number of doors")]
		public int NumberOfDoors { get; set; }
		[Display(Name = "Allowed countries")]
		public string CountriesAllowedIn { get; set; }
		[Display(Name = "Image ID")]
		public string ImageId { get; set; }

		public bool HasImage
		{
			get
			{
				return !string.IsNullOrEmpty(ImageId);
			}
		}
	}
}