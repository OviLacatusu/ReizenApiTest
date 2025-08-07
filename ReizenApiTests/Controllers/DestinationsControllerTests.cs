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
    public class DestinationsControllerTests
    {
        private Mock<ICountriesContinentsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<DestinationsController>> _mockLogger;
        private DestinationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<ICountriesContinentsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DestinationsController>>();
            
            _controller = new DestinationsController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAll_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var destinations = new List<DestinationDAL> 
            { 
                new DestinationDAL { CountryId = 1, PlaceName = "Bangkok", Country = new CountryDAL { Name = "Thailand" } },
                new DestinationDAL { CountryId = 2, PlaceName = "Amsterdam", Country = new CountryDAL { Name = "Netherlands" } }
            };
            var destinationDtos = new List<DestinationDTO> 
            { 
                new DestinationDTO { PlaceName = "Bangkok", Country = new CountryDTO { Name = "Thailand" } },
                new DestinationDTO { PlaceName = "Amsterdam", Country = new CountryDTO { Name = "Netherlands" } }
            };

            _mockRepository.Setup(x => x.GetDestinationsAsync())
                .ReturnsAsync(Result<IList<DestinationDAL>>.Success(destinations));
            _mockMapper.Setup(x => x.Map<ICollection<DestinationDTO>>(destinations))
                .Returns(destinationDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(destinationDtos, okResult.Value);
        }

        [TestMethod]
        public async Task GetAll_WhenNoDestinationsFound_ReturnsNotFound()
        {
            // Arrange
            var errorMessage = "No destinations found";

            _mockRepository.Setup(x => x.GetDestinationsAsync())
                .ReturnsAsync(Result<IList<DestinationDAL>>.Failure(errorMessage));

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(errorMessage, notFoundResult.Value);
        }

        [TestMethod]
        public async Task GetAll_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var exceptionMessage = "Database error";

            _mockRepository.Setup(x => x.GetDestinationsAsync())
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue(objectResult.Value.ToString().Contains(exceptionMessage));
        }

        [TestMethod]
        public async Task GetByCountry_WithValidCountryName_ReturnsOkResult()
        {
            // Arrange
            var countryName = "Thailand";
            var destinations = new List<DestinationDAL> 
            { 
                new DestinationDAL { CountryId = 1, PlaceName = "Bangkok", Country = new CountryDAL { Name = "Thailand" } },
                new DestinationDAL { CountryId = 2, PlaceName = "Phuket", Country = new CountryDAL { Name = "Thailand" } }
            };
            var destinationDtos = new List<DestinationDTO> 
            { 
                new DestinationDTO { PlaceName = "Bangkok", Country = new CountryDTO { Name = "Thailand" } },
                new DestinationDTO { PlaceName = "Phuket", Country = new CountryDTO { Name = "Thailand" } }
            };

            _mockRepository.Setup(x => x.GetDestinationsOfCountryAsync(countryName))
                .ReturnsAsync(Result<IList<DestinationDAL>>.Success(destinations));
            _mockMapper.Setup(x => x.Map<ICollection<DestinationDTO>>(destinations))
                .Returns(destinationDtos);

            // Act
            var result = await _controller.GetByCountry(countryName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(destinationDtos, okResult.Value);
        }

        [TestMethod]
        public async Task GetByCountry_WithEmptyCountryName_ReturnsBadRequest()
        {
            // Arrange
            string countryName = "";

            // Act
            var result = await _controller.GetByCountry(countryName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetByCountry_WithNullCountryName_ReturnsBadRequest()
        {
            // Arrange
            string countryName = null;

            // Act
            var result = await _controller.GetByCountry(countryName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetByCountry_WhenNoDestinationsFound_ReturnsNotFound()
        {
            // Arrange
            var countryName = "Antarctica";

            _mockRepository.Setup(x => x.GetDestinationsOfCountryAsync(countryName))
                .ReturnsAsync(Result<IList<DestinationDAL>>.Failure("No destinations found"));

            // Act
            var result = await _controller.GetByCountry(countryName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetByCountry_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var countryName = "Thailand";
            var exceptionMessage = "Database error";

            _mockRepository.Setup(x => x.GetDestinationsOfCountryAsync(countryName))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetByCountry(countryName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_WithValidDestinationDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var destinationDto = new DestinationDTO 
            { 
                PlaceName = "Bangkok", 
                Country = new CountryDTO { Name = "Thailand" } 
            };
            var destinationDAL = new DestinationDAL 
            { 
                CountryId = 1,
                PlaceName = "Bangkok", 
                Country = new CountryDAL { Name = "Thailand" } 
            };
            var createdDestination = new DestinationDAL 
            { 
                CountryId = 1,
                PlaceName = "Bangkok", 
                Country = new CountryDAL { Name = "Thailand" } 
            };
            var createdDestinationDto = new DestinationDTO 
            { 
                PlaceName = "Bangkok", 
                Country = new CountryDTO { Name = "Thailand" } 
            };

            _mockMapper.Setup(x => x.Map<DestinationDAL>(destinationDto)).Returns(destinationDAL);
            _mockRepository.Setup(x => x.AddDestinationAsync(destinationDAL))
                .ReturnsAsync(Result<DestinationDAL>.Success(createdDestination));
            _mockMapper.Setup(x => x.Map<DestinationDTO>(createdDestination)).Returns(createdDestinationDto);

            // Act
            var result = await _controller.Post(destinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdAtResult = result as CreatedAtActionResult;
            Assert.AreEqual(createdDestinationDto, createdAtResult.Value);
            Assert.AreEqual("GetByCountry", createdAtResult.ActionName);
        }

        [TestMethod]
        public async Task Post_WithNullDestinationDto_ReturnsBadRequest()
        {
            // Arrange
            DestinationDTO destinationDto = null;

            // Act
            var result = await _controller.Post(destinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Invalid data", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Post_WhenRepositoryFails_Returns500()
        {
            // Arrange
            var destinationDto = new DestinationDTO 
            {  
                PlaceName = "Bangkok", 
                Country = new CountryDTO { Name = "Thailand" } 
            };
            var destinationDAL = new DestinationDAL 
            { 
                CountryId = 1, 
                PlaceName = "Bangkok", 
                Country = new CountryDAL { Name = "Thailand" } 
            };

            _mockMapper.Setup(x => x.Map<DestinationDAL>(destinationDto)).Returns(destinationDAL);
            _mockRepository.Setup(x => x.AddDestinationAsync(destinationDAL))
                .ReturnsAsync(Result<DestinationDAL>.Failure("Failed to add destination"));

            // Act
            var result = await _controller.Post(destinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_WhenExceptionOccurs_Returns500()
        {
            // Arrange
            var destinationDto = new DestinationDTO 
            { 
                PlaceName = "Bangkok", 
                Country = new CountryDTO { Name = "Thailand" } 
            };
            var exceptionMessage = "Database error";

            _mockMapper.Setup(x => x.Map<DestinationDAL>(destinationDto))
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = await _controller.Post(destinationDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
    }
} 