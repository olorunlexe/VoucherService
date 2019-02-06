using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace VoucherService.MQ
{
    public class GiftRedemption:BackgroundService
    {
        private IVoucherService baseVoucherService;
        private ILogger<GiftRedemption> _logger;

        public GiftRedemption(IVoucherService baseVoucherService, ILogger<GiftRedemption> _logger)
        {
            this.baseVoucherService = baseVoucherService;
            this._logger = _logger;
        }

        public void CodeReceiver()
        {

            var factory = new ConnectionFactory()
            {
                HostName = "192.168.99.100",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                Protocol = Protocols.DefaultProtocol,
                UserName = "guest",
                RequestedHeartbeat = 30,
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                

                //channel.ExchangeDeclare("gift-exchange", ExchangeType.Topic);
                channel.QueueDeclare("gift-one", true, false, false, null);
                channel.QueueDeclare("gift-two", true, false, false, null);
                channel.QueueDeclare("gift-three", true, false, false, null);
                channel.QueueBind("gift-two", "gift-exchange", "gift-two", null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received From VoucherApi {0}", message);
                    var deserialized = JsonConvert.DeserializeObject<Gift>(message);
                    Console.WriteLine("Received Gift Object{0} {1}", deserialized.Code, deserialized.GiftBalance);
                    //use the Gotten message(code) to check database for gift details
                    Task<Gift> gift = baseVoucherService.GetGiftVoucher(deserialized.Code);
                    Console.WriteLine(" Gift Object", gift.ToString());
                    //send back the Gift object to Redemption...
                    if (gift != null)
                    {
                        string result = JsonConvert.SerializeObject(gift.Result);
                        var resultSet = Encoding.UTF8.GetBytes(result);
                        publishToVoucher(resultSet);  
                    }

                };
                channel.BasicConsume(queue: "gift-one",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                

            }
        }

        public void publishToVoucher(byte[] resultSet)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.99.100",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                Protocol = Protocols.DefaultProtocol,
                UserName = "guest",
                RequestedHeartbeat = 30,
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                IBasicProperties props = channel.CreateBasicProperties();
                props.ContentType = "application/json";

                //channel.ExchangeDeclare("gift-exchange", ExchangeType.Topic);
                channel.QueueDeclare("gift-one", true, false, false, null);
                channel.QueueDeclare("gift-two", true, false, false, null);
                channel.QueueDeclare("gift-three", true, false, false, null);
                channel.QueueBind("gift-two", "gift-exchange", "gift-two", null);

                channel.BasicPublish(exchange: "gift-exchange",
                                             routingKey: "gift-two",
                                             basicProperties: props,
                                             body: resultSet);
                Console.WriteLine(" [x] Sent {0}", resultSet);
            }
        }

        //public void updateVoucher()
        //{
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "192.168.99.100",
        //        Port = AmqpTcpEndpoint.UseDefaultPort,
        //        Protocol = Protocols.DefaultProtocol,
        //        UserName = "guest",
        //        RequestedHeartbeat = 30,
        //        Password = "guest"
        //    };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        IBasicProperties props = channel.CreateBasicProperties();
        //        props.ContentType = "application/json";

        //        //channel.ExchangeDeclare("gift-exchange", ExchangeType.Topic);
        //        channel.QueueDeclare("gift-one", true, false, false, null);
        //        channel.QueueDeclare("gift-two", true, false, false, null);
        //        channel.QueueDeclare("gift-three", true, false, false, null);
        //        channel.QueueBind("gift-three", "gift-exchange", "gift-tree", null);

        //        var consumer = new EventingBasicConsumer(channel);
        //        consumer.Received += (model, ea) =>
        //        {
        //            var body = ea.Body;
        //            var message = Encoding.UTF8.GetString(body);
        //            var deserialized = JsonConvert.DeserializeObject<Gift>(message);
        //            Console.WriteLine("Received Gift Object{0} {1}", deserialized.Code, deserialized.GiftBalance);
        //            baseVoucherService.UpdateGiftVoucherAmount(deserialized.Code, deserialized.GiftBalance);
        //            Console.WriteLine("Successful Update of GiftVoucher amount for voucher {0}", deserialized.Code);
        //        };

        //        channel.BasicConsume(queue: "gift-three",
        //                             autoAck: true,
        //                             consumer: consumer);

        //        Console.WriteLine("Press [enter] to exit.");
        //    }
        //}

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"Gift Redemption task doing background work.");

                // This eShopOnContainers method is querying a database table 
                // and publishing events into the Event Bus (RabbitMS / ServiceBus)
                CodeReceiver();

               // updateVoucher();
                _logger.LogDebug($"////////////////////////////////////////////////////////////");
            }
            return null;
        }
    }
}
