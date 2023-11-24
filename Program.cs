using Azure;
using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam;
using Student_Platform_DB_Exam.Models;
using System;
using static Student_Platform_DB_Exam.Models.Student;

namespace University_DB_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // ============ TO DO ============           


            //----------- program start ----------
            using var dbContext = new UniContext();

            MainMenu();


        }

        // // --------------------------------- END OF MAIN  ---------------------------------


        // ====================================== METHODS  ======================================        
        #region ShowMethods        
        public static void ShowAllFaculties()
        {
            using var dbContext = new UniContext();
            List<Faculty> allFacultyNames = dbContext.Faculties.ToList();
            Console.WriteLine();
            Console.WriteLine("=====================================");
            Console.WriteLine("All faculties:");
            foreach (Faculty faculty in allFacultyNames)
            {
                Console.WriteLine(faculty.FacultyName);
            };
            Console.WriteLine();
            Console.WriteLine("=====================================");
            InputKeyToContinue();
        }

        public static void ShowAllLectures()
        {
            using var dbContext = new UniContext();
            //List<Lecture> allLectureNames = dbContext.Lectures.ToList();
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
            };
            Console.WriteLine("=====================================");
            InputKeyToContinue();
        }

        //...show all students
        public static void ShowAllStudents()
        {
            using var dbContext = new UniContext();
            //var allStudents = dbContext.Students.ToList();
            var allStudents = dbContext.Students.Include(s => s.StudentFaculty).ToList();//to have access to StudentFaculty.FacultyName

            Console.WriteLine();

            if (allStudents.Any())
            {
                Console.WriteLine("=====================================");
                Console.WriteLine(" =========== All Students ===========");

                foreach (var student in allStudents)
                {
                    // Check if StudentFaculty is not null before accessing its properties
                    string facultyName = student.StudentFaculty != null ? student.StudentFaculty.FacultyName : "N/A";

                    Console.WriteLine($"{student.Id}: {student.StudentFirstName} {student.StudentLastName} || {student.StudentLevel} {student.StudentYear} || {student.StudentFaculty.FacultyName} || {student.StudentEmail}");
                }
                Console.WriteLine("=====================================");
            }
            else
            {
                Console.WriteLine("No students found.");
            }
            InputKeyToContinue();
        }

        //...show all students from a faculty
        public static void ShowAllStudentsFromFaculty()
        {
            using var dbContext = new UniContext();

            ShowAllFaculties();
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

        //...show all lectures of a faculty
        public static void ShowAllLecturesOfFaculty()
        {
            using var dbContext = new UniContext();
            Console.WriteLine("View all faculties(y/n)?");
            string viewChoice = Console.ReadLine();
            if (viewChoice == "y")
            {
                ShowAllFaculties();
            }

            Console.WriteLine();
            Console.WriteLine("Input the name of the faculty to view its lectures");
            string inputFaculty = Console.ReadLine();
            Faculty chosenFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == inputFaculty);

            if (chosenFaculty == null)
            {
                Console.WriteLine("Faculty not found. Exiting.");
                return;
            }

            //Access information from DB to show up later in the method
            List<Lecture> allLecturesByFaculty = dbContext.Lectures
                .Include(l => l.LectureFaculties)
                .Include(l => l.LectureStudents)
                .ToList();


            //all lectures that share the name of the selected faculty
            allLecturesByFaculty = dbContext.Lectures
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

        //...show all lectures of individual student
        public static void ShowAllLecturesOfOneStudent()
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

        #endregion

        //----------

        #region CreateNew


        // ...creates new faculty
        public static Faculty CreateNewFaculty()
        {
            using var dbContext = new UniContext();

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
            InputKeyToContinue();
            return facultyNew;

        }

        // ...creates new lecture
        public static Lecture CreateNewLecture()
        {
            using var dbContext = new UniContext();

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
            InputKeyToContinue();
            return lectureNew;
        }

        // ...creates new student
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
        #endregion

        //----------

        #region Add                    

        public static void AddLectureToFaculty()
        {
            using var dbContext = new UniContext();
            string confirmAddLecture = "n";

            while (confirmAddLecture == "n" || confirmAddLecture == "abort")
            {
                Console.WriteLine("Type ID of the lecture to update ");
                string inputLectureID = Console.ReadLine();

                if (Guid.TryParse(inputLectureID, out Guid lectureId))
                {
                    Lecture foundLecture = dbContext.Lectures
                        .Include(l => l.LectureFaculties)
                        .FirstOrDefault(l => l.Id == lectureId);

                    if (foundLecture != null)
                    {
                        Console.WriteLine($"Enter the name of the Faculty to add lecture {foundLecture.Id}: {foundLecture.LectureName} to");
                        string facultyNameToWhereAddLecture = Console.ReadLine();

                        Faculty foundFaculty = dbContext.Faculties
                            .SingleOrDefault(f => f.FacultyName == facultyNameToWhereAddLecture);

                        if (foundFaculty == null)
                        {
                            Console.WriteLine($"Faculty with name '{facultyNameToWhereAddLecture}' not found.");
                            continue;
                        }

                        Console.WriteLine("Confirm? (y/n)");
                        Console.WriteLine(@"Type {abort} to quit");
                        confirmAddLecture = Console.ReadLine().ToLower();

                        if (confirmAddLecture == "abort")
                        {
                            Console.WriteLine("Operation aborted");
                            break;
                        }

                        if (foundLecture.LectureFaculties == null)
                        {
                            foundLecture.LectureFaculties = new List<Faculty>();
                        }

                        foundLecture.LectureFaculties.Add(foundFaculty);

                        Console.WriteLine($"Lecture {foundLecture.Id}: {foundLecture.LectureName} added successfully to {foundFaculty.FacultyName}!");
                    }
                }

                Console.WriteLine("Do you want to add another lecture? (y/n)");
                confirmAddLecture = Console.ReadLine().ToLower();
            }

            InputKeyToContinue();
            dbContext.SaveChanges();
        }


        #endregion

        // --- update ---
        #region UpdateFaculty

        public static void UpdateFacultyData()
        {
            using var dbContext = new UniContext();

            ShowAllFaculties();

            Console.WriteLine("Enter the name of the faculty to update");
            string facultyNameToUpdate = Console.ReadLine();

            Faculty foundFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

            if (foundFaculty != null)
            {
                DisplayFacultyDetails(foundFaculty);

                Console.WriteLine(" =========== What to do? =========== ");
                Console.WriteLine("[1] Update Faculty || [2] Remove Faculty || [3] Abort");
                string facultyActionChoice = Console.ReadLine();

                switch (facultyActionChoice)
                {
                    case "1":
                        ProcessFacultyUpdate(foundFaculty);
                        break;

                    case "2":
                        ConfirmAndRemoveFaculty(foundFaculty);
                        break;

                    case "3":
                        Console.WriteLine("Operation aborted");
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
                ConfirmAndUpdateFaculty(foundFaculty);
            }
            else
            {
                Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found.");
            }
        }

        private static void DisplayFacultyDetails(Faculty faculty)
        {
            Console.WriteLine(" =========== Faculty found! ===========");
            Console.WriteLine($"{faculty.FacultyName}: Dean - {faculty.Id}");
        }

        private static void ProcessFacultyUpdate(Faculty foundFaculty)
        {
            Console.WriteLine(" =========== What to change? =========== ");
            Console.WriteLine("NAME || DEAN ||| [ABORT]");
            string facultyUpdateChoice = Console.ReadLine().ToUpper();

            switch (facultyUpdateChoice)
            {
                case "NAME":
                    UpdateFacultyName(foundFaculty);
                    break;

                case "DEAN":
                    UpdateFacultyDean(foundFaculty);
                    break;

                case "ABORT":
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private static void UpdateFacultyName(Faculty foundFaculty)
        {
            Console.WriteLine("Input a new name for the faculty");
            string facultyNewInputName = Console.ReadLine();
            foundFaculty.FacultyName = facultyNewInputName;
            Console.WriteLine($" New Faculty name is: {facultyNewInputName}");
            InputKeyToContinue();
        }

        private static void UpdateFacultyDean(Faculty foundFaculty)
        {
            using var dbContext = new UniContext();

            Console.WriteLine("Enter the first name of the new dean for the faculty");
            string newDeanFirstName = Console.ReadLine();

            Console.WriteLine("Enter the last name of the new dean for the faculty");
            string newDeanLastName = Console.ReadLine();

            Worker deanToUpdate = dbContext.Workers.FirstOrDefault(w =>
                w.WorkerFirstName == newDeanFirstName && w.WorkerLastName == newDeanLastName);

            if (deanToUpdate != null)
            {
                foundFaculty.FacultyDean = deanToUpdate;
                Console.WriteLine($"New Faculty Dean name is {deanToUpdate.WorkerFirstName} {deanToUpdate.WorkerLastName}");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine($"Dean with name '{newDeanFirstName} {newDeanLastName}' not found.");
                InputKeyToContinue();
            }
        }

        private static void ConfirmAndUpdateFaculty(Faculty foundFaculty)
        {
            using var dbContext = new UniContext();

            Console.WriteLine("Confirm Faculty updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            string confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "abort")
            {
                Console.WriteLine("Operation aborted");
                InputKeyToContinue();
                return;
            }
            else if (confirmUpdate == "y")
            {
                //Update faculty
                Console.WriteLine("Faculty updated successfully!");
                dbContext.SaveChanges();
                InputKeyToContinue();
                return;  //exit the loop
            }
            else
            {
                Console.WriteLine("Invalid input. Operation aborted");
            }
        }

        private static void ConfirmAndRemoveFaculty(Faculty foundFaculty)
        {
            using var dbContext = new UniContext();

            Console.WriteLine("Are you sure you want to remove this faculty? (y/n)");
            string confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "y")
            {
                dbContext.Faculties.Remove(foundFaculty);
                dbContext.SaveChanges();
                Console.WriteLine("Faculty removed successfully!");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine("Operation aborted");
            }
        }

        #endregion




        #region UpdateLecture


        private static void UpdateLecture()
        {
            using var dbContext = new UniContext();

            Console.WriteLine();
            ShowAllLectures();

            Console.WriteLine("Type ID of the lecture to update ");
            string inputLectureID = Console.ReadLine();

            if (Guid.TryParse(inputLectureID, out Guid lectureId))
            {
                Lecture foundLecture = dbContext.Lectures.FirstOrDefault(l => l.Id == lectureId);

                if (foundLecture != null)
                {
                    DisplayLectureDetails(foundLecture.Id);

                    Console.WriteLine(" =========== What to change? =========== ");
                    Console.WriteLine("NAME || WORKER || FACULTY || STUDENTS || REMOVE || [ABORT]");
                    string lectureActionChoice = Console.ReadLine().ToUpper();

                    ProcessLectureAction(foundLecture, lectureActionChoice, dbContext);

                    Console.WriteLine("Confirm updates? (y/n)");
                    Console.WriteLine(@"Type {abort} to quit");
                    string confirmUpdate = Console.ReadLine().ToLower();

                    if (confirmUpdate == "y")
                    {
                        Console.WriteLine("Lecture updated successfully!");
                        dbContext.SaveChanges();
                        InputKeyToContinue();
                    }
                    else if (confirmUpdate == "abort")
                    {
                        Console.WriteLine("Operation aborted");
                        InputKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Operation aborted");
                        InputKeyToContinue();
                    }
                }
                else
                {
                    Console.WriteLine("Lecture not found!");
                    InputKeyToContinue();
                }
            }
            else
            {
                Console.WriteLine("Invalid lecture ID format.");
                InputKeyToContinue();
            }
        }
        private static void ConfirmAndUpdateLecture(ref string confirmLecture, UniContext dbContext)
        {
            Console.WriteLine("Confirm Lecture updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            confirmLecture = Console.ReadLine().ToLower();

            if (confirmLecture == "abort")
            {
                Console.WriteLine("Operation aborted");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine("Lecture updated successfully!");
                dbContext.SaveChanges();
                InputKeyToContinue();
            }
        }

        private static void ProcessLectureAction(Lecture foundLecture, string lectureActionChoice, UniContext dbContext)
        {
            switch (lectureActionChoice)
            {
                case "NAME":
                    UpdateLectureName(foundLecture);
                    break;

                case "WORKER":
                    UpdateLectureWorker(foundLecture, dbContext);
                    break;

                case "FACULTY":
                    UpdateLectureFaculty(foundLecture, dbContext);
                    break;

                case "STUDENTS":
                    UpdateLectureStudents(foundLecture, dbContext);
                    break;

                case "REMOVE":
                    RemoveLecture();
                    break;

                case "ABORT":
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void UpdateLectureName(Lecture foundLecture)
        {
            Console.WriteLine("input a new name for the lecture");
            string lectureNewInputName = Console.ReadLine();
            foundLecture.LectureName = lectureNewInputName;
            Console.WriteLine($"New Lecture Name: {lectureNewInputName}");
            InputKeyToContinue();
        }

        private static void UpdateLectureWorker(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Select a new worker for the lecture");
            string workerOfLectureNameToChange = Console.ReadLine();
            Worker? foundWorker = dbContext.Workers.FirstOrDefault(w =>
                w.WorkerFirstName + " " + w.WorkerLastName == workerOfLectureNameToChange);

            if (foundWorker != null)
            {
                foundLecture.LectureWorker = foundWorker;
                Console.WriteLine($"The Lecturer for {foundLecture} is now {workerOfLectureNameToChange}");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine($"Worker with name '{workerOfLectureNameToChange}' not found.");
                InputKeyToContinue();
            }
        }

        private static void UpdateLectureFaculty(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Current Faculties: " + string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName)));
            Console.WriteLine("Options: [1] Add faculty, [2] Remove faculty, [3] Cancel");
            string facultyOption = Console.ReadLine();

            switch (facultyOption)
            {
                case "1":
                    AddExistingFacultyToLecture(foundLecture, dbContext);
                    break;

                case "2":
                    RemoveFacultyFromLecture(foundLecture, dbContext);
                    break;

                case "3":
                    // Cancel the faculty update
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void AddExistingFacultyToLecture(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Enter the name of the existing faculty to add");
            string facultyNameToAdd = Console.ReadLine();
            Faculty facultyToAdd = dbContext.Faculties.FirstOrDefault(f =>
                f.FacultyName == facultyNameToAdd);

            if (facultyToAdd != null && !foundLecture.LectureFaculties.Contains(facultyToAdd))
            {
                foundLecture.LectureFaculties.Add(facultyToAdd);
            }
            else if (facultyToAdd == null) { Console.WriteLine($"Faculty with name '{facultyNameToAdd}' not found."); InputKeyToContinue(); }
            else { Console.WriteLine("Faculty is already assigned to the lecture."); InputKeyToContinue(); }
        }

        private static void RemoveFacultyFromLecture(Lecture foundLecture, UniContext dbContext)
        {
            // Include related data
            var lecture = dbContext.Lectures
                .Include(l => l.LectureFaculties)
                .FirstOrDefault(l => l.Id == foundLecture.Id);

            Console.WriteLine("Current Faculties: " + string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName)));
            Console.WriteLine("Enter the name of the faculty to remove from the lecture");
            string facultyNameToRemove = Console.ReadLine();

            Faculty facultyToRemove = lecture.LectureFaculties.FirstOrDefault(f => f.FacultyName == facultyNameToRemove);

            if (facultyToRemove != null)
            {
                lecture.LectureFaculties.Remove(facultyToRemove);
                Console.WriteLine($"Faculty '{facultyNameToRemove}' removed from the lecture");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine($"Faculty '{facultyNameToRemove}' not found in the lecture.");
                InputKeyToContinue();
            }
        }


        private static void UpdateLectureStudents(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Current Students: " + string.Join(", ", foundLecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName)));
            Console.WriteLine("Options: [1] Add existing student, [2] Remove existing student, [3] Cancel");
            string studentOption = Console.ReadLine();

            switch (studentOption)
            {
                case "1":
                    AddExistingStudentToLecture(foundLecture, dbContext);
                    break;

                case "2":
                    RemoveExistingStudentFromLecture(foundLecture, dbContext);
                    break;

                case "3":
                    // Cancel the student update
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void AddExistingStudentToLecture(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Enter the ID of the existing student to add");
            string studentIdToAdd = Console.ReadLine();

            if (Guid.TryParse(studentIdToAdd, out Guid parsedStudentIdToAdd))
            {
                Student studentToAdd = dbContext.Students.FirstOrDefault(s => s.Id == parsedStudentIdToAdd);

                if (studentToAdd != null && !foundLecture.LectureStudents.Contains(studentToAdd))
                {
                    foundLecture.LectureStudents.Add(studentToAdd);
                }
                else if (studentToAdd == null)
                {
                    Console.WriteLine($"Student with ID '{studentIdToAdd}' not found.");
                    InputKeyToContinue();
                }
                else
                {
                    Console.WriteLine("Student is already assigned to the lecture.");
                    InputKeyToContinue();
                }
            }
            else
            {
                Console.WriteLine("Invalid student ID format.");
                InputKeyToContinue();
            }
        }

        private static void RemoveExistingStudentFromLecture(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Enter the ID of the existing student to remove");
            string studentIdToRemove = Console.ReadLine();

            if (Guid.TryParse(studentIdToRemove, out Guid parsedStudentIdToRemove))
            {
                Student studentToRemove = foundLecture.LectureStudents.FirstOrDefault(s => s.Id == parsedStudentIdToRemove);

                if (studentToRemove != null)
                {
                    foundLecture.LectureStudents.Remove(studentToRemove);
                    Console.WriteLine("Student removed");
                    InputKeyToContinue();
                }
                else
                {
                    Console.WriteLine($"Student with ID '{studentIdToRemove}' not found in the lecture.");
                    InputKeyToContinue();
                }
            }
            else
            {
                Console.WriteLine("Invalid student ID format.");
                InputKeyToContinue();
            }
        }

        public static void RemoveLecture()
        {
            using var dbContext = new UniContext();

            ShowAllLectures();

            Console.WriteLine("Enter the ID of the lecture to remove");
            string inputLectureID = Console.ReadLine();

            if (Guid.TryParse(inputLectureID, out Guid lectureId))
            {
                Lecture foundLecture = dbContext.Lectures.FirstOrDefault(l => l.Id == lectureId);

                if (foundLecture != null)
                {
                    DisplayLectureDetails(foundLecture.Id);

                    Console.WriteLine("Are you sure you want to remove this lecture? (y/n)");
                    string confirmRemove = Console.ReadLine().ToLower();

                    if (confirmRemove == "y")
                    {
                        dbContext.Lectures.Remove(foundLecture);
                        dbContext.SaveChanges();
                        Console.WriteLine("Lecture removed successfully!");
                        InputKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Operation aborted");
                    }
                }
                else
                {
                    Console.WriteLine("Lecture not found!");
                    InputKeyToContinue();
                }
            }
            else
            {
                Console.WriteLine("Invalid lecture ID format.");
                InputKeyToContinue();
            }
        }

        private static void DisplayLectureDetails(Guid lectureId)
        {
            using var dbContext = new UniContext();

            // Include related data
            var lecture = dbContext.Lectures
                .Include(l => l.LectureFaculties)
                .Include(l => l.LectureStudents)
                .Include(l => l.LectureWorker) // Include LectureWorker
                .FirstOrDefault(l => l.Id == lectureId);

            if (lecture == null)
            {
                Console.WriteLine("Lecture not found.");
                return;
            }

            //check if not null to avoid error.
            string facultyNames = lecture.LectureFaculties != null && lecture.LectureFaculties.Any()
                ? string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName))
                : "N/A";

            string workerName = lecture.LectureWorker != null
                ? $"{lecture.LectureWorker.WorkerFirstName} {lecture.LectureWorker.WorkerLastName}"
                : "N/A";

            string studentNames = lecture.LectureStudents != null
                ? string.Join(", ", lecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName))
                : "N/A";

            Console.WriteLine(" =========== Lecture Information ===========");
            Console.WriteLine($"{lecture.Id}: {lecture.LectureName} - {workerName} @ {facultyNames}");
            Console.WriteLine(" =========== Students in the Lecture ===========");
            Console.WriteLine(studentNames);
        }

        #endregion




        #region UpdateStudent

        public static void UpdateStudentData()
        {
            using var dbContext = new UniContext();
            string confirmUpdate = "n";

            while (confirmUpdate == "n" || confirmUpdate == "abort")
            {
                ShowAllStudents();
                Console.WriteLine("Type ID of the student to update ");
                string inputStudentID = Console.ReadLine();

                if (Guid.TryParse(inputStudentID, out Guid studentId))
                {
                    Student foundStudent = dbContext.Students.FirstOrDefault(s => s.Id == studentId);

                    if (foundStudent != null)
                    {
                        DisplayStudentDetails(foundStudent);

                        Console.WriteLine(" =========== What to change? =========== ");
                        Console.WriteLine("FIRST NAME || LAST NAME || FACULTY |||||| [ABORT]");
                        string studentActionChoice = Console.ReadLine().ToUpper();

                        ProcessStudentAction(foundStudent, studentActionChoice);

                        ConfirmAndUpdateStudent(out confirmUpdate);
                    }
                    else
                    {
                        Console.WriteLine("Student not found!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid student ID format.");
                }
            }
        }

        private static void DisplayStudentDetails(Student student)
        {
            Console.WriteLine();
            Console.WriteLine(" =========== Student found! ===========");
            Console.WriteLine($"{student.Id}: {student.StudentFirstName} {student.StudentLastName}");

            if (student.StudentFaculty != null)
            {
                Console.WriteLine($"@ {student.StudentFaculty.FacultyName} || YEAR: {student.StudentYear} || EMAIL: {student.StudentEmail}");
            }
            else
            {
                Console.WriteLine($"@ N/A || YEAR: {student.StudentYear} || EMAIL: {student.StudentEmail}");
            }
        }


        private static void ProcessStudentAction(Student foundStudent, string studentActionChoice)
        {
            switch (studentActionChoice)
            {
                case "FIRST NAME":
                    UpdateStudentFirstName(foundStudent);
                    break;

                case "LAST NAME":
                    UpdateStudentLastName(foundStudent);
                    break;

                case "FACULTY":
                    UpdateStudentFaculty(foundStudent);
                    break;

                case "ABORT":
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private static void UpdateStudentFirstName(Student foundStudent)
        {
            using var dbContext = new UniContext();
            Console.WriteLine("Input a new first name for the student");
            string studentNewInputFirstName = Console.ReadLine();
            foundStudent.StudentFirstName = studentNewInputFirstName;
            Console.WriteLine($"Student's First Name has been changed to {studentNewInputFirstName}");
            dbContext.SaveChanges();
            InputKeyToContinue();
        }

        private static void UpdateStudentLastName(Student foundStudent)
        {
            using var dbContext = new UniContext();
            Console.WriteLine("Input a new last name for the student");
            string studentNewInputLastName = Console.ReadLine();
            foundStudent.StudentLastName = studentNewInputLastName;
            Console.WriteLine($"Student's Last Name has been changed to {studentNewInputLastName}");
            dbContext.SaveChanges();
            InputKeyToContinue();
        }

        private static void UpdateStudentFaculty(Student foundStudent)
        {
            using var dbContext = new UniContext();

            // Load the found student from the database
            foundStudent = dbContext.Students.FirstOrDefault(s => s.Id == foundStudent.Id);

            Console.WriteLine("Enter the name of the new faculty for the student");
            string facultyNameToUpdate = Console.ReadLine();
            Faculty facultyToUpdate = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

            if (facultyToUpdate != null)
            {
                foundStudent.StudentFaculty = facultyToUpdate;

                // Save changes to the database
                dbContext.SaveChanges();

                Console.WriteLine($"Student's faculty updated successfully to {facultyToUpdate.FacultyName}!");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found.");
                InputKeyToContinue();
            }
        }


        private static void ConfirmAndUpdateStudent(out string confirmUpdate)
        {
            using var dbContext = new UniContext();
            Console.WriteLine("Confirm Student updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "abort")
            {
                Console.WriteLine("Operation aborted");
            }
            else if (confirmUpdate == "y")
            {
                Console.WriteLine("Student updated successfully!");
                dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Invalid input. Operation aborted");
            }
        }


        //update student lectures


        #endregion
        //adds lectures(s) to student


        #region Change
        //move student to another faculty (update)

        //remove student from faculty

        #endregion

        // ----------------- MENU METHODS-----------------


        // ----------------- END OF MENU METHODS-----------------
        // ----------------- DATABASE METHODS -----------------
        //method that shows menu

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                DisplayMainMenuOptions();

                Console.Write("Enter your choice: ");
                string userChoice = Console.ReadLine();

                ProcessMainMenuChoice(userChoice);
            }
        }
        private static void DisplayMainMenuOptions()
        {
            Console.WriteLine("========== Main Menu ==========");
            Console.WriteLine("1. Show");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Change");
            //Console.WriteLine("4. Add"); - OBSOLETE; All changes are done by CHANGE command
            Console.WriteLine("0. Exit");
        }

        private static void ProcessMainMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ShowMenu();
                    break;

                case "2":
                    CreateMenu();
                    break;

                case "3":
                    ChangeMenu();
                    break;

                case "4":
                    AddMenu();
                    break;

                case "0":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Show Menu ==========");
            Console.WriteLine("1. Show All Faculties");
            Console.WriteLine("2. Show All Lectures");
            Console.WriteLine("3. Show All Students");
            Console.WriteLine("4. Show Students From Faculty");
            Console.WriteLine("5. Show Lectures Of Faculty");
            Console.WriteLine("6. Show Lectures Of One Student");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessShowMenuChoice(userChoice);
        }

        private static void ProcessShowMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ShowAllFaculties();
                    break;

                case "2":
                    ShowAllLectures();
                    break;

                case "3":
                    ShowAllStudents();
                    break;

                case "4":
                    ShowAllStudentsFromFaculty();
                    break;

                case "5":
                    ShowAllLecturesOfFaculty();
                    break;

                case "6":
                    ShowAllLecturesOfOneStudent();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void CreateMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Create Menu ==========");
            Console.WriteLine("1. Create New Faculty");
            Console.WriteLine("2. Create New Lecture");
            Console.WriteLine("3. Create New Student");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessCreateMenuChoice(userChoice);
        }

        private static void ProcessCreateMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    CreateNewFaculty();
                    break;

                case "2":
                    CreateNewLecture();
                    break;

                case "3":
                    CreateNewStudent();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void ChangeMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Change Menu ==========");
            Console.WriteLine("1. Update Faculty Data");
            Console.WriteLine("2. Update Lecture");
            Console.WriteLine("3. Update Student Data");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessChangeMenuChoice(userChoice);
        }

        private static void ProcessChangeMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    UpdateFacultyData();
                    break;

                case "2":
                    UpdateLecture();
                    break;

                case "3":
                    UpdateStudentData();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        public static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }


        //=======================================================================================================================================================================
        // OBSOLETE
        public void UpdateLectureBACKUP()
        {
            using var dbContext = new UniContext();
            string confirmLecture = "n";

            while (confirmLecture == "n" || confirmLecture == "abort")
            {
                Console.WriteLine("Type ID of the lecture to update ");
                string inputLectureID = Console.ReadLine();

                if (Guid.TryParse(inputLectureID, out Guid lectureId))
                {
                    Lecture foundLecture = dbContext.Lectures.FirstOrDefault(l => l.Id == lectureId);

                    if (foundLecture != null)
                    {
                        //view data of one lecture
                        string facultyNames = string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName));
                        string studentNames = string.Join(", ", foundLecture.LectureStudents.Select(f => f.Id));

                        Console.WriteLine(" =========== Lecture found! ===========");
                        Console.WriteLine($"{foundLecture.Id}: {foundLecture.LectureName} - {foundLecture.LectureWorker} @ {facultyNames}");
                        Console.WriteLine(" =========== Students of the class:");
                        Console.WriteLine($"{foundLecture.LectureStudents}");

                        Console.WriteLine(" =========== What to change? =========== ");
                        Console.WriteLine("NAME || WORKER || FACULTY || STUDENTS |||||| [ABORT]");
                        string lectureActionChoice = Console.ReadLine().ToUpper();


                        switch (lectureActionChoice)
                        {
                            case "NAME":
                                Console.WriteLine("input a new name for the lecture");
                                string lectureNewInputName = Console.ReadLine();
                                foundLecture.LectureName = lectureNewInputName;
                                break;

                            case "WORKER":
                                Console.WriteLine("Select a new worker for the lecture");
                                string workerOfLectureNameToChange = Console.ReadLine();
                                Worker? foundWorker = dbContext.Workers.FirstOrDefault(w =>
                                w.WorkerFirstName + " " + w.WorkerLastName == workerOfLectureNameToChange);

                                if (foundWorker != null)
                                {
                                    foundLecture.LectureWorker = foundWorker;
                                }
                                else
                                {
                                    Console.WriteLine($"Worker with name '{workerOfLectureNameToChange}' not found.");
                                }
                                break;


                            case "FACULTY":
                                Console.WriteLine("Current Faculties: " + string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName)));
                                Console.WriteLine("Options: [1] Add existing faculty, [2] Update existing faculty, [3] Cancel");
                                string facultyOption = Console.ReadLine();

                                switch (facultyOption)
                                {
                                    case "1":
                                        Console.WriteLine("Enter the name of the existing faculty to add");
                                        string facultyNameToAdd = Console.ReadLine();
                                        Faculty? facultyToAdd = dbContext.Faculties.FirstOrDefault(f =>
                                        f.FacultyName == facultyNameToAdd);

                                        if (facultyToAdd != null && !foundLecture.LectureFaculties.Contains(facultyToAdd))
                                        {
                                            foundLecture.LectureFaculties.Add(facultyToAdd);
                                        }
                                        else if (facultyToAdd == null) { Console.WriteLine($"Faculty with name '{facultyNameToAdd}' not found."); }
                                        else { Console.WriteLine("Faculty is already assigned to the lecture."); }
                                        break;

                                    case "2":
                                        Console.WriteLine("Enter the name of the faculty to update");
                                        string facultyNameToUpdate = Console.ReadLine();
                                        Faculty facultyToUpdate = foundLecture.LectureFaculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

                                        if (facultyToUpdate != null)
                                        {
                                            Console.WriteLine("Enter the new name for the faculty");
                                            string newFacultyNameToUpdate = Console.ReadLine();
                                            facultyToUpdate.FacultyName = newFacultyNameToUpdate;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found.");
                                        }
                                        break;

                                    case "3":
                                        // Cancel the faculty update
                                        break;

                                    default:
                                        Console.WriteLine("Invalid option");
                                        break;
                                }
                                break;


                            case "STUDENTS":
                                Console.WriteLine("Current Students: " + string.Join(", ", foundLecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName)));
                                Console.WriteLine("Options: [1] Add existing student, [2] Remove existing student, [3] Cancel");
                                string studentOption = Console.ReadLine();

                                switch (studentOption)
                                {
                                    case "1":
                                        Console.WriteLine("Enter the ID of the existing student to add");
                                        string studentIdToAdd = Console.ReadLine();

                                        if (Guid.TryParse(studentIdToAdd, out Guid parsedStudentIdToAdd))
                                        {
                                            Student studentToAdd = dbContext.Students.FirstOrDefault(s => s.Id == parsedStudentIdToAdd);

                                            if (studentToAdd != null && !foundLecture.LectureStudents.Contains(studentToAdd))
                                            {
                                                foundLecture.LectureStudents.Add(studentToAdd);
                                            }
                                            else if (studentToAdd == null)
                                            {
                                                Console.WriteLine($"Student with ID '{studentIdToAdd}' not found.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Student is already assigned to the lecture.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid student ID format.");
                                        }
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter the ID of the existing student to remove");
                                        string studentIdToRemove = Console.ReadLine();

                                        if (Guid.TryParse(studentIdToRemove, out Guid parsedStudentIdToRemove))
                                        {
                                            Student studentToRemove = foundLecture.LectureStudents.FirstOrDefault(s => s.Id == parsedStudentIdToRemove);

                                            if (studentToRemove != null)
                                            {
                                                foundLecture.LectureStudents.Remove(studentToRemove);
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Student with ID '{studentIdToRemove}' not found in the lecture.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid student ID format.");
                                        }
                                        break;
                                    case "3":
                                        // Cancel the student update
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option");
                                        break;
                                }
                                break;




                            case "ABORT":
                                break;

                        }

                        Console.WriteLine("Confirm Lecture updates? (y/n)");
                        Console.WriteLine(@"Type {abort} to quit");
                        confirmLecture = Console.ReadLine().ToLower();

                        if (confirmLecture == "abort")
                        {
                            Console.WriteLine("Operation aborted");
                            break;
                        }
                    }
                    Console.WriteLine("Lecture updated successfully!");
                    dbContext.SaveChanges();
                }

                // ================================== END OF METHODS  ===================================
            }
        }

        public void ShowOneStudentData()
        {
            using var dbContext = new UniContext();
            List<string> oneStudentData = dbContext.Students
                .Select(s =>
                $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}  || FACULTY: {s.StudentFaculty} || YEAR: {s.StudentYear} || EMAIL: {s.StudentEmail}") //sitoj vietoje dar reikia parodyti kokias lectures eina
                .ToList();
        }

        public static void AddStudentToFaculty()
        {
            using var dbContext = new UniContext();
            string confirmAddStudent = "n";

            while (confirmAddStudent == "n" || confirmAddStudent == "abort")
            {
                Console.WriteLine("Type ID of the student: ");
                string inputStudentId = Console.ReadLine();

                if (Guid.TryParse(inputStudentId, out Guid studentId))
                {
                    Student selectedStudent = dbContext.Students.FirstOrDefault(s => s.Id == studentId);

                    if (selectedStudent != null)
                    {
                        Console.WriteLine($"Enter the name of the Faculty to add student {selectedStudent.Id}: {selectedStudent.StudentFirstName} {selectedStudent.StudentLastName} to");
                        string facultyNameToWhereAddStudent = Console.ReadLine();

                        // find Faculty by name
                        Faculty foundFaculty = dbContext.Faculties.SingleOrDefault(f => f.FacultyName == facultyNameToWhereAddStudent);

                        if (foundFaculty == null)
                        {
                            Console.WriteLine($"Faculty with name '{facultyNameToWhereAddStudent}' not found.");
                            continue;
                        }


                        Console.WriteLine("Confirm? (y/n)");
                        Console.WriteLine(@"Type {abort} to quit");
                        confirmAddStudent = Console.ReadLine().ToLower();

                        if (confirmAddStudent == "abort")
                        {
                            Console.WriteLine("Operation aborted");
                            break;
                        }


                        //UPDATE
                        selectedStudent.StudentFaculty = foundFaculty;

                        Console.WriteLine($"Student {selectedStudent.Id}: {selectedStudent.StudentFirstName} {selectedStudent.StudentLastName} added successfully to {foundFaculty.FacultyName}!");

                        //ask whether to add another student?? - no time                        
                    }
                }
            }
            InputKeyToContinue();
            dbContext.SaveChanges();
        }

        private static void DisplayLectureDetailsERROR(Lecture lecture)
        {
            if (lecture != null)
            {
                string facultyNames = string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName));
                string studentNames = string.Join(", ", lecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName));

                Console.WriteLine(" =========== Lecture found! ===========");
                Console.WriteLine($"{lecture.Id}: {lecture.LectureName} - {lecture.LectureWorker} @ {facultyNames}");
                Console.WriteLine(" =========== Students of the class:");
                Console.WriteLine(studentNames);
                InputKeyToContinue();
            }
        }

        private static void AddMenu()
        {
            Console.Clear();
            Console.WriteLine("========== Add Menu ==========");
            Console.WriteLine("1. Add Student To Faculty");
            Console.WriteLine("2. Add Lecture To Faculty");

            Console.Write("Enter your choice: ");
            string userChoice = Console.ReadLine();

            ProcessAddMenuChoice(userChoice);
        }

        private static void ProcessAddMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    AddStudentToFaculty();
                    break;

                case "2":
                    AddLectureToFaculty();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    InputKeyToContinue();
                    break;
            }
        }

        private static void UpdateExistingFacultyInLecture(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Enter the name of the faculty to update");
            string facultyNameToUpdate = Console.ReadLine();
            Faculty facultyToUpdate = foundLecture.LectureFaculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

            if (facultyToUpdate != null)
            {
                Console.WriteLine("Enter the new name for the faculty");
                string newFacultyNameToUpdate = Console.ReadLine();
                facultyToUpdate.FacultyName = newFacultyNameToUpdate;
                Console.WriteLine($"The new name of the faculty is now {newFacultyNameToUpdate}");
                InputKeyToContinue();
            }
            else
            {
                Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found."); InputKeyToContinue();
            }
        }
    }
}
