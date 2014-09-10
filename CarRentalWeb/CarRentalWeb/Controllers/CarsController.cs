using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRentalWeb.Domain;
using CarRentalWeb.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;

namespace CarRentalWeb.Controllers
{
    public class CarsController : BaseController
    {
        //
        // GET: /Cars/

        public ActionResult Index()
        {			
			MongoCursor<Car> carsInDbCursor = CarRentalContext.Cars.FindAll();			
			/* LINQ example	
			IEnumerable<Car> cars = carsInDbCursor.AsQueryable().Where(c => c.NumberOfDoors > 3); */
			IMongoSortBy sortByCars = SortBy<Car>.Descending(c => c.Make);
			carsInDbCursor.SetSortOrder(sortByCars);
			AggregateExamples();
			return View(carsInDbCursor.ConvertAllToViewModels());
        }

		public ActionResult Image(string id)
		{
			Car car = CarRentalContext.Cars.FindOneById(new ObjectId(id));
			
			return View(car.ConvertToViewModel());
		}

		[HttpPost]
		public ActionResult Image(string id, HttpPostedFileBase file)
		{
			Car car = CarRentalContext.Cars.FindOneById(new ObjectId(id));
			if (!string.IsNullOrEmpty(car.ImageId))
			{
				DeleteCarImage(car);
			}
			AttachImageToCar(file, car);
			return RedirectToAction("Index");
		}

		private void IndexExamples()
		{
			//insert
			IndexKeysBuilder<Car> carIndexBuilder = IndexKeys<Car>.Ascending(c => c.Make, c => c.NumberOfDoors);
			IndexOptionsBuilder<Car> carIndexOptions = IndexOptions<Car>.SetName("Car_CompositeIndex").SetTimeToLive(new TimeSpan(2, 0, 0, 0));
			CarRentalContext.Cars.EnsureIndex(carIndexBuilder, carIndexOptions);

			//delete
			CarRentalContext.Cars.DropIndexByName("Car_CompositeIndex");
		}

		private void CappedCollectionExample()
		{
			CollectionOptionsBuilder optionsBuilder = new CollectionOptionsBuilder();
			optionsBuilder.SetCapped(true);
			optionsBuilder.SetMaxSize(52428800);
			CarRentalContext.CarRentalDatabase.CreateCollection("NewCollection", optionsBuilder);
		}

		private void AggregateExamples()
		{
			AggregateArgs simpleAggregateArgs = new AggregateArgs()
			{
				Pipeline = new[]
				{
					new BsonDocument("$match", Query<Car>.LTE(c => c.DailyRentalFee, 10).ToBsonDocument())
					, new BsonDocument("$match", Query<Car>.GTE(c => c.DailyRentalFee, 3).ToBsonDocument())
					, new BsonDocument("$sort", new BsonDocument("DailyRentalFee", 0))
				}
			};
			IEnumerable<BsonDocument> documents = CarRentalContext.Cars.Aggregate(simpleAggregateArgs);

		}

		public ActionResult Details(string id)
		{
			Car car = CarRentalContext.Cars.FindOneById(new ObjectId(id));
			/*
			 * Some example code
			IMongoQuery doorQuery = Query<Car>.GT(c => c.NumberOfDoors, 3);
			MongoCursor<Car> bigCars = CarRentalContext.Cars.Find(doorQuery);
			long howMany = bigCars.Count();
			IMongoSortBy sortByCars = SortBy<Car>.Descending(c => c.DailyRentalFee);
			bigCars.SetSortOrder(sortByCars);
			foreach (Car c in bigCars)
			{

			}
			List<Car> carsList = bigCars.ToList();
			 */
			/*
			 * Some more example code
			IMongoQuery query = Query<Car>.LTE(c => c.DailyRentalFee, 5);
			Car cheapCar = CarRentalContext.Cars.FindOne(query);
			Customer customer = CarRentalContext.Cars.FindOneByIdAs<Customer>(new ObjectId("dfsdfs"));
			FindOneArgs findOneArgs = new FindOneArgs()
			{
				Query = Query<Customer>.NE(c => c.Name, "samsung")
				, ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred)
			};
			Customer cust = CarRentalContext.Cars.FindOneAs<Customer>(findOneArgs);
			 */
			return View(car.ConvertToViewModel());
		}

		[HttpGet]
		public ActionResult Edit(string id)
		{
			Car car = CarRentalContext.Cars.FindOneById(new ObjectId(id));
			return View(car.ConvertToUpdateViewModel());
		}

		[HttpPost]
		public ActionResult Edit(UpdateCarViewModel updateCarViewModel)
		{
			if (ModelState.IsValid)
			{
				Car modifiedCar = updateCarViewModel.ConvertToDomain();				
				CarRentalContext.Cars.Save(modifiedCar);
				
				//or use the Update method:
				//CarRentalContext.Cars.Update(Query.EQ("_id", ObjectId.Parse(updateCarViewModel.Id)), Update.Replace(modifiedCar), UpdateFlags.Upsert);

				//some example code
				/*
				CarRentalContext.Cars.Update(Query.EQ("_id", ObjectId.Parse(updateCarViewModel.Id)), Update<Car>.Set(c => c.DailyRentalFee, 12));
				CarRentalContext.Cars.Update(Query.EQ("Make", "Ford"), Update<Car>.Set(c => c.DailyRentalFee, 2), UpdateFlags.Multi);
				
				FindAndModifyArgs args = new FindAndModifyArgs()
				{
					Query = Query.EQ("Make", "Ford")
					,Update = Update<Car>.Set(c => c.DailyRentalFee, 2),
					Upsert = false
					,SortBy = SortBy<Car>.Ascending(c => c.Id)
					,VersionReturned = FindAndModifyDocumentVersion.Modified
				};
				FindAndModifyResult res = CarRentalContext.Cars.FindAndModify(args);
				*/
				
				return RedirectToAction("Index");
			}
			return View(updateCarViewModel);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(InsertCarViewModel insertCarViewModel)
		{
			if (ModelState.IsValid)
			{
				Car car = insertCarViewModel.ConvertToDomain();				
				WriteConcernResult writeResult = CarRentalContext.Cars.Insert(car);
				bool ok = writeResult.Ok;
				return RedirectToAction("Index");
			}
			return View(insertCarViewModel);
		}

		public ActionResult Delete(string id)
		{
			CarRentalContext.Cars.Remove(Query.EQ("_id", ObjectId.Parse(id)));
			return RedirectToAction("Index");
		}

		private void DeleteCarImage(Car car)
		{
			CarRentalContext.CarRentalDatabase.GridFS.DeleteById(car.ImageId);
			car.ImageId = string.Empty;
			CarRentalContext.Cars.Save(car);
		}

		private void AttachImageToCar(HttpPostedFileBase file, Car car)
		{
			ObjectId imageId = ObjectId.GenerateNewId();
			car.ImageId = imageId.ToString();
			CarRentalContext.Cars.Save(car);
			MongoGridFSCreateOptions createOptions = new MongoGridFSCreateOptions()
			{
				Id = imageId
				, ContentType = file.ContentType
			};
			CarRentalContext.CarRentalDatabase.GridFS.Upload(file.InputStream, file.FileName, createOptions);
		}
    }
}
