namespace ProyectoStudent.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cycle { get; set; }
        public bool StatusCourse { get; set; }
        public string Description { get; set; }
        public int IdProfessor { get; set; }
    }
}
