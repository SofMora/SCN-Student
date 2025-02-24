using System.Text.Json.Serialization;

namespace ProyectoStudent.Models
{
    public class Student
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lastName")]

        public string LastName { get; set; }

        [JsonPropertyName("email")]

        public string Email { get; set; }

        [JsonPropertyName("userName")]

        public string UserName { get; set; }

        [JsonPropertyName("password")] 
        public string Password { get; set; }
        public byte?[] Photo { get; set; } // Para almacenar imágenes

        [JsonPropertyName("SocialLinks")]

        public string? SocialLinks { get; set; }

        [JsonPropertyName("statusStudent")]

        public bool? StatusStudent { get; set; } // true = activo, false = inactivo
    }
}
