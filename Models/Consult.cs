namespace ProyectoStudent.Models
{
    public class Consult
    {
        
            public int? Id { get; set; }
        public int IdCourse { get; set; }
        public string? DescriptionConsult { get; set; }
        public bool? TypeConsult { get; set; }
        public int? Author { get; set; }
        public DateTime? DateConsult { get; set; }
        public bool? StatusConsult { get; set; }
    
    }
}
