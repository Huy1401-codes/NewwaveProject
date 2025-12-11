using BusinessLogicLayer.DTOs.ManagerTeacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Credit { get; set; }

        public bool? IsStatus { get; set; }

        // Optional: danh sách GradeComponents nếu muốn hiển thị chi tiết
        public List<GradeComponentDto> GradeComponents { get; set; } = new();
    }
}
