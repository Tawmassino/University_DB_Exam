using Student_Platform_DB_Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_DB_Exam
{
    public class Faculty
    {
        public Guid Id { get; set; }
        public string? FacultyName { get; set; }

        [NotMapped] public Worker? FacultyDean { get; set; }

        public List<Lecture>? FacultyLectures { get; set; }

        public List<Student>? FacultyStudents { get; set; }

        public List<Worker>? FacultyWorkers { get; set; }

    }
}
