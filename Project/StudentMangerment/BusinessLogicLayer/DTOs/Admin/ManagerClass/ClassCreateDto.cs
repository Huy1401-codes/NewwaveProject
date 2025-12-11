using BusinessLogicLayer.Enums.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.Admin.ManagerClass
{
    public class ClassCreateDto
    {
        [Required(ErrorMessage ="Không bỏ trống")]
        public string ClassName { get; set; }
        public ClassStatus IsStatus { get; set; } 

        [Required(ErrorMessage = "Không bỏ trống")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Không bỏ trống")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "Không bỏ trống")]
        public int TeacherId { get; set; }

        public List<int> StudentIds { get; set; } = new();
    }

}
