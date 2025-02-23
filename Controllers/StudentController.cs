using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;
using ProyectoStudent.Utils;

namespace ProyectoStudent.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDAO _studentDao;
        private readonly Encryption encryption;

        public StudentController(IConfiguration configuration)
        {
            _studentDao = new StudentDAO(configuration); // Instanciamos el DAO
            encryption = new Encryption();
        }

        public IActionResult GetAll()
        {
            try
            {
                var students = _studentDao.Get();
                return Ok(students); // Devolvemos la lista de estudiantes
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los estudiantes: {ex.Message}");
            }
        }


        public IActionResult GetByEmail(string email)
        {
            try
            {
                var student = _studentDao.Get(email);
                if (student.Id == 0) // Si no se encuentra el estudiante
                {
                    return NotFound("Estudiante no encontrado.");
                }

                return Ok(student); // Devuelve el estudiante encontrado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el estudiante: {ex.Message}");
            }
        }

      
        public IActionResult Create([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Estudiante no válido.");
                }

                var result = _studentDao.Insert(student);
                if (result > 0)
                {
                    return CreatedAtAction(nameof(GetByEmail), new { email = student.Email }, student);
                }
                else
                {
                    return StatusCode(500, "No se pudo insertar el estudiante.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el estudiante: {ex.Message}");
            }
        }


        public IActionResult Update([FromBody] Student student)
        {
            try
            {
                if (student == null || student.Id == 0)
                {
                    return BadRequest("Estudiante no válido.");
                }

                var result = _studentDao.Update(student);
                if (result > 0)
                {
                    return Ok(student); // Devuelve el estudiante actualizado
                }
                else
                {
                    return NotFound("Estudiante no encontrado para actualizar.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estudiante: {ex.Message}");
            }
        }


        public IActionResult Delete(string email)
        {
            try
            {
                var result = _studentDao.Delete(email);
                if (result > 0)
                {
                    return Ok("Estudiante eliminado.");
                }
                else
                {
                    return NotFound("Estudiante no encontrado para eliminar.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el estudiante: {ex.Message}");
            }
        }

        public IActionResult Login( string username, string pwd)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd))
                {
                    return BadRequest("Usuario y contraseña son requeridos.");
                }

                var result = _studentDao.GetByCredentials(username,encryption.Encrypt(pwd));

                if (result != null)
                {
                    return Ok(result); // Devuelve el estudiante autenticado
                }
                else
                {
                    return NotFound("Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el login: {ex.Message}");
            }
        }
    }
}
