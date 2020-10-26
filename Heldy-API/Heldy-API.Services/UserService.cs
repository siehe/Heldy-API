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

            await this.userRepository.CreateNewPerson(user);
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

        private string HashPassword(string password)
        { 
            var data = Encoding.ASCII.GetBytes(password);
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(data);
            var hashedPassword = Encoding.ASCII.GetString(sha256data,0,sha256data.Length);
            return hashedPassword;

        }
    }
}