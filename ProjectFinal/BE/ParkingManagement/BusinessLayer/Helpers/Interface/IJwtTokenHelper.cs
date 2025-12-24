using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpers.Interface
{
    public interface IJwtTokenHelper
    {
        string GenerateAccessToken(User user);
    }
}
