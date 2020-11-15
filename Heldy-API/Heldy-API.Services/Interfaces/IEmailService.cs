using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendStudentRegistrationEmailAsync(string email, string password);

        Task NotifyStudentAboutBigAmountOfTasksAsync(int userId);
    }
}