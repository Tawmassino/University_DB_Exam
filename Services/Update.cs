using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_DB_Exam;
using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam.Models;

namespace Student_Platform_DB_Exam.Services
{
    public class Update
    {

        //----------

        // -------- update --------
        #region UpdateFaculty

        public static void UpdateFacultyData()
        {
            try
            {
                using var dbContext = new UniContext();

                Show.AllFaculties();

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
                    InputKeyToContinue();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }


        private static void DisplayFacultyDetails(Faculty faculty)
        {
            try
            {
                Console.WriteLine(" =========== Faculty found! ===========");
                Console.WriteLine($"{faculty.FacultyName}: Dean - {faculty.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void ProcessFacultyUpdate(Faculty foundFaculty)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public static void UpdateFacultyName(Faculty foundFaculty)
        {
            try
            {
                Console.WriteLine("Input a new name for the faculty");
                string facultyNewInputName = Console.ReadLine();
                foundFaculty.FacultyName = facultyNewInputName;
                Console.WriteLine($" New Faculty name is: {facultyNewInputName}");
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public static void UpdateFacultyDean(Faculty foundFaculty)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void ConfirmAndUpdateFaculty(Faculty foundFaculty)
        {
            try
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
                    // Update faculty
                    Console.WriteLine("Faculty updated successfully!");
                    dbContext.SaveChanges();
                    InputKeyToContinue();
                    return;  // Exit the loop
                }
                else
                {
                    Console.WriteLine("Invalid input. Operation aborted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void ConfirmAndRemoveFaculty(Faculty foundFaculty)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        #endregion

        //----------

        #region UpdateLecture


        public static void UpdateLecture()
        {
            try
            {
                using var dbContext = new UniContext();

                Console.WriteLine();
                Show.AllLectures();

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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle or log the exception as needed
            }
        }

        //OBSOLETE, since there's no more loop for updating
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

        public static void UpdateLectureName(Lecture foundLecture)
        {
            try
            {
                Console.WriteLine("input a new name for the lecture");
                string lectureNewInputName = Console.ReadLine();
                foundLecture.LectureName = lectureNewInputName;
                Console.WriteLine($"New Lecture Name: {lectureNewInputName}");
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void UpdateLectureWorker(Lecture foundLecture, UniContext dbContext)
        {
            try
            {
                Console.WriteLine("Enter the ID of the new worker for the lecture");
                string workerIdToChange = Console.ReadLine();

                if (Guid.TryParse(workerIdToChange, out Guid parsedWorkerIdToChange))
                {
                    Worker foundWorker = dbContext.Workers
                        .FirstOrDefault(w => w.Id == parsedWorkerIdToChange);

                    if (foundWorker != null)
                    {
                        foundLecture.LectureWorker = foundWorker;
                        Console.WriteLine($"The Lecturer for {foundLecture.LectureName} is now {foundWorker.WorkerFirstName} {foundWorker.WorkerLastName}");
                        dbContext.SaveChanges();
                        InputKeyToContinue();
                    }
                    else
                    {
                        Console.WriteLine($"Worker with ID '{workerIdToChange}' not found.");
                        InputKeyToContinue();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid worker ID format.");
                    InputKeyToContinue();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
            try
            {
                Console.WriteLine("Enter the name of the existing faculty to add");
                string facultyNameToAdd = Console.ReadLine();
                Faculty facultyToAdd = dbContext.Faculties.FirstOrDefault(f =>
                    f.FacultyName == facultyNameToAdd);

                if (facultyToAdd != null && !foundLecture.LectureFaculties.Contains(facultyToAdd))
                {
                    foundLecture.LectureFaculties.Add(facultyToAdd);
                }
                else if (facultyToAdd == null)
                {
                    Console.WriteLine($"Faculty with name '{facultyNameToAdd}' not found.");
                    InputKeyToContinue();
                }
                else
                {
                    Console.WriteLine("Faculty is already assigned to the lecture.");
                    InputKeyToContinue();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        private static void RemoveFacultyFromLecture(Lecture foundLecture, UniContext dbContext)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void UpdateLectureStudents(Lecture foundLecture, UniContext dbContext)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void AddExistingStudentToLecture(Lecture foundLecture, UniContext dbContext)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void RemoveExistingStudentFromLecture(Lecture foundLecture, UniContext dbContext)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Handle or log the exception as needed
            }
        }



        public static void RemoveLecture()
        {
            try
            {
                using var dbContext = new UniContext();

                Show.AllLectures();

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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private static void DisplayLectureDetails(Guid lectureId)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        #endregion

        //----------

        #region UpdateStudent

        public static void UpdateStudentData()
        {
            try
            {
                using var dbContext = new UniContext();
                string confirmUpdate = "n";

                while (confirmUpdate == "n" || confirmUpdate == "abort")
                {
                    Show.AllStudents();
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                InputKeyToContinue();
            }
        }

        private static void DisplayStudentDetails(Student student)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying student details: {ex.Message}");
                InputKeyToContinue();
            }
        }



        private static void ProcessStudentAction(Student foundStudent, string studentActionChoice)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing student action: {ex.Message}");
                InputKeyToContinue();
            }
        }


        private static void UpdateStudentFirstName(Student foundStudent)
        {
            try
            {
                using var dbContext = new UniContext();
                Console.WriteLine("Input a new first name for the student");
                string studentNewInputFirstName = Console.ReadLine();
                foundStudent.StudentFirstName = studentNewInputFirstName;
                Console.WriteLine($"Student's First Name has been changed to {studentNewInputFirstName}");
                dbContext.SaveChanges();
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating student's first name: {ex.Message}");
                InputKeyToContinue();
            }
        }


        private static void UpdateStudentLastName(Student foundStudent)
        {
            try
            {
                using var dbContext = new UniContext();
                Console.WriteLine("Input a new last name for the student");
                string studentNewInputLastName = Console.ReadLine();
                foundStudent.StudentLastName = studentNewInputLastName;
                Console.WriteLine($"Student's Last Name has been changed to {studentNewInputLastName}");
                dbContext.SaveChanges();
                InputKeyToContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating student's last name: {ex.Message}");
                InputKeyToContinue();
            }
        }


        private static void UpdateStudentFaculty(Student foundStudent)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating student's faculty: {ex.Message}");
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

                try
                {
                    Console.WriteLine("Student updated successfully!");
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                }

            }
            else
            {
                Console.WriteLine("Invalid input. Operation aborted");
            }
        }

        #endregion

        public static void InputKeyToContinue()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

    }
}
