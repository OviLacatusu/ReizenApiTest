using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Reizen.Data.Models;
using Reizen.Data.Services;
using Reizen.CommonClasses.DTOs;
using ReizenApi.Controllers;
using Reizen.CommonClasses;

namespace ReizenApiTests.Controllers
{
    [TestClass]
    public class TripsControllerTests
    {
        private Mock<ITripsRepository> _mockTripsRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<TripsController>> _mockLogger;
        private TripsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockTripsRepository = new Mock<ITripsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TripsController>>();
            
            _controller = new TripsController(_mockTripsRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Get_WithValidDestinationCode_ReturnsOkResult()
        {
            // Arrange
            var destinationCode = "BANGK";
            var trips = new List<TripDAL> 
            { 
                new TripDAL { Id = 1, DestinationCode = "Test Trip" } 
            };
            var tripDtos = new List<TripDTO> 
            { 
                new TripDTO { Id = 1, DestinationCode = "Test Trip" } 
            };

            _mockTripsRepository.Setup(x => x.GetTripsToDestinationAsync(destinationCode))
                .ReturnsAsync(Result<IList<TripDAL>>.Success(trips));
            _mockMapper.Setup(x => x.Map<ICollection<TripDTO>>(trips))
                .Returns(tripDtos);

            // Act
            var result = await _controller.Get(destinationCode);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(tripDtos, okResult.Value);
        }

        [TestMethod]
        public async Task Get_WithEmptyDestinationCode_ReturnsBadRequest()
        {
            // Arrange
            string destinationCode = "";

            // Act
            var result = await _controller.Get(destinationCode);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Get_WithNullDestinationCode_ReturnsBadRequest()
        {
            // Arrange
            string destinationCode = null;

            // Act
            var result = await _controller.Get(destinationCode);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Get_WhenNoTripsFound_ReturnsNotFound()
        {
            // Arrange
            var destinationCode = "BANGK";
            var errorMessage = "No trips found";

            _mockTripsRepository.Setup(x => x.GetTripsToDestinationAsync(destinationCode))
                .ReturnsAsync(Result<IList<TripDAL>>.Failure(errorMessage));

            // Act
            var result = await _controller.Get(destinationCode);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(errorMessage, notFoundResult.Value);
        }

        [TestMethod]
        public async Task Get_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var destinationCode = "BANGK";
            var exceptionMessage = "Database error";

            _mockTripsRepository.Setup(x => x.GetTripsToDestinationAsync(destinationCode))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.Get(destinationCode);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue(objectResult.Value.ToString().Contains(exceptionMessage));
        }

        [TestMethod]
        public async Task Get_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var trip = new TripDAL { Id = 1, DestinationCode = "Test Trip" };
            var tripDto = new TripDTO { Id = 1, DestinationCode = "Test Trip" };

            _mockTripsRepository.Setup(x => x.GetTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Success(trip));
            _mockMapper.Setup(x => x.Map<TripDTO>(trip))
                .Returns(tripDto);

            // Act
            var result = await _controller.Get(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(tripDto, okResult.Value);
        }

        [TestMethod]
        public async Task Get_WithNegativeId_ReturnsBadRequest()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _controller.Get(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Get_WhenTripNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var errorMessage = "Trip not found";

            _mockTripsRepository.Setup(x => x.GetTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Failure(errorMessage));

            // Act
            var result = await _controller.Get(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(errorMessage, notFoundResult.Value);
        }

        [TestMethod]
        public async Task Post_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var tripDto = new TripDTO { Id = 1, DestinationCode = "Test Trip" };
            var destinationDto = new DestinationDTO { Code = "Test Destination" };
            var tripDestinationDto = (tripDto, destinationDto);

            var tripDAL = new TripDAL { Id = 1, DestinationCode = "Test Trip" };
            var destinationDAL = new DestinationDAL { CountryId = 1, Code = "Test Destination" };
            var createdTrip = new TripDAL { Id = 1, DestinationCode = "Test Trip" };
            var createdTripDto = new TripDTO { Id = 1, DestinationCode = "Test Trip" };

            _mockMapper.Setup(x => x.Map<TripDAL>(tripDto)).Returns(tripDAL);
            _mockMapper.Setup(x => x.Map<DestinationDAL>(destinationDto)).Returns(destinationDAL);
            _mockTripsRepository.Setup(x => x.AddTripToDestinationAsync(tripDAL, destinationDAL))
                .ReturnsAsync(Result<TripDAL>.Success(createdTrip));
            _mockMapper.Setup(x => x.Map<TripDTO>(createdTrip)).Returns(createdTripDto);

            // Act
            var result = await _controller.Post(tripDestinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(createdTripDto, okResult.Value);
        }

        [TestMethod]
        public async Task Post_WithNullTripDto_ReturnsBadRequest()
        {
            // Arrange
            TripDTO tripDto = null;
            var destinationDto = new DestinationDTO {  Code = "Test Destination" };
            var tripDestinationDto = (tripDto, destinationDto);

            // Act
            var result = await _controller.Post(tripDestinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Post_WithNullDestinationDto_ReturnsBadRequest()
        {
            // Arrange
            var tripDto = new TripDTO { Id = 1, DestinationCode = "Test Trip" };
            DestinationDTO destinationDto = null;
            var tripDestinationDto = (tripDto, destinationDto);

            // Act
            var result = await _controller.Post(tripDestinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Post_WhenRepositoryFails_Returns500()
        {
            // Arrange
            var tripDto = new TripDTO { Id = 1, DestinationCode = "Test Trip" };
            var destinationDto = new DestinationDTO {  Code = "Test Destination" };
            var tripDestinationDto = (tripDto, destinationDto);

            var tripDAL = new TripDAL { Id = 1, DestinationCode = "Test Trip" };
            var destinationDAL = new DestinationDAL { CountryId = 1, Code = "Test Destination" };
            var errorMessage = "Failed to add trip";

            _mockMapper.Setup(x => x.Map<TripDAL>(tripDto)).Returns(tripDAL);
            _mockMapper.Setup(x => x.Map<DestinationDAL>(destinationDto)).Returns(destinationDAL);
            _mockTripsRepository.Setup(x => x.AddTripToDestinationAsync(tripDAL, destinationDAL))
                .ReturnsAsync(Result<TripDAL>.Failure(errorMessage));

            // Act
            var result = await _controller.Post(tripDestinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var trip = new TripDAL { Id = 1, DestinationCode = "Test Trip" };

            _mockTripsRepository.Setup(x => x.GetTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Success(trip));
            _mockTripsRepository.Setup(x => x.DeleteTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Success(trip));

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
        public async Task Delete_WhenTripNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var errorMessage = "Trip not found";

            _mockTripsRepository.Setup(x => x.GetTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Failure(errorMessage));

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(errorMessage, notFoundResult.Value);
        }

        [TestMethod]
        public async Task Delete_WhenDeletionFails_Returns500()
        {
            // Arrange
            var id = 1;
            var trip = new TripDAL { Id = 1, DestinationCode = "Test Trip" };
            var errorMessage = "Failed to delete trip";

            _mockTripsRepository.Setup(x => x.GetTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Success(trip));
            _mockTripsRepository.Setup(x => x.DeleteTripWithIdAsync(id))
                .ReturnsAsync(Result<TripDAL>.Failure(errorMessage));

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
    }
} 