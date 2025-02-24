using Microsoft.AspNetCore.Mvc;
using ProyectoStudent.DAO;
using ProyectoStudent.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoStudent.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentDao _commentDAO;

        public CommentController(IConfiguration configuration)
        {
            _commentDAO = new CommentDao(configuration); // Instantiate the DAO
        }

        // GET: /Comment/GetByNewsId/{id}
        public async Task<ActionResult<List<Comment>>> GetCommentsByNewsId(int id)
        {
            try
            {
                // Get the list of comments for the specified news ID
                var comments = await _commentDAO.GetCommentsByNewsIdAsync(id);

                // Check if comments were found
                if (comments == null || comments.Count == 0)
                {
                    return NotFound("No se encontraron comentarios para esta noticia.");
                }

                // Return the comments
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Return an internal server error if an exception occurs
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        public async Task<ActionResult> InsertComment([FromBody] Comment comment)
        {
            if (comment == null || string.IsNullOrWhiteSpace(comment.Description))
            {
                return BadRequest("El comentario no puede estar vacío.");
            }

            try
            {
                comment.CommentDate= DateTime.Now;
           //     comment.Author = 1; // Se quema el usuario con ID 1
                await _commentDAO.InsertCommentAsync(comment);
                return Ok("Comentario agregado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }



}

