using DataAccessLayer.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Semester : BaseNameEntity
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<ClassSemester> ClassSemesters { get; set; } = new List<ClassSemester>();
    }

}
