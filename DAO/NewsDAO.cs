using ProyectoStudent.Models;
using System.Data.SqlClient;

namespace ProyectoStudent.DAO
{
    public class NewsDAO
    {
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public NewsDAO(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AdminConnection");
        }

        public async Task<List<News>> GetAllNewsAsync()
        {
            List<News> newsList = new List<News>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT n.Id, n.Title, n.Author, n.TextNews, n.DateNews, tn.Name AS TypeNews 
                    FROM scn_admin.News n
                    INNER JOIN scn_admin.TypeNews tn ON n.TypeNews = tn.Id
                    ORDER BY n.DateNews DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    newsList.Add(new News
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetInt32(2),
                        TextNews = reader.GetString(3),
                        DateNews = reader.GetDateTime(4),
                    
                        TypeNews = reader.GetString(5)
                    });
                }
            }

            return newsList;
        }


    }
}
