using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class User : BasePersonEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public Student Student { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
       = new List<UserRole>(); 

    }


}
