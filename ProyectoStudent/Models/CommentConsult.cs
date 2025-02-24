namespace ProyectoStudent.Models
{
    public class CommentConsult
    {
       
            public int Id { get; set; }
            public int IdConsult { get; set; }
            public string DescriptionComment { get; set; }
            public int Author { get; set; }
           public DateTime DateComment { get; set; }
        
    }
}
