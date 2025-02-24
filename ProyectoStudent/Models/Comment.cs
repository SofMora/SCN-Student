namespace ProyectoStudent.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int IdNews { get; set; }
        public string Description { get; set; }
        public int Author { get; set; }
        public DateTime CommentDate { get; set; }

    }
}
