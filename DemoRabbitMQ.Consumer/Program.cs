using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DemoRabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Consume();
        }

        static void Consume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "demo_rabbitmq",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    string message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] Received {0}", message);


                    Console.WriteLine("Ack {0}", message);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    //if (ea.Redelivered)
                    //{
                    //    Console.WriteLine("Redelivery Ack {0}", message);
                    //    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    //}
                    //else
                    //{
                    //    if (Int32.Parse(message) % 2 == 0)
                    //    {
                    //        Console.WriteLine("Ack {0}", message);
                    //        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Reject Ack");
                    //        channel.BasicReject(deliveryTag: ea.DeliveryTag, true);
                    //    }
                    //}


                };
                channel.BasicConsume(queue: "demo_rabbitmq",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine("Aguardando Mensagens...");
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
