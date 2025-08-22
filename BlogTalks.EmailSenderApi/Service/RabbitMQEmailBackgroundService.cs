using BlogTalks.EmailSenderApi.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace BlogTalks.EmailSenderApi.Service
{
    public class RabbitMQEmailBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly BlogTalks.EmailSenderApi.DTO.RabbitMQSettingseEmailSender _rabbitSettings;
        private readonly ILogger<RabbitMQEmailBackgroundService> _logger;
        private IConnection _connection;
        private IChannel _channel;
        private readonly IEmailSender _emailSender;

        public RabbitMQEmailBackgroundService(IServiceScopeFactory serviceScopeFactory, IOptions<BlogTalks.EmailSenderApi.DTO.RabbitMQSettingseEmailSender> rabbitSettings, ILogger<RabbitMQEmailBackgroundService> logger, IEmailSender emailSender)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitSettings = rabbitSettings.Value;
            _emailSender = emailSender;
            _logger = logger;
        }

        private async Task InitRabbitMQAsync(EmailDto emailDto)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitSettings.RabbitURL,
                UserName = _rabbitSettings.Username,
                Password = _rabbitSettings.Password
            };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(_rabbitSettings.ExchangeName, _rabbitSettings.ExchhangeType, durable: true);
            await channel.QueueDeclareAsync(_rabbitSettings.QueueName, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(_rabbitSettings.QueueName, _rabbitSettings.ExchangeName, _rabbitSettings.RouteKey);
            await _channel.BasicQosAsync(0, 1, false);

            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var emailDto = JsonConvert.DeserializeObject<EmailDto>(message);

                await _emailSender.SendAsync(emailDto);
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };
            consumer.ShutdownAsync += OnConsumerShutdown;
            consumer.RegisteredAsync += OnConsumerRegistered;
            consumer.UnregisteredAsync += OnConsumerUnregistered;

            await _channel.BasicConsumeAsync(_rabbitSettings.QueueName, autoAck: false, consumer);
        }
        public override void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
            base.Dispose();
        }

        private Task OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerUnregistered(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerRegistered(object sender, ConsumerEventArgs e) => Task.CompletedTask;
        private Task OnConsumerShutdown(object sender, ShutdownEventArgs e) => Task.CompletedTask;
        private Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) => Task.CompletedTask;
    }
}
