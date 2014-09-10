using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRentalWeb.MongoDb;

namespace CarRentalWeb.Controllers
{
	public class BaseController : Controller
	{
		private CarRentalContext _carRentalContext;

		public BaseController()
		{
			_carRentalContext = new CarRentalContext();
		}

		public CarRentalContext CarRentalContext
		{
			get
			{
				return _carRentalContext;
			}
		}
	}
}