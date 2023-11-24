using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_DB_Exam;

namespace Student_Platform_DB_Exam
{
    internal class Trash_Bin_for_Obsoletes
    {
        //public void UpdateLectureBACKUP()
        //{
        //    using var dbContext = new UniContext();
        //    string confirmLecture = "n";

        //    while (confirmLecture == "n" || confirmLecture == "abort")
        //    {
        //        Console.WriteLine("Type ID of the lecture to update ");
        //        string inputLectureID = Console.ReadLine();

        //        if (Guid.TryParse(inputLectureID, out Guid lectureId))
        //        {
        //            Lecture foundLecture = dbContext.Lectures.FirstOrDefault(l => l.Id == lectureId);

        //            if (foundLecture != null)
        //            {
        //                //view data of one lecture
        //                string facultyNames = string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName));
        //                string studentNames = string.Join(", ", foundLecture.LectureStudents.Select(f => f.Id));

        //                Console.WriteLine(" =========== Lecture found! ===========");
        //                Console.WriteLine($"{foundLecture.Id}: {foundLecture.LectureName} - {foundLecture.LectureWorker} @ {facultyNames}");
        //                Console.WriteLine(" =========== Students of the class:");
        //                Console.WriteLine($"{foundLecture.LectureStudents}");

        //                Console.WriteLine(" =========== What to change? =========== ");
        //                Console.WriteLine("NAME || WORKER || FACULTY || STUDENTS |||||| [ABORT]");
        //                string lectureActionChoice = Console.ReadLine().ToUpper();


        //                switch (lectureActionChoice)
        //                {
        //                    case "NAME":
        //                        Console.WriteLine("input a new name for the lecture");
        //                        string lectureNewInputName = Console.ReadLine();
        //                        foundLecture.LectureName = lectureNewInputName;
        //                        break;

        //                    case "WORKER":
        //                        Console.WriteLine("Select a new worker for the lecture");
        //                        string workerOfLectureNameToChange = Console.ReadLine();
        //                        Worker? foundWorker = dbContext.Workers.FirstOrDefault(w =>
        //                        w.WorkerFirstName + " " + w.WorkerLastName == workerOfLectureNameToChange);

        //                        if (foundWorker != null)
        //                        {
        //                            foundLecture.LectureWorker = foundWorker;
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine($"Worker with name '{workerOfLectureNameToChange}' not found.");
        //                        }
        //                        break;


        //                    case "FACULTY":
        //                        Console.WriteLine("Current Faculties: " + string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName)));
        //                        Console.WriteLine("Options: [1] Add existing faculty, [2] Update existing faculty, [3] Cancel");
        //                        string facultyOption = Console.ReadLine();

        //                        switch (facultyOption)
        //                        {
        //                            case "1":
        //                                Console.WriteLine("Enter the name of the existing faculty to add");
        //                                string facultyNameToAdd = Console.ReadLine();
        //                                Faculty? facultyToAdd = dbContext.Faculties.FirstOrDefault(f =>
        //                                f.FacultyName == facultyNameToAdd);

        //                                if (facultyToAdd != null && !foundLecture.LectureFaculties.Contains(facultyToAdd))
        //                                {
        //                                    foundLecture.LectureFaculties.Add(facultyToAdd);
        //                                }
        //                                else if (facultyToAdd == null) { Console.WriteLine($"Faculty with name '{facultyNameToAdd}' not found."); }
        //                                else { Console.WriteLine("Faculty is already assigned to the lecture."); }
        //                                break;

        //                            case "2":
        //                                Console.WriteLine("Enter the name of the faculty to update");
        //                                string facultyNameToUpdate = Console.ReadLine();
        //                                Faculty facultyToUpdate = foundLecture.LectureFaculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

        //                                if (facultyToUpdate != null)
        //                                {
        //                                    Console.WriteLine("Enter the new name for the faculty");
        //                                    string newFacultyNameToUpdate = Console.ReadLine();
        //                                    facultyToUpdate.FacultyName = newFacultyNameToUpdate;
        //                                }
        //                                else
        //                                {
        //                                    Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found.");
        //                                }
        //                                break;

        //                            case "3":
        //                                // Cancel the faculty update
        //                                break;

        //                            default:
        //                                Console.WriteLine("Invalid option");
        //                                break;
        //                        }
        //                        break;


        //                    case "STUDENTS":
        //                        Console.WriteLine("Current Students: " + string.Join(", ", foundLecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName)));
        //                        Console.WriteLine("Options: [1] Add existing student, [2] Remove existing student, [3] Cancel");
        //                        string studentOption = Console.ReadLine();

