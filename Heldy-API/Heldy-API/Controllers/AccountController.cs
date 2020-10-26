﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Heldy.Models;
using Microsoft.AspNetCore.Mvc;
using Heldy.Services.Interfaces;
using Heldy_API.Models;
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

            switch (loginResult)
            {
                case LoginResult.WrongLoginOrPassword:
                    return this.BadRequest("Wrong login or password.");
                case LoginResult.UserDoesNotExists:
                    return this.BadRequest("User does not exists.");
                case LoginResult.Ok:
                    return this.GetLoginResponse(user);
                default: return this.BadRequest();
            }
        }

        private IActionResult GetLoginResponse(Person user)
        {
            var accessToken = this.GenerateAccessToken(user);
            var expiresTime = int.Parse(this.configuration["AccessToken:ExpiresTimeInMinutes"]);
            var response = new
            {
                accessToken,
                ExpiresTime = DateTime.Now.AddMinutes(expiresTime)
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
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["AccessToken:SecretKey"]));
            var signCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiresTime = int.Parse(this.configuration["AccessToken:ExpiresTimeInMinutes"]);

            var tokenOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresTime),
                signingCredentials: signCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}