using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CreateGradeComponentDto
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage="Khong bo trong")]
        [StringLength(100)]
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Hệ số (0 - 1)
        /// </summary>
        /// 
       [Required(ErrorMessage = "Khong bo trong")]
        public double Weight { get; set; }

        public bool IsDelete { get; set; } = true;
    }

}
