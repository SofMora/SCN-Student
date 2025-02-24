using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;

namespace ProyectoStudent.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseDAO _courseDAO;

        public CourseController(CourseDAO courseDAO)
        {
            _courseDAO = courseDAO;
        }

      
        public async Task<ActionResult<List<Course>>> GetAllCourses()
        {
            try
            {
                var courses = await _courseDAO.GetAllCoursesAsync();
                if (courses == null || courses.Count == 0)
                {
                    return NotFound("No se encontraron cursos.");
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hubo un error al obtener los cursos: " + ex.Message);
            }
        }

    }
}
