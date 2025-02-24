using Microsoft.Extensions.Configuration;
using ProyectoStudent.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProyectoStudent.DAO
{
    public class CommentDao
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public CommentDao(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AdminConnection");
        }

        public async Task InsertCommentAsync(Comment comment)
        {
            var errorMessage = string.Empty;
            var query = "[scn_admin].[Insert_Comment]";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdNews", SqlDbType.Int) { Value = comment.IdNews });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = comment.Description });
                    command.Parameters.Add(new SqlParameter("@Author", SqlDbType.Int) { Value = comment.Author });
                    command.Parameters.Add(new SqlParameter("@CommentDate", SqlDbType.DateTime) { Value = (object?)comment.CommentDate ?? DBNull.Value });

                    var errorParameter = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(errorParameter);

                    await command.ExecuteNonQueryAsync();

                    errorMessage = errorParameter.Value.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception($"Error inserting comment: {errorMessage}");
                    }
                }
            }
        }

        public async Task<List<Comment>> GetCommentsByNewsIdAsync(int newsId)
        {
            var comments = new List<Comment>();
            var errorMessage = string.Empty;
            var query = "[scn_admin].[Get_Comment]";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdNews", SqlDbType.Int) { Value = newsId });

                    var errorParameter = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(errorParameter);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comments.Add(new Comment
                            {
                                Id = reader.GetInt32(0),
                                IdNews = reader.GetInt32(1),
                                Description = reader.GetString(2),
                                Author = reader.GetInt32(3),
                                CommentDate = reader.GetDateTime(4)
                            });
                        }
                    }

                    errorMessage = errorParameter.Value.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception($"Error retrieving comments: {errorMessage}");
                    }
                }
            }
            return comments;
        }
    }
}
