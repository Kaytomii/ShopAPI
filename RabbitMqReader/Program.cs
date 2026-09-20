using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMqReader;

sealed class User
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

internal class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<User>(json);

            Console.WriteLine($"Email: {message.Email}");
            Console.WriteLine($"Password: {message.Password}");
        };

        await channel.BasicConsumeAsync(
            queue: "Users",
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("Waiting messages...");
        Console.ReadLine();
    }
}
