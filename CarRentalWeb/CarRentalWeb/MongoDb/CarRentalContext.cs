using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CarRentalWeb.Domain;
using MongoDB.Driver;

namespace CarRentalWeb.MongoDb
{
	public class CarRentalContext
	{
		private MongoClient _mongoClient;
		private MongoServer _mongoServer;
		private MongoDatabase _mongoDatabase;

		public CarRentalContext()
		{
			String mongoHost = ConfigurationManager.ConnectionStrings["CarRentalConnectionString"].ConnectionString;
			MongoClientSettings settings =
				MongoClientSettings.FromUrl(new MongoUrl(mongoHost));			
			_mongoClient = new MongoClient(settings);			
			_mongoServer = _mongoClient.GetServer();			
			_mongoDatabase = _mongoServer.GetDatabase(ConfigurationManager.AppSettings["CarRentalDatabaseName"]);
		}

		public MongoDatabase CarRentalDatabase
		{
			get
			{
				return _mongoDatabase;
			}
		}

		public MongoCollection<Car> Cars
		{
			get
			{
				return CarRentalDatabase.GetCollection<Car>("cars");
			}
		}
	}
}