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

            //========
            //DELETE
            //var page = new Page(new Guid("0915CC69-5CF9-4749-9DE3-C189A0950E7F"));//paduodame ID pagal kuri triname
            //dbContext.Pages.Remove(page);
            //dbContext.SaveChanges();


            //SELECT
            //var page = dbContext.Pages.FirstOrDefault();//pirmas psl duombazej. galima naudoti linq.
            //cia savechanges nereikia, nes nieko nekeiciam, tiesiog view
            //var page = dbContext.Pages.Where(p=>p.Number == 2);


            //UPDATE
            //var page = dbContext.Pages.First(p => p.Id == Guid.Parse("F7A7849A-E174-4C6B-B6CF-0B4C301FFB2C"));
            //page.Content += ". Added new content.";
            //dbContext.SaveChanges();

            //----------- program start ----------
            using var dbContext = new UniContext();

            //while (true)
            //{
            //    ShowMenu(myRestaurant, menuManager);
            //}


        }

        // // --------------------------------- END OF MAIN  ---------------------------------


        // ====================================== METHODS  ======================================

        // A METHOD THAT ........

        // ...shows menu
        // ...returns to main menu

        //----------
        #region ShowMethods

        //...show all faculties
        public void ShowAllFaculties()
        {
            var dbContext = new UniContext();
            List<Faculty> allFacultyNames = dbContext.Faculties.ToList();

            foreach (Faculty faculty in allFacultyNames)
            {
                Console.WriteLine(faculty.FacultyName);
            };
            Console.WriteLine();

        }

        //...show all lectures
        public void ShowAllLectures()
        {
            var dbContext = new UniContext();
            List<Lecture> allLectureNames = dbContext.Lectures.ToList();

            foreach (Lecture lecture in allLectureNames)
            {
                string facultyNames = string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName));

                Console.WriteLine($"{lecture.LectureName}: {lecture.Id} - {lecture.LectureWorker} @ {facultyNames}");
                Console.WriteLine();
            };

        }

        //...show all students
        public void ShowAllStudents()
        {
            var dbContext = new UniContext();
            List<string> allStudentNames = dbContext.Students.Select(s => $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}").ToList();

            Console.WriteLine(allStudentNames);
        }


        //...show all students from a faculty
        public void ShowAllStudentsFromFaculty()
        {
            var dbContext = new UniContext();

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
        }






        //...show all lectures of a faculty
        public void ShowAllLecturesOfFaculty()
        {
            var dbContext = new UniContext();
            Console.WriteLine("View all faculties?");
            string viewChoice = Console.ReadLine();
            if (viewChoice == "y")
            {
                ShowAllFaculties();
            }

            Console.WriteLine("Select a faculty to view its lectures");
            string inputFaculty = Console.ReadLine();
            Faculty chosenFaculty = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == inputFaculty);

            if (chosenFaculty == null)
            {
                Console.WriteLine("Faculty not found. Exiting.");
                return;
            }

            //all lectures that share the name of the selected faculty
            List<Lecture> allLecturesByFaculty = dbContext.Lectures
                .Where(l => l.LectureFaculties.Any(n => n.FacultyName == chosenFaculty.FacultyName))
                .ToList();



            Console.WriteLine($" ====== Lectures in {chosenFaculty.FacultyName} ======");

            foreach (var lecture in allLecturesByFaculty)
            {
                Console.WriteLine($"Lecture ID: {lecture.Id}");
                Console.WriteLine($"Lecture Name: {lecture.LectureName}");
                Console.WriteLine($"Lecturer: {lecture.LectureWorker}");
                Console.WriteLine($"Lecture Faculty: {string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName))}");
                Console.WriteLine();
            }

        }
        //...show all lectures of individual student
        public void ShowAllLecturesOfOneStudent()
        {
            var dbContext = new UniContext();

            // If you don't know which student you want
            ShowAllStudentsFromFaculty();
            Console.WriteLine("Type ID of the student to view all lectures of the student ");
            string inputStudentId = Console.ReadLine();

            if (Guid.TryParse(inputStudentId, out Guid studentId))
            {
                Student selectedStudent = dbContext.Students.FirstOrDefault(s => s.Id == studentId);

                if (selectedStudent != null)
                {
                    Console.WriteLine($" ====== Lectures of Student: {selectedStudent.Id} ======");

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

            //ask if user wants to do any more operations with this student
        }

        public void ShowOneLectureData()
        {

        }

        #endregion

        //----------

        #region CreateNew


        // ...creates new faculty
        public Faculty CreateNewFaculty()
        {
            var dbContext = new UniContext();

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

        // ...creates new lecture
        public Lecture CreateNewLecture()
        {
            var dbContext = new UniContext();

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

        // ...creates new student
        public Student CreateNewStudent()
        {
            var dbContext = new UniContext();
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
        public void AddStudentToFaculty()
        {
            var dbContext = new UniContext();
            string confirmAddStudent = "n";

            while (confirmAddStudent == "n" || confirmAddStudent == "abort")
            {
                Console.WriteLine("Type ID of the student to view all lectures of the student ");
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

                        //ask whether to add another student??                        
                    }
                }
            }
            dbContext.SaveChanges();
        }

        // --- update ---
        #region UpdateFaculty

        public void UpdateFacultyData()
        {
            var dbContext = new UniContext();
            string confirmUpdate = "n";

            while (confirmUpdate == "n" || confirmUpdate == "abort")
            {
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
                            ConfirmAndUpdateFaculty(confirmUpdate);
                            break;

                        case "2":
                            ConfirmAndRemoveFaculty(foundFaculty, confirmUpdate);
                            break;

                        case "3":
                            Console.WriteLine("Operation aborted");
                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found.");
                }
            }
        }

        private static void DisplayFacultyDetails(Faculty faculty)
        {
            Console.WriteLine(" =========== Faculty found! ===========");
            Console.WriteLine($"{faculty.FacultyName}: Dean - {faculty.FacultyDean?.WorkerFirstName} {faculty.FacultyDean.WorkerLastName}");
        }

        private void ProcessFacultyUpdate(Faculty foundFaculty)
        {
            Console.WriteLine(" =========== What to change? =========== ");
            Console.WriteLine("NAME || DEAN |||||| [ABORT]");
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
        }

        private void UpdateFacultyDean(Faculty foundFaculty)
        {
            var dbContext = new UniContext();

            Console.WriteLine("Enter the ID of the new dean for the faculty");
            string deanIdToUpdate = Console.ReadLine();

            if (Guid.TryParse(deanIdToUpdate, out Guid parsedDeanIdToUpdate))
            {
                Worker deanToUpdate = dbContext.Workers.FirstOrDefault(w => w.Id == parsedDeanIdToUpdate);

                if (deanToUpdate != null)
                {
                    foundFaculty.FacultyDean = deanToUpdate;
                }
                else
                {
                    Console.WriteLine($"Dean with ID '{deanIdToUpdate}' not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid dean ID format.");
            }
        }

        private static void ConfirmAndUpdateFaculty(string confirmUpdate)
        {
            var dbContext = new UniContext();
            Console.WriteLine("Confirm Faculty updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "abort")
            {
                Console.WriteLine("Operation aborted");
            }
            else
            {
                Console.WriteLine("Faculty updated successfully!");
                dbContext.SaveChanges();
            }
        }

        private void ConfirmAndRemoveFaculty(Faculty foundFaculty, string confirmUpdate)
        {
            var dbContext = new UniContext();

            Console.WriteLine("Are you sure you want to remove this faculty? (y/n)");
            confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "y")
            {
                dbContext.Faculties.Remove(foundFaculty);
                dbContext.SaveChanges();
                Console.WriteLine("Faculty removed successfully!");
            }
            else
            {
                Console.WriteLine("Operation aborted");
            }
        }

        //update faculty lectures (add/update/remove)

        #endregion




        #region UpdateLecture


        public void UpdateLecture()
        {
            var dbContext = new UniContext();
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
                        DisplayLectureDetails(foundLecture);

                        Console.WriteLine(" =========== What to change? =========== ");
                        Console.WriteLine("NAME || WORKER || FACULTY || STUDENTS |||||| [ABORT]");
                        string lectureActionChoice = Console.ReadLine().ToUpper();

                        ProcessLectureAction(foundLecture, lectureActionChoice, dbContext);

                        ConfirmAndUpdateLecture(confirmLecture, dbContext);
                    }
                    else
                    {
                        Console.WriteLine("Lecture not found!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid lecture ID format.");
                }
            }
        }

        private static void ConfirmAndUpdateLecture(string confirmLecture, UniContext dbContext)
        {
            Console.WriteLine("Confirm Lecture updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            confirmLecture = Console.ReadLine().ToLower();

            if (confirmLecture == "abort")
            {
                Console.WriteLine("Operation aborted");
            }
            else
            {
                Console.WriteLine("Lecture updated successfully!");
                dbContext.SaveChanges();
            }
        }

        private void ProcessLectureAction(Lecture foundLecture, string lectureActionChoice, UniContext dbContext)
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

                case "ABORT":
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private static void UpdateLectureName(Lecture foundLecture)
        {
            Console.WriteLine("input a new name for the lecture");
            string lectureNewInputName = Console.ReadLine();
            foundLecture.LectureName = lectureNewInputName;
        }

        private void UpdateLectureWorker(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Select a new worker for the lecture");
            string workerOfLectureNameToChange = Console.ReadLine();
            Worker foundWorker = dbContext.Workers.FirstOrDefault(w =>
                w.WorkerFirstName + " " + w.WorkerLastName == workerOfLectureNameToChange);

            if (foundWorker != null)
            {
                foundLecture.LectureWorker = foundWorker;
            }
            else
            {
                Console.WriteLine($"Worker with name '{workerOfLectureNameToChange}' not found.");
            }
        }

        private void UpdateLectureFaculty(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Current Faculties: " + string.Join(", ", foundLecture.LectureFaculties.Select(f => f.FacultyName)));
            Console.WriteLine("Options: [1] Add faculty, [2] Update faculty, [3] Cancel");
            string facultyOption = Console.ReadLine();

            switch (facultyOption)
            {
                case "1":
                    AddExistingFacultyToLecture(foundLecture, dbContext);
                    break;

                case "2":
                    UpdateExistingFacultyInLecture(foundLecture, dbContext);
                    break;

                case "3":
                    // Cancel the faculty update
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private void AddExistingFacultyToLecture(Lecture foundLecture, UniContext dbContext)
        {
            Console.WriteLine("Enter the name of the existing faculty to add");
            string facultyNameToAdd = Console.ReadLine();
            Faculty facultyToAdd = dbContext.Faculties.FirstOrDefault(f =>
                f.FacultyName == facultyNameToAdd);

            if (facultyToAdd != null && !foundLecture.LectureFaculties.Contains(facultyToAdd))
            {
                foundLecture.LectureFaculties.Add(facultyToAdd);
            }
            else if (facultyToAdd == null) { Console.WriteLine($"Faculty with name '{facultyNameToAdd}' not found."); }
            else { Console.WriteLine("Faculty is already assigned to the lecture."); }
        }

        private void UpdateExistingFacultyInLecture(Lecture foundLecture, UniContext dbContext)
        {
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
        }

        private void UpdateLectureStudents(Lecture foundLecture, UniContext dbContext)
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
                    break;
            }
        }

        private void AddExistingStudentToLecture(Lecture foundLecture, UniContext dbContext)
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
        }

        private void RemoveExistingStudentFromLecture(Lecture foundLecture, UniContext dbContext)
        {
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
        }

        private static void DisplayLectureDetails(Lecture lecture)
        {
            string facultyNames = string.Join(", ", lecture.LectureFaculties.Select(f => f.FacultyName));
            string studentNames = string.Join(", ", lecture.LectureStudents.Select(s => s.StudentFirstName + " " + s.StudentLastName));

            Console.WriteLine(" =========== Lecture found! ===========");
            Console.WriteLine($"{lecture.Id}: {lecture.LectureName} - {lecture.LectureWorker} @ {facultyNames}");
            Console.WriteLine(" =========== Students of the class:");
            Console.WriteLine(studentNames);
        }

        // update lectures (add/update/remove)

        #endregion




        #region UpdateStudent

        public void UpdateStudentData()
        {
            var dbContext = new UniContext();
            string confirmUpdate = "n";

            while (confirmUpdate == "n" || confirmUpdate == "abort")
            {
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

                        ConfirmAndUpdateStudent(confirmUpdate);
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
            Console.WriteLine(" =========== Student found! ===========");
            Console.WriteLine($"{student.Id}: {student.StudentFirstName} {student.StudentLastName} @ {student.StudentFaculty.FacultyName} || YEAR: {student.StudentYear} || EMAIL: {student.StudentEmail}");

        }

        private void ProcessStudentAction(Student foundStudent, string studentActionChoice)
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
            Console.WriteLine("Input a new first name for the student");
            string studentNewInputFirstName = Console.ReadLine();
            foundStudent.StudentFirstName = studentNewInputFirstName;
        }

        private static void UpdateStudentLastName(Student foundStudent)
        {
            Console.WriteLine("Input a new last name for the student");
            string studentNewInputLastName = Console.ReadLine();
            foundStudent.StudentLastName = studentNewInputLastName;
        }

        private void UpdateStudentFaculty(Student foundStudent)
        {
            var dbContext = new UniContext();
            Console.WriteLine("Enter the name of the new faculty for the student");
            string facultyNameToUpdate = Console.ReadLine();
            Faculty facultyToUpdate = dbContext.Faculties.FirstOrDefault(f => f.FacultyName == facultyNameToUpdate);

            if (facultyToUpdate != null)
            {
                foundStudent.StudentFaculty = facultyToUpdate;
            }
            else { Console.WriteLine($"Faculty with name '{facultyNameToUpdate}' not found."); }
        }

        private static void ConfirmAndUpdateStudent(string confirmUpdate)
        {
            var dbContext = new UniContext();
            Console.WriteLine("Confirm Student updates? (y/n)");
            Console.WriteLine(@"Type {abort} to quit");
            confirmUpdate = Console.ReadLine().ToLower();

            if (confirmUpdate == "abort")
            {
                Console.WriteLine("Operation aborted");
            }
            else
            {
                Console.WriteLine("Student updated successfully!");
                dbContext.SaveChanges();
            }
        }

        //update student lectures


        #endregion

        //adds lectures(s) to student
        #endregion

        #region Change


        //move student to another faculty (update)

        //remove student from faculty

        #endregion


        // ----------------- DATABASE METHODS -----------------
        //method that shows menu



        // OBSOLETE
        public void UpdateLectureBACKUP()
        {
            var dbContext = new UniContext();
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
            var dbContext = new UniContext();
            List<string> oneStudentData = dbContext.Students
                .Select(s =>
                $"{s.Id}: {s.StudentFirstName} {s.StudentLastName}  || FACULTY: {s.StudentFaculty} || YEAR: {s.StudentYear} || EMAIL: {s.StudentEmail}") //sitoj vietoje dar reikia parodyti kokias lectures eina
                .ToList();
        }

    }
}
