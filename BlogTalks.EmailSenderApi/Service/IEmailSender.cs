using BlogTalks.EmailSenderApi.DTO;

namespace BlogTalks.EmailSenderApi.Service
{
    public interface IEmailSender
    {
        Task SendAsync(EmailDto request);
    }

}