        //                        switch (studentOption)
        //                        {
        //                            case "1":
        //                                Console.WriteLine("Enter the ID of the existing student to add");
        //                                string studentIdToAdd = Console.ReadLine();

        //                                if (Guid.TryParse(studentIdToAdd, out Guid parsedStudentIdToAdd))
        //                                {
        //                                    Student studentToAdd = dbContext.Students.FirstOrDefault(s => s.Id == parsedStudentIdToAdd);

        //                                    if (studentToAdd != null && !foundLecture.LectureStudents.Contains(studentToAdd))
        //                                    {
        //                                        foundLecture.LectureStudents.Add(studentToAdd);
        //                                    }
        //                                    else if (studentToAdd == null)
        //                                    {
        //                                        Console.WriteLine($"Student with ID '{studentIdToAdd}' not found.");
        //                                    }
        //                                    else
        //                                    {
        //                                        Console.WriteLine("Student is already assigned to the lecture.");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Console.WriteLine("Invalid student ID format.");
        //                                }
        //                                break;
        //                            case "2":
        //                                Console.WriteLine("Enter the ID of the existing student to remove");
        //                                string studentIdToRemove = Console.ReadLine();

        //                                if (Guid.TryParse(studentIdToRemove, out Guid parsedStudentIdToRemove))
        //                                {
        //                                    Student studentToRemove = foundLecture.LectureStudents.FirstOrDefault(s => s.Id == parsedStudentIdToRemove);

        //                                    if (studentToRemove != null)
        //                                    {
        //                                        foundLecture.LectureStudents.Remove(studentToRemove);
        //                                    }
        //                                    else
        //                                    {
        //                                        Console.WriteLine($"Student with ID '{studentIdToRemove}' not found in the lecture.");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Console.WriteLine("Invalid student ID format.");
        //                                }
        //                                break;
        //                            case "3":
        //                                // Cancel the student update
        //                                break;
        //                            default:
        //                                Console.WriteLine("Invalid option");
        //                                break;
        //                        }
        //                        break;




        //                    case "ABORT":
        //                        break;

        //                }

        //                Console.WriteLine("Confirm Lecture updates? (y/n)");
        //                Console.WriteLine(@"Type {abort} to quit");
        //                confirmLecture = Console.ReadLine().ToLower();

        //                if (confirmLecture == "abort")
        //                {
        //                    Console.WriteLine("Operation aborted");
        //                    break;
        //                }
        //            }
        //            Console.WriteLine("Lecture updated successfully!");
        //            dbContext.SaveChanges();
        //        }

        //        // ================================== END OF METHODS  ===================================
        //    }
        //}

        //public void ShowOneStudentData()
        //{
        //    using var dbContext = new UniContext();
        //    List<string> oneStudentData = dbContext.Students
        //        .Select(s =>
        //        $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}  || FACULTY: {s.StudentFaculty} || YEAR: {s.StudentYear} || EMAIL: {s.StudentEmail}") //sitoj vietoje dar reikia parodyti kokias lectures eina
        //        .ToList();
        //}

        //public static void AddStudentToFaculty()
        //{
        //    using var dbContext = new UniContext();
        //    string confirmAddStudent = "n";

        //    while (confirmAddStudent == "n" || confirmAddStudent == "abort")
        //    {
        //        Console.WriteLine("Type ID of the student: ");
        //        string inputStudentId = Console.ReadLine();

        //        if (Guid.TryParse(inputStudentId, out Guid studentId))
        //        {
        //            Student selectedStudent = dbContext.Students.FirstOrDefault(s => s.Id == studentId);

        //            if (selectedStudent != null)
        //            {
        //                Console.WriteLine($"Enter the name of the Faculty to add student {selectedStudent.Id}: {selectedStudent.StudentFirstName} {selectedStudent.StudentLastName} to");
        //                string facultyNameToWhereAddStudent = Console.ReadLine();

        //                // find Faculty by name
        //                Faculty foundFaculty = dbContext.Faculties.SingleOrDefault(f => f.FacultyName == facultyNameToWhereAddStudent);

        //                if (foundFaculty == null)
        //                {
        //                    Console.WriteLine($"Faculty with name '{facultyNameToWhereAddStudent}' not found.");
        //                    continue;
        //                }


        //                Console.WriteLine("Confirm? (y/n)");
        //                Console.WriteLine(@"Type {abort} to quit");
        //                confirmAddStudent = Console.ReadLine().ToLower();

        //                if (confirmAddStudent == "abort")
        //                {
        //                    Console.WriteLine("Operation aborted");
        //                    break;
        //                }


        //                //UPDATE
        //                selectedStudent.StudentFaculty = foundFaculty;

        //                Console.WriteLine($"Student {selectedStudent.Id}: {selectedStudent.StudentFirstName} {selectedStudent.StudentLastName} added successfully to {foundFaculty.FacultyName}!");

