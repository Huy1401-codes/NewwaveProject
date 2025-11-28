using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public ICollection<ClassSemester> ClassSemesters { get; set; } = new List<ClassSemester>();

    }

}
