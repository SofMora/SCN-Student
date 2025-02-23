using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;

namespace ProyectoStudent.Controllers
{
    public class ConsultController : Controller
    {
        private readonly ConsultDao _consultDao;
        private readonly ProfessorDao _professorDao;
        private readonly CourseDAO _courseDao;

        // Constructor to inject ConsultDao
        public ConsultController(ConsultDao consultDao,ProfessorDao professorDao, CourseDAO courseDAO)
        {
            _consultDao = consultDao;
            _professorDao = professorDao;
            _courseDao = courseDAO;
        }

     
       
        public IActionResult SubmitConsult([FromBody] Consult consult)
        {
            if (ModelState.IsValid)
            {
                Boolean flag=false;

                if (consult.TypeConsult == false)
                {
                    flag = true;
                }
                // Call the DAO method to insert the consultation into the database
                string result = _consultDao.InsertConsult(consult);

                SendMails mail = new SendMails();

                if (result == "Success")
                {

                    if (flag==true)
                    {
                        Task<Course> curso= _courseDao.GetCourseByIdAsync(consult.IdCourse);
                     Task<Professor> professor= _professorDao.GetProfessorByIdAsync(curso.Result.IdProfessor);
                        mail.sendMailsSmartParking("Un estudiante solicito la cita", professor.Result.Email, "Nueva solicitud");
                     //todo buscar corro profe
                     //mail.sendMailsSmartParking
                    }

                    // Redirect or return a success message
                    return Json(new { success = true, message = "Consultation submitted successfully!" });
                }
                else
                {
                    // Return the error message if insertion fails
                    return Json(new { success = false, message = result });
                }
            }

            // If model validation fails, return an error message
            return Json(new { success = false, message = "Invalid data. Please try again." });
        }


        public ActionResult<List<Consult>> GetActiveConsults()
        {
            try
            {
                var activeConsults = _consultDao.GetActiveConsults();

                if (activeConsults == null || !activeConsults.Any())
                {
                    return NotFound("No se encontraron consultas activas.");
                }

                return Ok(activeConsults);
            }
            catch (Exception ex)
            {
                // Captura de error en caso de excepciones
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }



    }
}
