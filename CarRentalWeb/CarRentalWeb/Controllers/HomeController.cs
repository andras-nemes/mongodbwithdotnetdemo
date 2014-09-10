using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRentalWeb.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CarRentalWeb.Controllers
{
	public class HomeController : Controller
	{
		private CarRentalContext _carRentalContext;

		public HomeController()
		{
		
			_carRentalContext = new CarRentalContext();
		}

		public ActionResult Index()
		{
			_carRentalContext.CarRentalDatabase.GetCollectionNames();
			return Json(_carRentalContext.CarRentalDatabase.Server.BuildInfo, JsonRequestBehavior.AllowGet);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}

	[BsonIgnoreExtraElements]
	public class Customer
	{
		[BsonId]
		public int CustomerId { get; set; }

		[BsonRepresentation(MongoDB.Bson.BsonType.Double)]
		public decimal TotalOrders { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]		
		public DateTime CustomerSince { get; set; }

		public string Name { get; set; }
		[BsonIgnoreIfNull]
		public string Address { get; set; }
		IEnumerable<string> Telephones { get; set; }
		public WebPage PublicPage { get; set; }
	}

	public class WebPage
	{
		public bool IsSsl { get; set; }
		public string Domain { get; set; }
	}
}
