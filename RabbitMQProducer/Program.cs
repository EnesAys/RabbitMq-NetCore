using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User() { Name = "Enes A.", Message = "Rabbit mq ilk giden mesaj" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "EnesAys",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(user);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "EnesAys",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Gönderilen kişi: {user.Name}");
            }

            Console.WriteLine(" Mesaj gönderildi");
            Console.ReadLine();
        }

    }
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
