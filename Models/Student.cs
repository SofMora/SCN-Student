namespace ProyectoStudent.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte?[] Photo { get; set; } // Para almacenar imágenes
        public string? SocialLinks { get; set; }
        public bool? StatusStudent { get; set; } // true = activo, false = inactivo
    }
}
