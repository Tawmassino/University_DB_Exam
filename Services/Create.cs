using Student_Platform_DB_Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_DB_Exam;
using Microsoft.EntityFrameworkCore;

namespace Student_Platform_DB_Exam.Services
{
    public class Create
    {
        private static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static Faculty CreateNewFaculty()
        {
            using var dbContext = new UniContext();

            string confirmNewFaculty = "n";
            Faculty facultyNew = null;

            while (confirmNewFaculty == "n" || confirmNewFaculty == "abort")
            {
                Console.WriteLine("Enter Faculty Name");
                string newFacultyName = Console.ReadLine();

                Faculty findFaculty = dbContext.Faculties.SingleOrDefault(f => f.FacultyName == newFacultyName);

                if (findFaculty != null)
                {
                    Console.WriteLine("This faculty already exists. Operation aborted.");
                    return null;
                }

                facultyNew = new Faculty()
                {
                    Id = Guid.NewGuid(),
                    FacultyName = newFacultyName,
                    FacultyDean = null,
                    FacultyLectures = new List<Lecture>(),
                    FacultyStudents = new List<Student>(),
                    FacultyWorkers = new List<Worker>()
                };

                Console.WriteLine("Create New Faculty? (y/n)");
                Console.WriteLine(@"Type {abort} to quit");
                confirmNewFaculty = Console.ReadLine().ToLower();

                if (confirmNewFaculty == "abort")
                {
                    Console.WriteLine("Operation aborted");
                    return null;
                }
            }

            Console.WriteLine("Faculty created successfully!");
            dbContext.Faculties.Add(facultyNew);
            dbContext.SaveChanges();
            InputKeyToContinue();
            return facultyNew;
        }

        public static Lecture CreateNewLecture()
        {
            using var dbContext = new UniContext();

            string confirmNewLecture = "n";
            Lecture lectureNew = null;

            while (confirmNewLecture == "n" || confirmNewLecture == "abort")
            {
                Console.WriteLine("Enter Lecture Name");
                string newLectureName = Console.ReadLine();

                Lecture findLecture = dbContext.Lectures.SingleOrDefault(l => l.LectureName == newLectureName);

                if (findLecture != null)
                {
                    Console.WriteLine("A lecture by the same name already exists. Operation aborted.");
                    return null;
                }

                lectureNew = new Lecture()
                {
                    Id = Guid.NewGuid(),
                    LectureName = newLectureName,
                    LectureWorker = null,
                    LectureFaculties = new List<Faculty>(),
                    LectureStudents = new List<Student>()
                };

                Console.WriteLine("Create New Lecture? (y/n)");
                Console.WriteLine(@"Type {abort} to quit");
                confirmNewLecture = Console.ReadLine().ToLower();

                if (confirmNewLecture == "abort")
                {
                    Console.WriteLine("Operation aborted");
                    return null;
                }
            }

            Console.WriteLine("Lecture created successfully!");
            dbContext.Lectures.Add(lectureNew);
            dbContext.SaveChanges();
            InputKeyToContinue();
            return lectureNew;
        }

        public static Student CreateNewStudent()
        {
            using var dbContext = new UniContext();
            string confirmNewStudent = "n";
            Student studentNew = null;

            while (confirmNewStudent == "n" || confirmNewStudent == "abort")
            {
                Console.WriteLine("Enter Student First Name");
                string newStudentFirstName = Console.ReadLine();

                Console.WriteLine("Enter Student Last Name");
                string newStudentLastName = Console.ReadLine();

                Console.WriteLine("Enter Student email");
                string newStudentEmail = Console.ReadLine();

                Console.WriteLine("Assign a Faculty to the new student");
                string newStudentFacultyName = Console.ReadLine();

                Faculty findFaculty = dbContext.Faculties.SingleOrDefault(f => f.FacultyName == newStudentFacultyName);

                if (findFaculty == null)
                {
                    Console.WriteLine($"Faculty with name '{newStudentFacultyName}' not found.");
                    continue;
                }

                Console.WriteLine("What is the level of the new student? (BA, MA, PhD)");
                string newStudentLevel = Console.ReadLine();
                // Parse StudentLevels enum property in student class
                Student.StudentLevels parsedLevelNewStudent;
                Enum.TryParse(newStudentLevel, out parsedLevelNewStudent);

                Console.WriteLine("What is the year of the new student? (1,2,3,4,5,6)");
                string newStudentYear = Console.ReadLine();
                // Parse StudentYears enum property in student class
                Student.StudentYears parsedYearNewStudent;
                Enum.TryParse(newStudentYear, out parsedYearNewStudent);

                studentNew = new Student()
                {
                    Id = Guid.NewGuid(),
                    StudentFirstName = newStudentFirstName,
                    StudentLastName = newStudentLastName,
                    StudentEmail = newStudentEmail,
                    StudentScore = null,
                    StudentFaculty = findFaculty,
                    StudentLectures = new List<Lecture>(),
                    StudentLevel = parsedLevelNewStudent,
                    StudentYear = parsedYearNewStudent
                };

                Console.WriteLine("Create New Student? (y/n)");
                Console.WriteLine(@"Type {abort} to quit");
                confirmNewStudent = Console.ReadLine().ToLower();

                if (confirmNewStudent == "abort")
                {
                    Console.WriteLine("Operation aborted");
                    return null;
                }
            }

            Console.WriteLine("Student created successfully!");
            dbContext.Students.Add(studentNew);
            dbContext.SaveChanges();
            InputKeyToContinue();
            return studentNew;
        }

        public static Worker CreateNewWorker()
        {
            using var dbContext = new UniContext();

            string confirmNewWorker = "n";
            Worker workerNew = null;

            while (confirmNewWorker == "n" || confirmNewWorker == "abort")
            {
                Console.WriteLine("Enter Worker First Name");
                string newWorkerFirstName = Console.ReadLine();

                Console.WriteLine("Enter Worker Last Name");
                string newWorkerLastName = Console.ReadLine();

                // Assuming the full name is a combination of the first name and last name
                var fullName = $"{newWorkerFirstName} {newWorkerLastName}";

                // Check if the worker with the same full name already exists
                Worker findWorker = dbContext.Workers
                .SingleOrDefault(w => (w.WorkerFirstName + " " + w.WorkerLastName) == fullName);

                if (findWorker != null)
                {
                    Console.WriteLine("This worker already exists. Operation aborted.");
                    return null;
                }

                workerNew = new Worker()
                {
                    Id = Guid.NewGuid(),
                    WorkerFirstName = newWorkerFirstName,
                    WorkerLastName = newWorkerLastName,
                    WorkerPosition = null,
                    WorkerFaculties = new List<Faculty>(),
                    WorkerLectures = new List<Lecture>()
                };

                Console.WriteLine("Create New Worker? (y/n)");
                Console.WriteLine(@"Type {abort} to quit");
                confirmNewWorker = Console.ReadLine().ToLower();

                if (confirmNewWorker == "abort")
                {
                    Console.WriteLine("Operation aborted");
                    return null;
                }
            }

            Console.WriteLine("Worker created successfully!");
            dbContext.Workers.Add(workerNew);
            dbContext.SaveChanges();
            InputKeyToContinue();
            return workerNew;
        }

    }
}