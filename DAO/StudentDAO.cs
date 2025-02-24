using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.FileProviders.Physical;
using ProyectoStudent.Models;


namespace ProyectoStudent.DAO
{
    public class StudentDAO
    {
        private readonly IConfiguration _configuration;
        string connectionString;

        public StudentDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // Insertar un nuevo estudiante
        public int Insert(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[scn_student].[spInsertStudent]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@Name", student.Name);
                        command.Parameters.AddWithValue("@LastName", student.LastName);
                        command.Parameters.AddWithValue("@Email", student.Email);
                        command.Parameters.AddWithValue("@UserName", student.UserName);
                        command.Parameters.AddWithValue("@Password", student.Password);
                        command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = student.Photo ?? (object)DBNull.Value;
                        command.Parameters.AddWithValue("@SocialLinks", (object)student.SocialLinks ?? DBNull.Value);
                        command.Parameters.AddWithValue("@StatusStudent", (object)student.StatusStudent ?? DBNull.Value);

                        // Parámetro de salida
                        SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(errorMessageParam);

                        // Ejecutar el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Validar mensaje de error devuelto por el SP
                        string errorMessage = errorMessageParam.Value == DBNull.Value ? "Success" : errorMessageParam.Value.ToString();
                        Console.WriteLine($" Mensaje de salida del SP: {errorMessage}");

                        if (errorMessage == "Success")
                        { 
                            return 1; //  Devuelve 1 cuando la inserción es exitosa
                        }
                        else
                        {
                            Console.WriteLine($"❌ Error en SP: {errorMessage}");
                            throw new Exception(errorMessage);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(" SQL Error: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Error general: " + ex.Message);
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }


        // Eliminar un estudiante por email
        public int Delete(string email)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DeleteStudent", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", email);
                    result = command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException)
                {
                    throw;
                }
            }

            return result;
        }

        // Actualizar un estudiante
        public int Update(Student student)
        {
            int resultToReturn = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateStudent", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", student.Id);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@UserName", student.UserName);
                    command.Parameters.AddWithValue("@Password", student.Password);
                    //    command.Parameters.AddWithValue("@Photo", (object)student.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SocialLinks", (object)student.SocialLinks ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusStudent", student.StatusStudent);

                    resultToReturn = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultToReturn;
        }

        // Obtener un estudiante por email
        public Student Get(string email)
        {
            Student student = new Student();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetStudentByEmail", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    student.Id = reader.GetInt32(0);
                    student.Name = reader.GetString(1);
                    student.LastName = reader.GetString(2);
                    student.Email = reader.GetString(3);
                    student.UserName = reader.GetString(4);
                    student.Password = reader.GetString(5);
                    //    student.Photo? = reader["Photo"] as byte[];
                    student.SocialLinks = reader["SocialLinks"].ToString();
                    student.StatusStudent = reader.GetBoolean(7);
                }

                connection.Close();
            }

            return student;
        }

        // Obtener todos los estudiantes
        public List<Student> Get()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllStudents", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        UserName = reader["UserName"].ToString(),
                        StatusStudent = Convert.ToBoolean(reader["StatusStudent"])
                    });
                }

                connection.Close();
            }

            return students;
        }

        public Student? GetByCredentials(string username, string password)
        {
            Console.WriteLine($" Buscando usuario: {username}, Contraseña: {password}");

            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var command = new SqlCommand("sp_GetStudentByCredentials", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine(" Usuario encontrado en la BD");
                return new Student
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    Password = reader["Password"].ToString(),
                    Photo = null,
                    SocialLinks = reader["SocialLinks"]?.ToString(),
                    StatusStudent = reader["StatusStudent"] as bool?
                };
            }
            else
            {
                Console.WriteLine("❌ Usuario no encontrado o contraseña incorrecta.");
                return null;
            }
        }
    }
}

