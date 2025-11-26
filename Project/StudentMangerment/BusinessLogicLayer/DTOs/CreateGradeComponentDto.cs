using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class CreateGradeComponentDto
    {
        public int SubjectId { get; set; }

        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Hệ số (0 - 1)
        /// </summary>
        public double Weight { get; set; }
    }

}
