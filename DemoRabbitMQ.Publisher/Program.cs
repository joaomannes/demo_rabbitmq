using RabbitMQ.Client;
using System;
using System.Text;

namespace DemoRabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                int opcao = 0;
                do
                {
                    Random random = new Random();
                    Console.WriteLine("Digite o número de mensagens para enviar:");
                    int vezes = int.Parse(Console.ReadLine());
                    for (int i = 0; i < vezes; i++)
                    {
                        Publish(connection, random.Next(1, 10000).ToString());
                    }
                    Console.WriteLine("Continuar? (1 - Sim / 0 - Não)");
                    opcao = int.Parse(Console.ReadLine());
                } while (opcao != 0);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void Publish(IConnection connection, string message)
        {

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "demo_rabbitmq",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "demo_rabbitmq",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);

            }
        }
    }
}
