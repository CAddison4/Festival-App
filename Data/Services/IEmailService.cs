using TeamRedInternalProject.Models;
using SendGrid;
using System.Threading.Tasks;


namespace TeamRedInternalProject.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }

}
