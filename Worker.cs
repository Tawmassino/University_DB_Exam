using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_DB_Exam;

namespace Student_Platform_DB_Exam
{
    public class Worker
    {
        public Guid Id { get; set; }
        public string? WorkerFirstName { get; set; }
        public string? WorkerLastName { get; set; }
        public string? WorkerPosition { get; set; }
        public Faculty? WorkerFaculty { get; set; }
        public Lecture? WorkerLecture { get; set; }

    }
}
