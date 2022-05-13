using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.API.Controllers;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Models;
using System.Net;
using Xunit;

namespace Ordering.API.Tests {
    public class OrderControllerTests {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ILogger<OrderController>> _loggerMock;

        public OrderControllerTests() {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<OrderController>>();
        }

        [Fact]
        public async Task GetAsync_OrderNotFound_ReturnStatus404NotFound() {
            // Arrange
            _ = _orderRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Models.Aggregate.Order)null);
            Guid fakeOrderId = Guid.NewGuid();

            // Act
            var actual = await GetController().GetAsync(fakeOrderId) as StatusCodeResult;

            // Assert
            Assert.Equal(404, actual.StatusCode);
        }


        [Fact]
        public async Task GetAsync_OrderIdEmpty_ReturnStatus400BadRequest() {
            // Arrange
            Guid fakeOrderId = Guid.Empty;

            // Act
            var actual = await GetController().GetAsync(fakeOrderId) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, actual.StatusCode);
        }

        [Fact]
        public async Task GetAsync_OrderExists_ReturnSuccessWithOrderResponseData() {
            // Arrange
            var order = new Domain.Models.Aggregate.Order();
            Guid fakeOrderId = Guid.NewGuid();
            order.SetOrderId(fakeOrderId);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Calendar, 5);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);
            _ = _orderRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            var expected = new OrderResponseDTO {
                OrderID = fakeOrderId,
                OrderItems = new List<OrderItemRequestDTO> {
                    new OrderItemRequestDTO {
                        ProductType = "calendar",
                        Quantity = 5
                    },
                    new OrderItemRequestDTO {
                        ProductType = "canvas",
                        Quantity = 10
                    }
                },
                RequiredBinWidth = 210,
            };

            // Act
            var actual = await GetController().GetAsync(fakeOrderId) as OkObjectResult;

            // Assert
            Assert.Equal(200, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
        }


        [Fact]
        public async Task PostAsync_OrderSavedSuccessfully_ReturnStatus200Ok() {
            // Arrange
            var order = new Domain.Models.Aggregate.Order();
            Guid fakeOrderId = Guid.NewGuid();
            order.SetOrderId(fakeOrderId);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Calendar, 5);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);
            _ = _orderRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            var fakeOrderRequest = new OrderRequestDTO {
                OrderID = fakeOrderId,
                OrderItems = new List<OrderItemRequestDTO> {
                    new OrderItemRequestDTO {
                        ProductType = "calendar",
                        Quantity = 5
                    },
                    new OrderItemRequestDTO {
                        ProductType = "canvas",
                        Quantity = 10
                    }
                },
            };
            _ = _orderRepositoryMock.Setup(r => r.UnitOfWork.SaveEntitiesAsync(default(CancellationToken))).ReturnsAsync(true);

            // Act
            var actual = await GetController().PostAsync(fakeOrderRequest) as OkObjectResult;

            // Assert
            Assert.Equal(200, actual.StatusCode);
        }

        [Fact]
        public async Task PostAsync_AnyInvalidProductType_ReturnStatus400BadRequest() {
            // Arrange
            var order = new Domain.Models.Aggregate.Order();
            Guid fakeOrderId = Guid.NewGuid();

            var fakeOrderRequest = new OrderRequestDTO {
                OrderID = fakeOrderId,
                OrderItems = new List<OrderItemRequestDTO> {
                    new OrderItemRequestDTO {
                        ProductType = "invalidType1",
                        Quantity = 5
                    },
                    new OrderItemRequestDTO {
                        ProductType = "canvas",
                        Quantity = 10
                    }
                },
            };


            // Act
            var actual = await GetController().PostAsync(fakeOrderRequest) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, actual.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ErrorInSaving_ReturnStatus500Error() {
            // Arrange
            var order = new Domain.Models.Aggregate.Order();
            Guid fakeOrderId = Guid.NewGuid();
            order.SetOrderId(fakeOrderId);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Calendar, 5);
            order.AddItem(Domain.Models.Aggregate.Enum.ProductType.Canvas, 10);
            _ = _orderRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            var fakeOrderRequest = new OrderRequestDTO {
                OrderID = fakeOrderId,
                OrderItems = new List<OrderItemRequestDTO> {
                    new OrderItemRequestDTO {
                        ProductType = "calendar",
                        Quantity = 5
                    },
                    new OrderItemRequestDTO {
                        ProductType = "canvas",
                        Quantity = 10
                    }
                },
            };
            _ = _orderRepositoryMock.Setup(r => r.UnitOfWork.SaveEntitiesAsync(default(CancellationToken))).ReturnsAsync(false);

            // Act
            var actual = await GetController().PostAsync(fakeOrderRequest) as ObjectResult;

            // Assert
            Assert.Equal(500, actual.StatusCode);
        }

        private OrderController GetController() {
            return new OrderController(_orderRepositoryMock.Object, _loggerMock.Object);
        }
    }
}