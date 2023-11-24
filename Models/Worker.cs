using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Platform_DB_Exam.Models
{
    public class Worker
    {
        public Guid Id { get; set; }
        public string? WorkerFirstName { get; set; }
        public string? WorkerLastName { get; set; }
        public string? WorkerPosition { get; set; }
        public List<Faculty>? WorkerFaculties { get; set; }
        public List<Lecture>? WorkerLectures { get; set; }

    }
}
