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
    public static class Show
    {
        public static void AllFaculties()
        {
            try
            {
                using var dbContext = new UniContext();
                List<Faculty> allFacultyNames = dbContext.Faculties.ToList();
                Console.WriteLine();
                Console.WriteLine("=====================================");
                Console.WriteLine("All faculties:");

                foreach (Faculty faculty in allFacultyNames)
                {
                    Console.WriteLine(faculty.FacultyName);
                }

                Console.WriteLine("=====================================");
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving and displaying faculties: {ex.Message}");
                InputKeyToContinue();
            }
        }

        public static void AllLectures()
        {
            try
            {
                using var dbContext = new UniContext();
                var allLectureNames = dbContext.Lectures
                    .Include(l => l.LectureFaculties)
                    .Include(l => l.LectureStudents)
                    .ToList();

                Console.WriteLine();
                Console.WriteLine("=====================================");
                Console.WriteLine("All lectures:");

                foreach (Lecture lecture in allLectureNames)
                {
                    if (lecture.LectureFaculties != null)
                    {
                        string facultyNames = string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName));

                        Console.WriteLine($"{lecture.Id} : {lecture.LectureName} - {lecture.LectureWorker} @ {facultyNames}");
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("=====================================");
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }

        public static void AllStudents()
        {
            try
            {
                using var dbContext = new UniContext();
                var allStudents = dbContext.Students.Include(s => s.StudentFaculty).ToList();

                Console.WriteLine();

                if (allStudents.Any())
                {
                    Console.WriteLine("=====================================");
                    Console.WriteLine(" =========== All Students ===========");

                    foreach (var student in allStudents)
                    {
                        string facultyName = student.StudentFaculty != null ? student.StudentFaculty.FacultyName : "N/A";

                        Console.WriteLine($"{student.Id}: {student.StudentFirstName} {student.StudentLastName} || {student.StudentLevel} {student.StudentYear} || {facultyName} || {student.StudentEmail}");
                    }
                    Console.WriteLine("=====================================");
                }
                else
                {
                    Console.WriteLine("No students found.");
                    InputKeyToContinue();
                }

                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }


        public static void AllStudentsFromFaculty()
        {
            try
            {
                using var dbContext = new UniContext();

                AllFaculties();
                Console.WriteLine("Select a faculty to view students");
                string inputFaculty = Console.ReadLine();
                Faculty chosenFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == inputFaculty);

                if (chosenFaculty == null)
                {
                    Console.WriteLine("Faculty not found. Exiting.");
                    return;
                }

                List<Student> allStudentsByFaculty = dbContext.Students.Where(s => s.StudentFaculty.FacultyName == chosenFaculty.FacultyName).ToList();

                Console.WriteLine($" ====== Students in {chosenFaculty.FacultyName} ======");

                foreach (var student in allStudentsByFaculty)
                {
                    Console.WriteLine($"Student ID: {student.Id}");
                    Console.WriteLine($"Name: {student.StudentFirstName} {student.StudentLastName}");
                    Console.WriteLine($"Email: {student.StudentEmail}");
                    Console.WriteLine($"Level: {student.StudentLevel} || Year: {student.StudentYear}");
                    Console.WriteLine();
                }

                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }


        public static void AllLecturesOfFaculty()
        {
            try
            {
                using var dbContext = new UniContext();
                Console.WriteLine("View all faculties(y/n)?");
                string viewChoice = Console.ReadLine();
                if (viewChoice == "y")
                {
                    AllFaculties();
                }

                Console.WriteLine();
                Console.WriteLine("Input the name of the faculty to view its lectures");
                string inputFaculty = Console.ReadLine();
                Faculty chosenFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == inputFaculty);

                if (chosenFaculty == null)
                {
                    Console.WriteLine("Faculty not found. Exiting.");
                    InputKeyToContinue();
                    return;
                }

                List<Lecture> allLecturesByFaculty = dbContext.Lectures
                    .Include(l => l.LectureFaculties)
                    .Include(l => l.LectureStudents)
                    .Where(l => l.LectureFaculties.Any(n => n.FacultyName == chosenFaculty.FacultyName))
                    .ToList();

                Console.WriteLine();
                Console.WriteLine($" ====== Lectures in {chosenFaculty.FacultyName} ======");

                foreach (var lecture in allLecturesByFaculty)
                {
                    Console.WriteLine($"Lecture ID: {lecture.Id}");
                    Console.WriteLine($"Lecture Name: {lecture.LectureName}");
                    Console.WriteLine($"Lecturer: {lecture.LectureWorker}");
                    Console.WriteLine($"Lecture Faculty: {string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName))}");
                    Console.WriteLine();
                }

                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }


        public static void AllLecturesOfOneStudent()
        {
            try
            {
                using var dbContext = new UniContext();

                Console.WriteLine("Type ID of the student to view all lectures of the student ");
                string inputStudentId = Console.ReadLine();

                if (Guid.TryParse(inputStudentId, out Guid studentId))
                {
                    Student selectedStudent = dbContext.Students.FirstOrDefault(s => s.Id == studentId);

                    if (selectedStudent != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine($" ====== Lectures of Student: {selectedStudent.StudentFirstName} {selectedStudent.StudentLastName} {selectedStudent.Id} ======");

                        List<Lecture> studentLectures = dbContext.Lectures
                             .Where(l => l.LectureStudents.Any(ls => ls.Id == selectedStudent.Id))
                             .ToList();

                        foreach (var lecture in studentLectures)
                        {
                            Console.WriteLine($"Lecture ID: {lecture.Id}");
                            Console.WriteLine($"Lecture Name: {lecture.LectureName}");
                            Console.WriteLine($"Lecturer Name: {lecture.LectureWorker}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Student with ID {studentId} not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Student ID format. Please enter a valid GUID.");
                }

                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }


        private static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
