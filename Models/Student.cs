using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Platform_DB_Exam.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }

        public string? StudentEmail { get; set; }

        public double? StudentScore { get; set; }

        public Faculty? StudentFaculty { get; set; }

        public List<Lecture>? StudentLectures { get; set; } = new List<Lecture>();


        public StudentLevels? StudentLevel { get; set; }
        public enum StudentLevels { BA, MA, PhD, Alumni, Expelled } // Bachelor, Master, Doctorate. Completed, Stopped.

        public StudentYears? StudentYear { get; set; }
        public enum StudentYears
        {
            FirstYear = 1,
            SecondYear = 2,
            ThirdYear = 3,
            FourthYear = 4,
            FifthYear = 5,
            SixthYear = 6
        }

    }
}
