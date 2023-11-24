using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student_Platform_DB_Exam.Models;

namespace Student_Platform_DB_Exam
{
    public class Worker
    {
        public Guid Id { get; set; }
        [NotMapped] public string? WorkerFirstName { get; set; }
        [NotMapped] public string? WorkerLastName { get; set; }
        [NotMapped] public string? WorkerPosition { get; set; }
        [NotMapped] public List<Faculty>? WorkerFaculties { get; set; }
        [NotMapped] public List<Lecture>? WorkerLectures { get; set; }

    }
}
