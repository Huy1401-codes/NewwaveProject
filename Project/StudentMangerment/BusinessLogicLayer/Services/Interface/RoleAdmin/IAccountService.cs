using BusinessLogicLayer.DTOs.Results;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interface.RoleAdmin
{
    public interface IAccountService
    {
        Task<LoginResult> LoginAsync(string email, string password);
    }
}
