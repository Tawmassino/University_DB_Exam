using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Platform_DB_Exam.Models
{
    public class Lecture
    {
        public Guid Id { get; set; }
        public string? LectureName { get; set; }

        public Worker? LectureWorker { get; set; }
        public List<Faculty>? LectureFaculties { get; set; } = new List<Faculty>();
        public List<Student>? LectureStudents { get; set; } = new List<Student>();


    }
}
