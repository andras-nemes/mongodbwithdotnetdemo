using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace CarRentalWeb.Controllers
{
	public class ImagesController : BaseController
    {
        //
        // GET: /Images/

        public ActionResult Image(string imageId)
        {
			MongoGridFSFileInfo imageFileInfo = CarRentalContext.CarRentalDatabase.GridFS.FindOneById(new ObjectId(imageId));
			return File(imageFileInfo.OpenRead(), imageFileInfo.ContentType);
        }

    }
}
