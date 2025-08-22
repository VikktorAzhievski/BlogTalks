using BlogTalks.Application.Contracts;

namespace BlogTalks.Application.Abstractions
{
    public interface IMessagingService
    {
        Task Send(EmailDto email);
    }
}
