using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000); // wait one second and get message
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "EnesAys",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    User user = JsonConvert.DeserializeObject<User>(message);
                    Console.WriteLine($" Adı: {user.Name} Mesaj [{user.Message}]");
                };
                channel.BasicConsume(queue: "EnesAys",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Mesaj kuyruktan alındı. Dashboarda bakın.");
                Console.ReadLine();
            }
        }
    }
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
