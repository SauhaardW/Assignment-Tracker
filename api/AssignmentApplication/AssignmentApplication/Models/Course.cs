using MongoDB.Bson;
using System;

namespace AssignmentApplication.Models
{
    public class Course
    {
        public ObjectId Id { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }
    }
}

