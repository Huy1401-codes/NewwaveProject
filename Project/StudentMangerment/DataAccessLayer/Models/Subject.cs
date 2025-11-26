using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Range(1, 10)]
        public int Credit { get; set; }

        public bool? IsStatus { get; set; }

    }
}
