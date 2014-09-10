using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalWeb.ViewModels
{
	public class UpdateCarViewModel
	{
		[Editable(false)]
		public string Id { get; set; }
		[Required]
		[Display(Name = "Type of car")]
		public string Make { get; set; }
		[Required]
		[Display(Name = "Number of doors")]
		[Range(2, 6)]
		public int NumberOfDoors { get; set; }
		[Required]
		[Display(Name = "Daily rental fee")]
		public decimal DailyRentalFee { get; set; }
		[Required]
		[Display(Name = "List of allowed countries delimited with ';'")]
		public string DelimitedListOfCountries { get; set; }
	}
}