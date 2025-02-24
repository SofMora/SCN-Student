using ProyectoStudent.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoStudent.DAO
{
    public class ConsultDao
    {
        private readonly string _connectionString;

        public ConsultDao(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppointmentConection");
        }

        public string InsertConsult(Consult consult)
        {
            string errorMessage = string.Empty;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("scn_appointments.spInsertConsult", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DescriptionConsult", consult.DescriptionConsult);
                    command.Parameters.AddWithValue("@TypeConsult", consult.TypeConsult);
                    command.Parameters.AddWithValue("@Author", consult.Author);
                    command.Parameters.AddWithValue("@DateConsult", consult.DateConsult);
                    command.Parameters.AddWithValue("@StatusConsult", consult.StatusConsult);
                    command.Parameters.AddWithValue("@IdCourse", consult.IdCourse);

                    var outputErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputErrorMessage);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        errorMessage = outputErrorMessage.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "An error occurred: " + ex.Message;
                    }
                }
            }

            return errorMessage;
        }

        public List<Consult> GetActiveConsults()
        {
            List<Consult> consults = new List<Consult>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("scn_appointments.spGetActiveConsults", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Consult consult = new Consult
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        IdCourse = Convert.ToInt32(reader["IdCourse"]),
                                        DescriptionConsult = reader["DescriptionConsult"].ToString(),
                                        TypeConsult = Convert.ToBoolean(reader["TypeConsult"]),
                                        Author = Convert.ToInt32(reader["Author"]),
                                        DateConsult = Convert.ToDateTime(reader["DateConsult"]),
                                        StatusConsult = Convert.ToBoolean(reader["StatusConsult"])
                                    };

                                    if ((bool)consult.StatusConsult)
                                    {
                                        consults.Add(consult);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ocurrió un error al recuperar las consultas activas: " + ex.Message);
                    }
                }
            }

            return consults;
        }

    }
}
