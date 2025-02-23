namespace ProyectoStudent.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Author { get; set; }
        public string TextNews { get; set; }
        public DateTime DateNews { get; set; }
        public byte[] Images { get; set; }
        public string TypeNews { get; set; } // Tipo de noticia como string
    }
}
