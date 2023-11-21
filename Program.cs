using Azure;
using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam;
using System;
using static University_DB_Exam.Student;

namespace University_DB_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // ============ TO DO ============
            //====== METHODS TO DO ======
            //1.Sukurti departamentą ir į jį pridėti studentus, paskaitas(papildomi points jei pridedamos paskaitos jau egzistuojančios duomenų bazėje).
            //2.Pridėti studentus / paskaitas į jau egzistuojantį departamentą.
            //3.Sukurti paskaitą ir ją priskirti prie departamento.
            //4.Sukurti studentą, jį pridėti prie egzistuojančio departamento ir priskirti jam egzistuojančias paskaitas.
            //5.Perkelti studentą į kitą departamentą(bonus points jei pakeičiamos ir jo paskaitos).
            //6.Atvaizduoti visus departamento studentus.
            //7.Atvaizduoti visas departamento paskaitas.
            //8.Atvaizduoti visas paskaitas pagal studentą.

            //MANO
            //paskaitos destytojas
            //lankomumas
            //pazymiai
            //namu darbai


            //----------- program start ----------
            using var dbContext = new UniContext();

            //while (true)
            //{
            //    ShowMenu(myRestaurant, menuManager);
            //}


        }

        // // --------------------------------- END OF MAIN  ---------------------------------


        // ====================================== METHODS  ======================================

        // A METHOD THAT........

        // shows menu
        // returns to main menu

        //----------
        #region ShowMethods

        //show all faculties
        public void ShowAllFaculties()
        {
            var dbContext = new UniContext();
            List<string> allFacultyNames = dbContext.Faculties.Select(n => n.FacultyName).ToList();

            Console.WriteLine(allFacultyNames);
        }

        //show all lectures
        public void ShowAllLectures()
        {
            var dbContext = new UniContext();
            List<string> allLectureNames = dbContext.Lectures.Select(l => l.LectureName).ToList();

            Console.WriteLine(allLectureNames);
        }

        //show all students
        public void ShowAllStudents()
        {
            var dbContext = new UniContext();
            List<string> allStudentNames = dbContext.Students.Select(s => $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}").ToList();

            Console.WriteLine(allStudentNames);
        }


        //show all students from a faculty
        public void ShowAllStudentsFromFaculty()
        {
            var dbContext = new UniContext();

            ShowAllFaculties();
            Console.WriteLine("Select a faculty to view students");
            string inputFaculty = Console.ReadLine();
            Faculty chosenFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == inputFaculty);

            //reikia susieti kuri fakulteta gavome su ka darome apacioje
            List<string> allStudentNames = dbContext.Students.Select(s => $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}").ToList();

            Console.WriteLine(allStudentNames);
        }

        public void ShowOneStudentData()
        {
            var dbContext = new UniContext();
            List<string> oneStudentData = dbContext.Students
                .Select(s =>
                $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}  || FACULTY: {s.StudentFaculty} || YEAR: {s.StudentYear} || EMAIL: {s.StudentEmail}") //sitoj vietoje dar reikia parodyti kokias lectures eina
                .ToList();
        }
        //show all lectures of a faculty
        //show all lectures of individual student
        #endregion

        //----------

        #region CreateNew


        // creates new faculty
        public Faculty CreateNewFaculty(UniContext dbContext)
        {

            string confirmNewFaculty = "n";
            Faculty facultyNew = null;

            while (confirmNewFaculty == "n" || confirmNewFaculty == "abort")
            {
                Console.WriteLine("Enter Faculty Name");
                string newFacultyName = Console.ReadLine();

                //check if the faculty name already exists
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
            return facultyNew;
        }

        // creates new lecture
        public Lecture CreateNewLecture(UniContext dbContext)
        {

            string confirmNewLecture = "n";
            Lecture lectureNew = null;

            while (confirmNewLecture == "n" || confirmNewLecture == "abort")
            {
                Console.WriteLine("Enter Lecture Name");
                string newLectureName = Console.ReadLine();

                //check if the Lecture name doesn't already exist

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
            return lectureNew;
        }

        // creates new student

        public Student CreateNewStudent(UniContext dbContext)
        {

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

                // find Faculty by name
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

                Console.WriteLine("Create New Lecture? (y/n)");
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
            return studentNew;
        }
        #endregion

        //----------

        #region Add

        //adds student(s) to faculty
        //adds lectures(s) to faculty
        //adds lectures(s) to student
        #endregion

        #region Change


        //move student to another faculty (update)
        #endregion


        // ----------------- DATABASE METHODS -----------------
        //method that shows menu



        // OBSOLETE


        // ================================== END OF METHODS  ===================================
    }
}