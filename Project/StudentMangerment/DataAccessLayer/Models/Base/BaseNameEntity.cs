using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Base
{
    public abstract class BaseNameEntity : BaseStatusEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
    }
}
