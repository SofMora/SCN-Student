using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;

namespace ProyectoStudent.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsDAO _newsDAO;

        public NewsController(IConfiguration configuration)
        {
            _newsDAO = new NewsDAO(configuration); 

        }


        public async Task<ActionResult<List<News>>> GetAllNews()
        {
            try
            {
                var newsList = await _newsDAO.GetAllNewsAsync();
                if (newsList == null || newsList.Count == 0)
                {
                    return NotFound("No se encontraron noticias.");
                }
                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
