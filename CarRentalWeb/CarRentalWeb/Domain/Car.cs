using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarRentalWeb.Domain
{
	public class Car
	{
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		public string Id { get; set; }
		public string Make { get; set; }
		[BsonRepresentation(MongoDB.Bson.BsonType.Double)]
		public decimal DailyRentalFee { get; set; }
		public int NumberOfDoors { get; set; }
		public List<string> CountriesAllowedIn { get; set; }
		public string ImageId { get; set; }
	}
}