using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Platform_DB_Exam.Models
{
    public class Faculty
    {
        public Guid Id { get; set; }
        public string? FacultyName { get; set; }

        [NotMapped] public Worker? FacultyDean { get; set; }

        public List<Lecture>? FacultyLectures { get; set; } = new List<Lecture>();

        public List<Student>? FacultyStudents { get; set; } = new List<Student>();

        public List<Worker>? FacultyWorkers { get; set; } = new List<Worker>();

    }
}
