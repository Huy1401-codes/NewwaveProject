using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CreateSubjectDto
    {
        [Required(ErrorMessage = "Khong bo trong")]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 10, ErrorMessage = "Tín chỉ phải từ 1 đến 10")]
        public int Credit { get; set; }

        public bool? IsStatus { get; set; } = true;

        // Danh sách đầu điểm động
        public List<CreateGradeComponentDto> GradeComponents { get; set; } = new();
    }
}
