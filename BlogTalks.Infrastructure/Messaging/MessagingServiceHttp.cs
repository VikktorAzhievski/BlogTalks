using BlogTalks.Application.Abstractions;
using BlogTalks.Application.Contracts;
using System.Net.Http.Json;

namespace BlogTalks.Infrastructure.Messaging
{
    public class MessagingServiceHttp : IMessagingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MessagingServiceHttp(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task Send(EmailDto email)
        {
            var client = _httpClientFactory.CreateClient("EmailSenderApi");
            //change
            var res = await client.PostAsJsonAsync("/send", email);
        }

    }

}

