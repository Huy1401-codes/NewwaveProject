using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs.ManagerStudent
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Bạn phải chọn User")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "StudentCode không được để trống")]
        [StringLength(20)]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string Gender { get; set; }
    }


}
