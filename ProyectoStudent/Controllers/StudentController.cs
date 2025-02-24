using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;
using ProyectoStudent.Utils;
using System.Text;

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
                    Console.WriteLine("❌ Error: Estudiante no válido (null)");
                    return BadRequest("Estudiante no válido.");
                }

                // 📌 Imprimir los datos recibidos en la consola del servidor
                Console.WriteLine(" Datos recibidos:");
                Console.WriteLine($"   Name: {student.Name}");
                Console.WriteLine($"   LastName: {student.LastName}");
                Console.WriteLine($"   Email: {student.Email}");
                Console.WriteLine($"   UserName: {student.UserName}");
                Console.WriteLine($"   Password: {(string.IsNullOrEmpty(student.Password) ? " VACÍO O NULL" : student.Password)}");
                Console.WriteLine($"   Photo: {(student.Photo != null ? "✅ Imagen presente" : " No hay imagen")}");
                Console.WriteLine($"   SocialLinks: {student.SocialLinks}");
                Console.WriteLine($"   StatusStudent: {student.StatusStudent}");

                var result = _studentDao.Insert(student);

                if (result > 0)
                {
                    Console.WriteLine(" Estudiante insertado con éxito.");
                    return CreatedAtAction(nameof(GetByEmail), new { email = student.Email }, student);
                }
                else
                {
                    Console.WriteLine(" Error: No se pudo insertar el estudiante.");
                    return StatusCode(500, "No se pudo insertar el estudiante.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al crear el estudiante: {ex.Message}");
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

        public IActionResult Login([FromBody] Student student)
        {
            try
            {
                if (string.IsNullOrEmpty(student.UserName) || string.IsNullOrEmpty(student.Password))
                {
                    return BadRequest("Usuario y contraseña son requeridos.");
                }

                var result = _studentDao.GetByCredentials(student.UserName, student.Password);

                if (result != null)
                {
                    return Ok(result); 
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
