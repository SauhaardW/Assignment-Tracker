using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace AssignmentApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CourseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Course>("Course").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]

        public JsonResult Post(Course cou)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            int LastCourseId = dbClient.GetDatabase("testdb").GetCollection<Course>("Course").AsQueryable().Count();
            cou.CourseId = LastCourseId + 1;

            dbClient.GetDatabase("testdb").GetCollection<Course>("Course").InsertOne(cou);

            return new JsonResult("Created new course successfully");
        }

        [HttpPut]

        public JsonResult Put(Course cou)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var filter = Builders<Course>.Filter.Eq("CourseId", cou.CourseId);

            var update = Builders<Course>.Update.Set("CourseName", cou.CourseName);

            dbClient.GetDatabase("testdb").GetCollection<Course>("Course").UpdateOne(filter, update);

            return new JsonResult("Updated course successfully");
        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("AssignmentAppCon"));

            var filter = Builders<Course>.Filter.Eq("CourseId", id);

            

            dbClient.GetDatabase("testdb").GetCollection<Course>("Course").DeleteOne(filter);

            return new JsonResult("Deleted course successfully");
        }
    }
}

