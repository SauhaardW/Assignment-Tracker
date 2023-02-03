using System;
using MongoDB.Bson;

namespace AssignmentApplication.Models
{
    public class Assignment
    {
        public ObjectId Id { get; set; }

        public int AssignmentId { get; set; }

        public string AssignmentName { get; set; }

        public string Course { get; set; }

        public string PhotoFile { get; set; }

        public string DueDate { get; set; }
    }
}

