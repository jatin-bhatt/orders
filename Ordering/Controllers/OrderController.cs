using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Models.Aggregate;
using Ordering.Domain.Models.Aggregate.Enum;
using Ordering.Infrastructure.Models;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ordering.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository orderRepository,
            ILogger<OrderController> logger) {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ActionResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ActionResult))]
        public async Task<ActionResult> PostAsync([FromBody] OrderRequestDTO orderDetails) {
            var order = new Order();
            order.SetOrderId(orderDetails.OrderID);
            order.SetOrderRecievedStatus();

            foreach (var item in orderDetails.OrderItems) {
                if (Enum.TryParse(item.ProductType, true, out ProductType type)) {
                    order.AddItem(type, item.Quantity);
                } else {
                    return BadRequest($"type {item.ProductType} is not valid." );
                }
            }

            _logger.LogInformation("Creating Order {@Order}", order);
            await _orderRepository.AddAsync(order);

            bool isSuccess = await _orderRepository.UnitOfWork.SaveEntitiesAsync();
            if (isSuccess) {
                return await GetOrder(order.Id);
            }

            return Problem($"Error occurred");
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAsync(Guid orderId) {
            if (orderId == Guid.Empty) {
                return BadRequest($"Order ID is empty. please use an actual GUID order id");
            }
            return await GetOrder(orderId);
        }

        private async Task<ActionResult> GetOrder(Guid orderId) {
            var order = await _orderRepository.GetAsync(orderId);
            if (order is object) {
                OrderResponseDTO response = new OrderResponseDTO() {
                    OrderID = order.Id,
                    OrderItems = order.OrderItems.Select(
                    i => new OrderItemRequestDTO() {
                        ProductType = System.Enum.GetName(i.Type.GetType(), i.Type).ToLower(),
                        Quantity = i.Quantity
                    }).ToList(),
                    RequiredBinWidth = order.RequiredBinWidth
                };

                if (response is object && response.OrderID != Guid.Empty) {
                    return Ok(response);
                }
            }
            return NotFound();
        }
    }
}
