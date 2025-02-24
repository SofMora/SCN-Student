using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;

namespace ProyectoStudent.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ProfessorDao _professorDao;

        public ProfessorController(IConfiguration configuration)
        {
            _professorDao = new ProfessorDao(configuration); 
        }

        
        public IActionResult GetAll()
        {
            try
            {
                List<Professor> professors = _professorDao.GetAll();
                return Ok(professors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los profesores: {ex.Message}");
            }
        }

        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] Professor professor)
        {
            try
            {
                int result = _professorDao.Insert(professor);
                return result > 0
                    ? Ok("Profesor agregado correctamente")
                    : BadRequest("Error al insertar el profesor");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar el profesor: {ex.Message}");
            }
        }
    }
}
