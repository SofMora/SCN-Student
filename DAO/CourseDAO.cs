using ProyectoStudent.Models;
using System.Data.SqlClient;

namespace ProyectoStudent.DAO
{
    public class CourseDAO
    {


        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CourseDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProfesorConnection");
        }

        // Obtener todos los cursos
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            List<Course> courseList = new List<Course>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT c.Id, c.Name, c.Cycle, c.StatusCourse, c.Description, c.IdProfessor 
                    FROM scn_professor.Course c ";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    courseList.Add(new Course
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Cycle = reader.GetInt32(2),
                        StatusCourse = reader.GetBoolean(3),
                        Description = reader.GetString(4),
                        IdProfessor = reader.GetInt32(5),
                    });
                }
            }

            return courseList;
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT c.Id, c.Name, c.Cycle, c.StatusCourse, c.Description, c.IdProfessor 
            FROM scn_professor.Course c
            WHERE c.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Course
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Cycle = reader.GetInt32(2),
                        StatusCourse = reader.GetBoolean(3),
                        Description = reader.GetString(4),
                        IdProfessor = reader.GetInt32(5),
                    };
                }
            }

            return null; // Si no se encuentra el curso, se retorna null.
        }



    }
}
