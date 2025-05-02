using Confluent.Kafka;
using Ecommerce.Model;
using Ecommerce.OrderServices.Data;
using Ecommerce.OrderServices.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Ecommerce.OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
         private readonly OrderDbContext _dbContext;
        private readonly IKafkaProducer _kafkapro;

        public OrderController(OrderDbContext dbContext, IKafkaProducer kafkapro)
        {
            _dbContext = dbContext;
            _kafkapro = kafkapro;
        }

        [HttpGet]
        public async Task<List<OrderModel>> GetOrders()
        {
            return await _dbContext.Orders.ToListAsync(); 
        }


        [HttpPost]
        public async Task<ActionResult<OrderModel>> CreateOrder(OrderModel order)
        {
            try
            {
                order.OrderDate = DateTime.Now;
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                // Produce a message using Newtonsoft.Json
                await _kafkapro.ProduceAsync("order-topic", new Message<string, string>
                {
                    Key = order.Id.ToString(),
                    Value = JsonConvert.SerializeObject(order)
                });

                return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order.");
            }
        }



    }
}
