using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQConsumer
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			var factory = new ConnectionFactory()
			{
				HostName = "localhost",
				Port = 5672,
				UserName = "guest",
				Password = "guest"
			};


			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "infotechq",
					durable: false,
					exclusive: false,
					autoDelete: false,
					arguments: null);


				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					Product p1 = new Product();
					p1 = JsonConvert.DeserializeObject<Product>(message);
					Console.WriteLine("[x] Alınan mesaj {0}", p1.Name);
					Console.WriteLine("Alınan {0}", message);
				};
				
				channel.BasicConsume(queue: "infotechq",
					autoAck:true,
					consumer: consumer);

				
				Console.WriteLine("Çıkmak için bi tuşa basın");
				Console.ReadLine();




			}


			



		}
	}
}