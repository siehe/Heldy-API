using System;
using System.Collections.Generic;
using System.Text;
using Heldy.Models;

namespace Heldy.Services.DTO
{
    public class LoginResultDto
    {
        public LoginResult LoginResult { get; set; }

        public Person Person { get; set; }
    }
}