        //                //ask whether to add another student?? - no time                        
        //            }
        //        }
        //    }
        //    InputKeyToContinue();
        //    dbContext.SaveChanges();
        //}

        //private static void DisplayLectureDetailsERROR(Lecture lecture)
        //{
        //    if (lecture != null)
        //    {
        //        string facultyNames = string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName));
        //        string studentNames = string.Join(", ", lecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName));

        //        Console.WriteLine(" =========== Lecture found! ===========");
        //        Console.WriteLine($"{lecture.Id}: {lecture.LectureName} - {lecture.LectureWorker} @ {facultyNames}");
        //        Console.WriteLine(" =========== Students of the class:");
        //        Console.WriteLine(studentNames);
        //        InputKeyToContinue();
        //    }
        //}

        //private static void AddMenu()
        //{
        //    Console.Clear();
        //    Console.WriteLine("========== Add Menu ==========");
        //    Console.WriteLine("1. Add Student To Faculty");
        //    Console.WriteLine("2. Add Lecture To Faculty");

        //    Console.Write("Enter your choice: ");
        //    string userChoice = Console.ReadLine();

        //    ProcessAddMenuChoice(userChoice);
        //}

        //private static void ProcessAddMenuChoice(string choice)
        //{
        //    switch (choice)
        //    {
        //        case "1":
        //            AddStudentToFaculty();
        //            break;

        //        case "2":
        //            AddLectureToFaculty();
        //            break;

        //        default:
        //            Console.WriteLine("Invalid choice. Please enter a valid option.");
        //            InputKeyToContinue();
        //            break;
        //    }
        //}

        //private static void UpdateExistingFacultyInLecture(Lecture foundLecture, UniContext dbContext)
        //{
        //    Console.WriteLine("Enter the name of the faculty to update");
        //    string facultyNameToUpdate = Console.ReadLine();
        //    Faculty facultyToUpdate = foundLecture.LectureFaculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

        //    if (facultyToUpdate != null)
        //    {
        //        Console.WriteLine("Enter the new name for the faculty");
        //        string newFacultyNameToUpdate = Console.ReadLine();
        //        facultyToUpdate.FacultyName = newFacultyNameToUpdate;
        //        Console.WriteLine($"The new name of the faculty is now {newFacultyNameToUpdate}");
        //        InputKeyToContinue();
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found."); InputKeyToContinue();
        //    }
        //}

        // ----------------- ADD  -----------------

        //public static void AddLectureToFaculty()
        //{
        //    using var dbContext = new UniContext();
        //    string confirmAddLecture = "n";

        //    while (confirmAddLecture == "n" || confirmAddLecture == "abort")
        //    {
        //        Console.WriteLine("Type ID of the lecture to update ");
        //        string inputLectureID = Console.ReadLine();

        //        if (Guid.TryParse(inputLectureID, out Guid lectureId))
        //        {
        //            Lecture foundLecture = dbContext.Lectures
        //                .Include(l => l.LectureFaculties)
        //                .FirstOrDefault(l => l.Id == lectureId);

        //            if (foundLecture != null)
        //            {
        //                Console.WriteLine($"Enter the name of the Faculty to add lecture {foundLecture.Id}: {foundLecture.LectureName} to");
        //                string facultyNameToWhereAddLecture = Console.ReadLine();

        //                Faculty foundFaculty = dbContext.Faculties
        //                    .SingleOrDefault(f => f.FacultyName == facultyNameToWhereAddLecture);

        //                if (foundFaculty == null)
        //                {
        //                    Console.WriteLine($"Faculty with name '{facultyNameToWhereAddLecture}' not found.");
        //                    continue;
        //                }

        //                Console.WriteLine("Confirm? (y/n)");
        //                Console.WriteLine(@"Type {abort} to quit");
        //                confirmAddLecture = Console.ReadLine().ToLower();

        //                if (confirmAddLecture == "abort")
        //                {
        //                    Console.WriteLine("Operation aborted");
        //                    break;
        //                }

        //                if (foundLecture.LectureFaculties == null)
        //                {
        //                    foundLecture.LectureFaculties = new List<Faculty>();
        //                }

        //                foundLecture.LectureFaculties.Add(foundFaculty);

        //                Console.WriteLine($"Lecture {foundLecture.Id}: {foundLecture.LectureName} added successfully to {foundFaculty.FacultyName}!");
        //            }
        //        }

        //        Console.WriteLine("Do you want to add another lecture? (y/n)");
        //        confirmAddLecture = Console.ReadLine().ToLower();
        //    }

        //    InputKeyToContinue();
        //    dbContext.SaveChanges();
        //}
    }
}
