using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Base
{
    public abstract class BaseStatusEntity : BaseEntity
    {
        public bool? IsStatus { get; set; } = true;
    }

}
