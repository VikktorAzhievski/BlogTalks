using BlogTalks.Application.Contracts;
using BlogTalks.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using BlogTalks.Application.Abstractions;

public class MessagingServiceRabbitMQ : IMessagingService
{
    private readonly RabbitMqSettings _rabbitMQSettings;

    public MessagingServiceRabbitMQ(IOptions<RabbitMqSettings> rabbitMqOptions)
    {
        _rabbitMQSettings = rabbitMqOptions.Value;
    }
    public async Task Send(EmailDto emailDto)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMQSettings.Host,
            UserName = _rabbitMQSettings.Username,
            Password = _rabbitMQSettings.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(_rabbitMQSettings.ExchangeName, _rabbitMQSettings.ExchhangeType, durable: true);
        await channel.QueueDeclareAsync(_rabbitMQSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(_rabbitMQSettings.QueueName, _rabbitMQSettings.ExchangeName, _rabbitMQSettings.RouteKey);

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(emailDto));

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: _rabbitMQSettings.ExchangeName,
            routingKey: _rabbitMQSettings.RouteKey,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }
}