using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;

namespace Heldy.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Registration(Person user);
        Task<LoginResult> Login(Person user);
        Task<string> RegisterStudent(string email);
    }
}
