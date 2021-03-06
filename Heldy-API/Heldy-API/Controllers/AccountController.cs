﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;
using Microsoft.AspNetCore.Mvc;
using Heldy.Services.Interfaces;
using Heldy_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid data.");
            }

            var user = new Person
            {
                Name = model.Name,
                SecondName = model.SecondName,
                Surname = model.Surname,
                DOB = model.DOB,
                Email = model.Email,
                Password = model.Password
            };

            if (await this.userService.Registration(user))
            {
                return this.Ok();
            }

            return this.BadRequest("User already exists.");

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid data.");
            }

            var user = new Person { Email = model.Email, Password = model.Password };
            var loginResult = await this.userService.Login(user);

            switch (loginResult.LoginResult)
            {
                case LoginResult.WrongLoginOrPassword:
                    return this.BadRequest("Wrong login or password.");
                case LoginResult.UserDoesNotExists:
                    return this.BadRequest("User does not exists.");
                case LoginResult.Ok:
                    return this.GetLoginResponse(loginResult.Person);
                default: return this.BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("registerStudent")]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid data.");
            }

            var password = await userService.RegisterStudent(model.Email);
            if (password == null)
            {
                return BadRequest("User already exists.");
            }

            var response = new
            {
                email = model.Email,
                password = password
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("updateProfileImage")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateProfileImage()
        {
            var file = Request.Form.Files[0];
            var personId = Int32.Parse(Request.Form["personId"]);
            var fileName = await SaveImage(file);
            await userService.UpdatePersonImage(personId, fileName);
            return Ok();
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
            var fileType = this.GetFileType(file.FileName);
            var filename = file.GetHashCode() + '.' + fileType;
            var path = "images/" + filename;
            await using (var fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return filename;
        }

        private string GetFileType(string str)
        {
            var split = str.Split(".");
            return split[split.Length - 1];
        }



        private IActionResult GetLoginResponse(Person user)
        {
            var accessToken = this.GenerateAccessToken(user);
            var expiresTime = int.Parse(this.configuration["AccessToken:ExpiresTimeInMinutes"]);
            var response = new
            {
                accessToken,
                ExpiresTime = DateTime.Now.AddMinutes(expiresTime),
                userId = user.Id,
                roleId = user.RoleId
            };
            return this.Ok(response);
        }

        private string GenerateAccessToken(Person user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString())
            };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AccessToken:SecretKey"]));
            var expiresTimeInMinutes = int.Parse(configuration["AccessToken:ExpiresTimeInMinutes"]);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: configuration["AccessToken:Issuer"],
                audience: configuration["AccessToken:Audience"],
                notBefore: now,
                claims: claims,
                expires: now.AddMinutes(expiresTimeInMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AccessToken:SecretKey"])), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}