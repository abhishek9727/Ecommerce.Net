
using Confluent.Kafka;
using Ecommerce.Model;
using Ecommerce.ProductServices.Data;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Ecommerce.ProductServices.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(()=>
            {
                _ = ConsumeAsync("order-topic", stoppingToken);
            },stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var cosumerResult = consumer.Consume();

                var order = JsonConvert.DeserializeObject<OrderModel>(cosumerResult.Message.Value);

                using var scope = _scopeFactory.CreateScope();
                var dbcontext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                var product =  await dbcontext.Products.FindAsync(order.ProductId);

                if(product != null)
                {
                    product.Quantity -= order.Quantity;
                    await dbcontext.SaveChangesAsync();
                }
              
            }
            consumer.Close();
        }
    }
}
