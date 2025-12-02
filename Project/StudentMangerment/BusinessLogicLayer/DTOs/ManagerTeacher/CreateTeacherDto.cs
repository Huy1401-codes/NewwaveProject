using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.ManagerTeacher
{
    public class CreateTeacherDto
    {
        
            [Required(ErrorMessage = "Bạn phải chọn User")]
            public int? UserId { get; set; }

            [Required(ErrorMessage = "TeacherCode không được để trống")]
            [StringLength(20)]
            public string TeacherCode { get; set; }

            [Required(ErrorMessage = "Bằng cấp không được để trống")]
            public string Degree { get; set; }
        

    }
}
