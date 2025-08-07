using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Reizen.Data.Models;
using Reizen.Data.Services;
using Reizen.CommonClasses.DTOs;
using ReizenApi.Controllers;
using Reizen.CommonClasses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReizenApiTests.Controllers
{
    [TestClass]
    public class BookingsControllerTests
    {
        private Mock<IBookingsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<DestinationsController>> _mockLogger;
        private BookingsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IBookingsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DestinationsController>>();
            
            _controller = new BookingsController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Get_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var bookings = new List<BookingDAL> 
            { 
                new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 },
                new BookingDAL { Id = 2, NumberOfAdults = 1, NumberOfMinors = 0 }
            };
            var bookingDTOs = new List<BookingDTO> 
            { 
                new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 },
                new BookingDTO { Id = 2, NumberOfAdults = 1, NumberOfMinors = 0 }
            };

            _mockRepository.Setup(x => x.GetBookingsAsync())
                .ReturnsAsync(Result<IList<BookingDAL>>.Success(bookings));
            _mockMapper.Setup(x => x.Map<ICollection<BookingDTO>>(bookings))
                .Returns(bookingDTOs);
            var okResult = new OkObjectResult(bookingDTOs);
            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(bookingDTOs, (result as ObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Get_WhenNoBookingsFound_ReturnsNotFound()
        {
            // Arrange
            var errorMessage = "No bookings found";

            _mockRepository.Setup(x => x.GetBookingsAsync())
                .ReturnsAsync(Result<IList<BookingDAL>>.Failure(errorMessage));
            var notFoundResult = new NotFoundObjectResult(errorMessage);
            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(errorMessage, (result as NotFoundObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Get_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var exceptionMessage = "Database error";

            _mockRepository.Setup(x => x.GetBookingsAsync())
                .ThrowsAsync(new Exception(exceptionMessage));
            var objectResult = new ObjectResult("Database error");
            objectResult.StatusCode = 500;
            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue((result as ObjectResult)?.Value?.ToString()?.Contains(exceptionMessage));
        }

        [TestMethod]
        public async Task GetWithId_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var booking = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Success(booking));
            _mockMapper.Setup(x => x.Map<BookingDTO>(booking))
                .Returns(bookingDto);
            var okResult = new OkObjectResult(bookingDto);
            // Act
            var result = await _controller.GetWithId(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(bookingDto, (result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetWithId_WithNegativeId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _controller.GetWithId(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetWithId_WhenBookingNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var errorMessage = "Booking not found";

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Failure(errorMessage));
            var notFoundResult = new NotFoundObjectResult(errorMessage);
            // Act
            var result = await _controller.GetWithId(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(errorMessage, (result as NotFoundObjectResult)?.Value);
        }

        [TestMethod]
        public async Task GetWithId_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var id = 1;
            var exceptionMessage = "Database error";

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ThrowsAsync(new Exception(exceptionMessage));
            var objectResult = new ObjectResult(exceptionMessage);
            objectResult.StatusCode = 500;
            // Act
            var result = await _controller.GetWithId(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue((result as ObjectResult)?.Value?.ToString()?.Contains(exceptionMessage));
        }

        [TestMethod]
        public async Task Post_WithValidBookingDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var bookingDAL = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var createdBooking = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var createdBookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };

            _mockMapper.Setup(x => x.Map<BookingDAL>(bookingDto)).Returns(bookingDAL);
            _mockRepository.Setup(x => x.AddBookingAsync(bookingDAL))
                .ReturnsAsync(Result<BookingDAL>.Success(createdBooking));
            _mockMapper.Setup(x => x.Map<BookingDTO>(createdBooking)).Returns(createdBookingDto);
            var createdAtResult = new CreatedAtActionResult("Post", "BookingsController", new{ id = 1}, createdBookingDto);
            // Act
            var result = await _controller.Post(bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.AreEqual(createdBookingDto, (result as CreatedAtActionResult)?.Value);
            Assert.AreEqual("Post", createdAtResult.ActionName);
        }

        [TestMethod]
        public async Task Post_WithNullBookingDto_ReturnsBadRequest()
        {
            // Arrange
            BookingDTO bookingDto = null;

            // Act
            var result = await _controller.Post(bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Post_WithZeroAdultsAndMinors_ReturnsBadRequest()
        {
            // Arrange
            var errorMessage = "Number of adults or minors invalid";
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 0, NumberOfMinors = 0 };
            var badRequestResult = new BadRequestObjectResult(errorMessage);
            // Act
            var result = await _controller.Post(bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(badRequestResult.Value, (result as BadRequestObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Post_WhenRepositoryFails_Returns500()
        {
            // Arrange
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var bookingDAL = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };

            _mockMapper.Setup(x => x.Map<BookingDAL>(bookingDto)).Returns(bookingDAL);
            _mockRepository.Setup(x => x.AddBookingAsync(bookingDAL))
                .ReturnsAsync(Result<BookingDAL>.Failure("Failed to add booking"));
            var objectResult = new ObjectResult("Failed to add booking");
            objectResult.StatusCode = 500;
            // Act
            var result = await _controller.Post(bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(500, (result as ObjectResult)?.StatusCode);
        }

        [TestMethod]
        public async Task Post_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var exceptionMessage = "An error occurred while processing your request";

            _mockMapper.Setup(x => x.Map<BookingDAL>(bookingDto))
                .Throws(new Exception(exceptionMessage));
            var objectResult = new ObjectResult (exceptionMessage);
            objectResult.StatusCode = 500;
            // Act
            var result = await _controller.Post(bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue((result as ObjectResult)?.Value?.ToString()?.Contains(exceptionMessage));
        }

        [TestMethod]
        public async Task Put_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 3, NumberOfMinors = 2 };
            var bookingDAL = new BookingDAL { Id = 1, NumberOfAdults = 3, NumberOfMinors = 2 };
            var updatedBooking = new BookingDAL { Id = 1, NumberOfAdults = 3, NumberOfMinors = 2 };
            var updatedBookingDto = new BookingDTO { Id = 1, NumberOfAdults = 3, NumberOfMinors = 2 };

            _mockRepository.Setup (repo => repo.GetBookingWithIdAsync (id)).ReturnsAsync (Result<BookingDAL>.Success (bookingDAL));
            _mockMapper.Setup(x => x.Map<BookingDAL>(bookingDto)).Returns(bookingDAL);
            _mockRepository.Setup(x => x.UpdateBookingAsync(bookingDAL, id))
                .ReturnsAsync(Result<BookingDAL>.Success(updatedBooking));
            _mockMapper.Setup(x => x.Map<BookingDTO>(updatedBooking)).Returns(updatedBookingDto);
            var okResult = new OkObjectResult(updatedBookingDto);
            // Act
            var result = await _controller.Put(id, bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(updatedBookingDto, (result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Put_WithNullBookingDto_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            BookingDTO bookingDto = null;

            // Act
            var result = await _controller.Put(id, bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Put_WhenRepositoryFails_Returns500()
        {
            // Arrange
            var errorMessage = "An error occurred while processing your request";
            var id = 1;
            var bookingDto = new BookingDTO { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var bookingDAL = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };

            _mockMapper.Setup(x => x.Map<BookingDAL>(bookingDto)).Returns(bookingDAL);
            _mockRepository.Setup(x => x.UpdateBookingAsync(bookingDAL, id))
                .ReturnsAsync(Result<BookingDAL>.Failure("Failed to update booking"));
            var objectResult = new ObjectResult(errorMessage);
            // Act
            var result = await _controller.Put(id, bookingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual (objectResult?.Value, (result as ObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Delete_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var booking = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Success(booking));
            _mockRepository.Setup(x => x.DeleteBookingAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Success(booking));

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Delete_WithNegativeId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Delete_WhenBookingNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var errorMessage = "Booking not found";

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Failure(errorMessage));
            var notFoundResult = new NotFoundObjectResult (errorMessage);
            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(errorMessage, (result as NotFoundObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Delete_WhenDeletionFails_Returns500()
        {
            // Arrange
            var id = 1;
            var booking = new BookingDAL { Id = 1, NumberOfAdults = 2, NumberOfMinors = 1 };
            var errorMessage = "Failed to delete booking";

            _mockRepository.Setup(x => x.GetBookingWithIdAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Success(booking));
            _mockRepository.Setup(x => x.DeleteBookingAsync(id))
                .ReturnsAsync(Result<BookingDAL>.Failure(errorMessage));
            var objectResult = new ObjectResult (errorMessage);
            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(500, (result as ObjectResult)?.StatusCode);
        }
    }
} 