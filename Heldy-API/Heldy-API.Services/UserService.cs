using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Services.Interfaces;

namespace Heldy.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository userRepository;
        private const int PasswordLength = 16;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Registration(Person user)
        {
            var userFromDb = await this.userRepository.GetPersonByEmail(user.Email);
            if (userFromDb != null)
            {
                return false;
            }

            user.Password = HashPassword(user.Password);

            await this.userRepository.CreateNewTeacher(user);
            return true;
        }

        public async Task<LoginResult> Login(Person user)
        {
            var userFromDb = await this.userRepository.GetPersonByEmail(user.Email);
            if (userFromDb == null)
            {
                return LoginResult.UserDoesNotExists;
            }

            if (userFromDb.Password == HashPassword(user.Password))
            {
                return LoginResult.Ok;
            }

            return LoginResult.WrongLoginOrPassword;

        }

        public async Task<string> RegisterStudent(string email)
        {
            var userFromDb = await userRepository.GetPersonByEmail(email);

            if (userFromDb != null)
            {
                return null;
            }

            var password = GenerateRandomPassword(PasswordLength);
            var hashedPassword = HashPassword(password);
            var newUser = new Person
            {
                Email = email,
                Password = hashedPassword
            };

            await userRepository.CreateNewStudent(newUser);
            return password;
        }

        private string HashPassword(string password)
        { 
            var data = Encoding.ASCII.GetBytes(password);
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(data);
            var hashedPassword = Encoding.ASCII.GetString(sha256data,0,sha256data.Length);
            return hashedPassword;
        }

        private string GenerateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}