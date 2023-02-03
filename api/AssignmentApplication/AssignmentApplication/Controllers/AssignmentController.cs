using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AssignmentApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AssignmentController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Assignment>("Assignment").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]

        public JsonResult Post(Assignment ass)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            int LastAssignmentId = dbClient.GetDatabase("testdb").GetCollection<Course>("Assignment").AsQueryable().Count();
            ass.AssignmentId = LastAssignmentId + 1;

            dbClient.GetDatabase("testdb").GetCollection<Assignment>("Assignment").InsertOne(ass);

            return new JsonResult("Created new assignment successfully");
        }

        [HttpPut]

        public JsonResult Put(Assignment ass)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var filter = Builders<Assignment>.Filter.Eq("AssignmentId", ass.AssignmentId);

            var update = Builders<Assignment>.Update.Set("AssignmentName", ass.AssignmentName)
                                                    .Set("Course", ass.Course)
                                                    .Set("DueDate", ass.DueDate)
                                                    .Set("PhotoFile", ass.PhotoFile);

            dbClient.GetDatabase("testdb").GetCollection<Assignment>("Assignment").UpdateOne(filter, update);

            return new JsonResult("Updated assignment successfully");
        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var filter = Builders<Assignment>.Filter.Eq("AssignmentId", id);



            dbClient.GetDatabase("testdb").GetCollection<Assignment>("Assignment").DeleteOne(filter);

            return new JsonResult("Deleted assignment successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("temp.png");
            }
        }
    }
}