using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CarRentalWeb.Domain;
using CarRentalWeb.ViewModels;

namespace CarRentalWeb
{
	public static class DomainExtensions
	{
		public static Car ConvertToDomain(this InsertCarViewModel insertCarViewModel)
		{
			Car car = new Car()
			{
				DailyRentalFee = insertCarViewModel.DailyRentalFee
				,
				Make = insertCarViewModel.Make
				,
				NumberOfDoors = insertCarViewModel.NumberOfDoors
			};
			string[] countries = insertCarViewModel.DelimitedListOfCountries.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			car.CountriesAllowedIn = countries.ToList();
			return car;
		}

		public static Car ConvertToDomain(this UpdateCarViewModel updateCarViewModel)
		{
			Car car = new Car()
			{
				Id = updateCarViewModel.Id
				, DailyRentalFee = updateCarViewModel.DailyRentalFee
				, Make = updateCarViewModel.Make
				, NumberOfDoors = updateCarViewModel.NumberOfDoors
			};
			string[] countries = updateCarViewModel.DelimitedListOfCountries.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			car.CountriesAllowedIn = countries.ToList();
			return car;
		}

		public static IEnumerable<CarViewModel> ConvertAllToViewModels(this IEnumerable<Car> carDomains)
		{
			foreach (Car car in carDomains)
			{
				yield return car.ConvertToViewModel();
			}
		}

		public static CarViewModel ConvertToViewModel(this Car carDomain)
		{
			CarViewModel carViewModel = new CarViewModel()
			{
				Id = carDomain.Id
				, DailyRentalFee = carDomain.DailyRentalFee
				, Make = carDomain.Make
				, NumberOfDoors = carDomain.NumberOfDoors
				, ImageId = carDomain.ImageId
			};

			if (carDomain.CountriesAllowedIn != null && carDomain.CountriesAllowedIn.Count() > 0)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < carDomain.CountriesAllowedIn.Count(); i++)
				{
					sb.Append(carDomain.CountriesAllowedIn.ElementAt(i));
					if (i < carDomain.CountriesAllowedIn.Count() - 1)
					{
						sb.Append(",");
					}
				}
				carViewModel.CountriesAllowedIn = sb.ToString();
			}

			return carViewModel;
		}

		public static UpdateCarViewModel ConvertToUpdateViewModel(this Car carDomain)
		{
			UpdateCarViewModel updateVm = new UpdateCarViewModel()
			{
				Id = carDomain.Id
				, DailyRentalFee = carDomain.DailyRentalFee
				, Make = carDomain.Make
				, NumberOfDoors = carDomain.NumberOfDoors
			};

			if (carDomain.CountriesAllowedIn != null && carDomain.CountriesAllowedIn.Count() > 0)
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < carDomain.CountriesAllowedIn.Count(); i++)
				{
					sb.Append(carDomain.CountriesAllowedIn.ElementAt(i));
					if (i < carDomain.CountriesAllowedIn.Count() - 1)
					{
						sb.Append(";");
					}
				}
				updateVm.DelimitedListOfCountries = sb.ToString();
			}

			return updateVm;
		}
	}
}