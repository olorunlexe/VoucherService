using Microsoft.Extensions.Hosting;
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
    public class GiftRedemptionUpdate: BackgroundService
    {
        private IVoucherService baseVoucherService;
        private readonly ILogger<GiftRedemption> _logger;

        public GiftRedemptionUpdate(IVoucherService baseVoucherService, ILogger<GiftRedemption> _logger)
        {
            this.baseVoucherService = baseVoucherService;
            this._logger = _logger;
        }

        public void voucherUpdate()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.99.100",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "gift-three",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var deserialized = JsonConvert.DeserializeObject<Gift>(message);
                    Console.WriteLine("Received Gift Object{0} {1}", deserialized.Code, deserialized.GiftBalance);
                    baseVoucherService.UpdateGiftVoucherAmount(deserialized.Code, deserialized.GiftBalance);
                    Console.WriteLine("Successful Update of GiftVoucher amount for voucher {0}", deserialized.Code);
                };

                channel.BasicConsume(queue: "gift-three",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           

    
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GiftRedemptionUpdateService is starting.");
                voucherUpdate();


               
            }
            return null;
        }
    }
}
