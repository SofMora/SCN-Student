namespace ProyectoStudent.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public string? SocialLink { get; set; }
        public bool? StatusProfessor { get; set; } // true = activo, false = inactivo
    }
}
