using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Reizen.CommonClasses;
using Reizen.CommonClasses.DTOs;
using Reizen.Data.Models;
using Reizen.Data.Services;
using ReizenApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReizenApiTests.Controllers
{
    [TestClass]
    public class CountriesControllerTest
    {
        private Mock<ILogger<CountriesController>> _mockLogger;
        private Mock<ICountriesContinentsRepository> _mockCountriesContinentsRepository;
        private Mock<IMapper> _mockMapper;
        private CountriesController _countriesController;

        [TestInitialize]
        public void Setup ()
        {
            _mockLogger = new Mock<ILogger<CountriesController>>();
            _mockMapper = new Mock<IMapper>();
            _mockCountriesContinentsRepository = new Mock<ICountriesContinentsRepository> ();
            _countriesController = new CountriesController (_mockCountriesContinentsRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }
        [TestMethod ()]
        public async Task GetCountriesAsync_WithCountries_ReturnsOkResult ()
        {   // Arrange
            var countriesDTO = new List<CountryDTO> { 
                new CountryDTO { Name = "Belgium", Id = 1 },
                new CountryDTO { Name = "Netherlands", Id = 2 } };
            var countriesDAL = new List<CountryDAL> { 
                new CountryDAL { Name = "Belgium", Id = 1 },
                new CountryDAL { Name = "Netherlands", Id = 2 } };
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesAsync()).ReturnsAsync (Result<IList<CountryDAL>>.Success(countriesDAL));
            _mockMapper.Setup (m => m.Map<ICollection<CountryDTO>> (countriesDAL)).Returns (countriesDTO);
            // Act
            var result = await _countriesController.GetCountries ();
            // Assert
            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (countriesDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetCountriesAsync_WithNoCountries_ReturnsNotFoundResult ()
        {
            var error = "No countries found";
            IList<CountryDAL> countryDALs = null;
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesAsync ()).ReturnsAsync (Result<IList<CountryDAL>>.Failure ("error"));
            var notFound = new NotFoundObjectResult (error);

            var result = await _countriesController.GetCountries ();

            Assert.IsInstanceOfType(result, typeof (NotFoundObjectResult));
            Assert.AreEqual (notFound.Value, (result as NotFoundObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetOfContinent_WithValidName_ReturnsOkObjectResult ()
        {
            var continentName = "Europe";
            var countriesDTO = new List<CountryDTO> {
                new CountryDTO { Name = "Belgium", Id = 1 },
                new CountryDTO { Name = "Netherlands", Id = 2 } };
            var countriesDAL = new List<CountryDAL> {
                new CountryDAL { Name = "Belgium", Id = 1 },
                new CountryDAL { Name = "Netherlands", Id = 2 } };
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Success(countriesDAL));
            _mockMapper.Setup (m => m.Map<ICollection<CountryDTO>> (countriesDAL)).Returns (countriesDTO);

            var result = await _countriesController.GetCountriesOfContinent (continentName);

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (countriesDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetCountriesOfContinent_WithContinentAsNull_ReturnsBadRequestResult ()
        {
            string? continentName = null;
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync(continentName)).ReturnsAsync(Result<IList<CountryDAL>>.Failure("error"));

            var result = await _countriesController.GetCountriesOfContinent(continentName);

            Assert.IsInstanceOfType(result, typeof (BadRequestObjectResult));
        }
        [TestMethod ()]
        public async Task GetCountriesOfContinent_WithContinentAsEmptyString_ReturnsBadRequestResult ()
        {
            var continentName = String.Empty;
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Failure ("error"));

            var result = await _countriesController.GetCountriesOfContinent (continentName);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
        }
        [TestMethod ()]
        public async Task GetCountriesOfContinent_WithNoCountriesAndWithValidContinentName_ReturnsNotFoundResult ()
        {
            string? continentName = "Antarctica";
            ContinentDAL continent = new ContinentDAL { Name = continentName, Id = 6, Countries = new List<CountryDAL> () };

            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Failure ("error"));

            var result = await _countriesController.GetCountriesOfContinent (continentName);

            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
        }

        [TestMethod ()]
        public async Task GetCountryWithId_WithCountriesAndValidId_ReturnsOkResult ()
        {
            var countryId = 1;
            var countryDTO = new CountryDTO { Name = "Belgium", Id = 1 };
            var countryDAL = new CountryDAL{ Name = "Belgium", Id = 1 };
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountryWithIdAsync (countryId)).ReturnsAsync(Result<CountryDAL>.Success(countryDAL));
            _mockMapper.Setup (m => m.Map<CountryDTO> (countryDAL)).Returns (countryDTO);

            var result = await _countriesController.GetCountryWithId (countryId);

            Assert.IsInstanceOfType(result, typeof (OkObjectResult));
            Assert.AreEqual (countryDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetCountryWithId_WhenCountryNotFound_ReturnsNotFoundResult ()
        {
            var countryId = 888;
            var error = $"Country with ID {countryId} not found";
            var notFound = new NotFoundObjectResult (error);
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountryWithIdAsync (countryId)).ReturnsAsync (Result<CountryDAL>.Failure (error));

            var result = await _countriesController.GetCountryWithId (countryId);

            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
            Assert.AreEqual (notFound.Value, (result as NotFoundObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetCountryWithId_WithNegativeId_ReturnsBadRequestResult ()
        {
            var countryId = -1;
            var error = $"Invalid Id";
            var notFound = new BadRequestObjectResult (error);
            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountryWithIdAsync (countryId)).ReturnsAsync (Result<CountryDAL>.Failure (error));

            var result = await _countriesController.GetCountryWithId (countryId);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
            Assert.AreEqual (notFound.Value, (result as BadRequestObjectResult)?.Value);
        }

        [TestMethod ()]
        public async Task Post_WithValidCountryDTO_ReturnsCreatedAtActionResult ()
        {
            var countryDTO = new CountryDTO { Name = "Belgium", Id = 1 };
            var countryDAL = new CountryDAL { Name = "Belgium", Id = 1 };
            var newCountryDAL = new CountryDAL { Name = "Belgium", Id = 1 };
            var newCountryDTO = new CountryDTO { Name = "Belgium", Id = 1 };
            _mockMapper.Setup (m => m.Map<CountryDAL> (countryDTO)).Returns (countryDAL);
            _mockCountriesContinentsRepository.Setup (repo => repo.AddCountryAsync (countryDAL)).ReturnsAsync (Result<CountryDAL>.Success(newCountryDAL));
            _mockMapper.Setup (m => m.Map<CountryDTO> (newCountryDAL)).Returns (newCountryDTO);

            var result = await _countriesController.Post (countryDTO);

            Assert.IsInstanceOfType(result, typeof (CreatedAtActionResult));
            Assert.AreEqual (newCountryDTO, (result as CreatedAtActionResult)?.Value);
        }

        [TestMethod ()]
        public async Task Post_WithCountryDTOAsNull_ReturnsBadRequestResult ()
        {
            CountryDAL? countryDAL = null;
            CountryDTO? countryDTO = null;
            var error = "Invalid country data";
            _mockCountriesContinentsRepository.Setup (repo => repo.AddCountryAsync (countryDAL)).ReturnsAsync (Result<CountryDAL>.Failure (error));
            var badRequest = new BadRequestObjectResult (error);

            var result = await _countriesController.Post (countryDTO);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
            Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);
        }
        [TestMethod]
        public async Task Put_WithValidData_ReturnsOkResult ()
        {
            // Arrange
            var id = 1;
            var countryDTO = new CountryDTO { Id = 1, Name = "Netherlands Updated", Continent = null}; 
            //var existingCountry = new CountryDAL { Id = 1, Name = "Netherlands Updated", Continent = null };
            var countryDAL = new CountryDAL { Id = 1, Name = "Netherlands Updated", Continent = null };
            var updatedCountryDAL = new CountryDAL { Id = 1, Name = "Netherlands Updated", Continent = null };
            var updatedCountryDTO = new CountryDTO { Id = 1, Name = "Netherlands Updated", Continent = null };

            _mockCountriesContinentsRepository.Setup (repo => repo.GetCountryWithIdAsync (id)).ReturnsAsync (Result<CountryDAL>.Success(countryDAL));
            _mockMapper.Setup (x => x.Map<CountryDAL> (countryDTO)).Returns (countryDAL);
            _mockCountriesContinentsRepository.Setup (x => x.UpdateCountryWithIdAsync (id, countryDAL))
                .ReturnsAsync (Result<CountryDAL>.Success (updatedCountryDAL));
            _mockMapper.Setup (x => x.Map<CountryDTO> (updatedCountryDAL)).Returns (updatedCountryDTO);

            // Act
            var result = await _countriesController.Put (id, countryDTO);

            // Assert
            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual (updatedCountryDTO, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_WithNullCountryDto_ReturnsBadRequest ()
        {
            // Arrange
            var id = 1;
            CountryDTO countryDto = null;

            // Act
            var result = await _countriesController.Put (id, countryDto);

            // Assert
            Assert.IsInstanceOfType (result, typeof (BadRequestResult));
        }

        [TestMethod]
        public async Task Put_WhenRepositoryFails_Returns500 ()
        {
            // Arrange
            var id = 1;
            var countryDto = new CountryDTO { Id = 1, Name = "Netherlands"};
            var countryDAL = new CountryDAL { Id = 1, Name = "Netherlands"};
            var errorMessage = "Failed to update country";

            _mockMapper.Setup (x => x.Map<CountryDAL> (countryDto)).Returns (countryDAL);
            _mockCountriesContinentsRepository.Setup (x => x.UpdateCountryWithIdAsync (id, countryDAL))
                .ReturnsAsync (Result<CountryDAL>.Failure (errorMessage));

            // Act
            var result = await _countriesController.Put (id, countryDto);

            // Assert
            Assert.IsInstanceOfType (result, typeof (ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual (500, objectResult?.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WithValidId_ReturnsOkResult ()
        {
            // Arrange
            var id = 1;
            var country = new CountryDAL { Id = 1, Name = "Netherlands" };
            var countryDTO = new CountryDTO { Id = 1, Name = "Netherlands" };

            _mockCountriesContinentsRepository.Setup (x => x.GetCountryWithIdAsync (id))
                .ReturnsAsync (Result<CountryDAL>.Success (country));
            _mockCountriesContinentsRepository.Setup (x => x.DeleteCountryWithIdAsync (id))
                .ReturnsAsync (Result<CountryDAL>.Success (country));
            _mockMapper.Setup (m => m.Map<CountryDTO> (country)).Returns (countryDTO);

            // Act
            var result = await _countriesController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (countryDTO, (result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Delete_WithNegativeId_ReturnsBadRequest ()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _countriesController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (BadRequestResult));
        }

        [TestMethod]
        public async Task Delete_WhenCountryNotFound_ReturnsNotFound ()
        {
            // Arrange
            var id = 999;
            var errorMessage = "Country not found";

            _mockCountriesContinentsRepository.Setup (x => x.GetCountryWithIdAsync (id))
                .ReturnsAsync (Result<CountryDAL>.Failure (errorMessage));

            // Act
            var result = await _countriesController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual (errorMessage, notFoundResult?.Value);
        }

        [TestMethod]
        public async Task Delete_WhenDeletionFails_Returns500 ()
        {
            // Arrange
            var id = 1;
            var country = new CountryDAL { Id = 1, Name = "Netherlands" };
            var errorMessage = "Failed to delete country";

            _mockCountriesContinentsRepository.Setup (x => x.GetCountryWithIdAsync (id))
                .ReturnsAsync (Result<CountryDAL>.Success (country));
            _mockCountriesContinentsRepository.Setup (x => x.DeleteCountryWithIdAsync (id))
                .ReturnsAsync (Result<CountryDAL>.Failure (errorMessage));

            // Act
            var result = await _countriesController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual (500, objectResult?.StatusCode);
        }
    }
}
