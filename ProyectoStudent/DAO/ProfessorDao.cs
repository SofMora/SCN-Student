using Microsoft.Extensions.Configuration;
using ProyectoStudent.Models;
using System.Data.SqlClient;

namespace ProyectoStudent.DAO
{
    public class ProfessorDao
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public ProfessorDao(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("ProfesorConnection");
        }

        public List<Professor> GetAll()
        {
            var professors = new List<Professor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM [scn_professor].[Professors]", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            professors.Add(new Professor
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                UserName = reader.GetString(4),
                                Password = reader.GetString(5),
                                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                               // Photo = reader.IsDBNull(7) ? null : (byte[])reader["Photo"],
                                SocialLink = reader.IsDBNull(8) ? null : reader.GetString(8),
                                StatusProfessor = reader.IsDBNull(9) ? (bool?)null : reader.GetBoolean(9)
                            });
                        }
                    }
                }
            }

            return professors;
        }

        public async Task<Professor> GetProfessorByIdAsync(int id)
        {
            Professor professor = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM [scn_professor].[Professors] WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            professor = new Professor
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                UserName = reader.GetString(4),
                                Password = reader.GetString(5),
                                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                                // Photo = reader.IsDBNull(7) ? null : (byte[])reader["Photo"],
                                SocialLink = reader.IsDBNull(8) ? null : reader.GetString(8),
                                StatusProfessor = reader.IsDBNull(9) ? (bool?)null : reader.GetBoolean(9)
                            };
                        }
                    }
                }
            }

            return professor; // Si no se encuentra el profesor, se retorna null.
        }



        public int Insert(Professor professor)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO [scn_professor].[Professors] (Name, LastName, Email, UserName, Password, Description, SocialLink, StatusProfessor) " +
                                                           "VALUES (@Name, @LastName, @Email, @UserName, @Password, @Description, @SocialLink, @StatusProfessor)", connection))
                {
                    command.Parameters.AddWithValue("@Name", professor.Name);
                    command.Parameters.AddWithValue("@LastName", professor.LastName);
                    command.Parameters.AddWithValue("@Email", professor.Email);
                    command.Parameters.AddWithValue("@UserName", professor.UserName);
                    command.Parameters.AddWithValue("@Password", professor.Password);
                    command.Parameters.AddWithValue("@Description", (object)professor.Description ?? DBNull.Value);
                 //   command.Parameters.AddWithValue("@Photo", (object)professor.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SocialLink", (object)professor.SocialLink ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusProfessor", (object)professor.StatusProfessor ?? DBNull.Value);

                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
