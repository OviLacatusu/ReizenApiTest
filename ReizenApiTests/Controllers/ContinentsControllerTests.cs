using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace ReizenApi.Controllers.Tests
{
    [TestClass ()]
    public class ContinentsControllerTests
    {
        private ContinentsController continentsController;
        private Mock<ICountriesContinentsRepository> mockCountriesContinentsRepository;
        private Mock<ILogger<ContinentsController>> mockLogger;
        private Mock<IMapper> mockMapper;

        [TestInitialize]
        public void Setup ()
        {
            mockLogger = new Mock<ILogger<ContinentsController>> ();
            mockMapper = new Mock<IMapper> ();  
            mockCountriesContinentsRepository = new Mock<ICountriesContinentsRepository> ();
            continentsController = new ContinentsController(mockCountriesContinentsRepository.Object, mockMapper.Object, mockLogger.Object);
        }
        [TestMethod ()]
        public async Task GetContinentsAsyncTest_WithContinents_ReturnsOkResult ()
        {
            var continents = new List<ContinentDAL> () {
                new ContinentDAL { Name = "Asia", Id = 1 },
                new ContinentDAL { Name = "Europe", Id= 2 } };
            var continentsDTO = new List<ContinentDTO> () { 
                new ContinentDTO { Name = "Asia", Id = 1 }, 
                new ContinentDTO { Name = "Europe", Id = 2} };
            mockCountriesContinentsRepository.Setup (repo => repo.GetContinentsAsync()).ReturnsAsync(Result<IList<ContinentDAL>>.Success(continents));
            mockMapper.Setup (mapper => mapper.Map<ICollection<ContinentDTO>>(continents)).Returns(continentsDTO);

            var result = await continentsController.GetContinentsAsync();

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (continentsDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetContinentsAsync_WithNoContinents_ReturnsNotFound ()
        {
            string error = "No continents found: No world regions found";

            mockCountriesContinentsRepository.Setup (repo => repo.GetContinentsAsync ()).ReturnsAsync (Result<IList<ContinentDAL>>.Failure (error));

            var result = await continentsController.GetContinentsAsync ();
            var notFoundResult = new NotFoundObjectResult (error);

            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
            Assert.AreEqual (notFoundResult.Value, (result as NotFoundObjectResult)?.Value); 
        }

        [TestMethod ()]
        public async Task GetCountriesOfContinentAsync_WithValidContinentName_ReturnsOkResult ()
        {
            var continentObj = new ContinentDAL { Name = "Europe", Id = 1};
            var continentName = "Europe";
            var countries = new List<CountryDAL>() {
                new CountryDAL { Name = "Belgium", Id = 30, Continent = continentObj, Continentid = 1 },
                new CountryDAL { Name = "Netherlands", Id = 31, Continent = continentObj, Continentid = 1 }
            };
            continentObj.Countries = countries;

            var countriesDTO = new List<CountryDTO> () {
                new CountryDTO {
                    Name = "Belgium",
                    Id = 30
                },
                new CountryDTO {
                    Name = "Netherlands",
                    Id = 31
                }
            };
            mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Success (countries));
            mockMapper.Setup (mapper => mapper.Map<ICollection<CountryDTO>>(countries)).Returns(countriesDTO);

            var result = await continentsController.GetCountriesOfContinentAsync (continentName);

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (countriesDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetCountriesOfContinentAsync_WithContinentAsEmptyString_ReturnsBadRequest ()
        {
            var continentName = String.Empty;
            string error = "Invalid continent name";
            mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Failure (error));
            var badRequest = new BadRequestObjectResult (error);

            var result = await continentsController.GetCountriesOfContinentAsync(continentName);

            Assert.IsInstanceOfType(result, typeof (BadRequestObjectResult));
            Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);

        }
        [TestMethod ()]
        public async Task GetCountriesOfContinentAsync_WithNoCountries_ReturnsNotFound ()
        {
            var error = "No countries found for continent";
            var continentName = "Antarctica";
            var continentDAL = new ContinentDAL { Name = continentName, Id = 6 };
            var countriesDAL = new List<CountryDAL> ();
            continentDAL.Countries = countriesDAL;

            mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Failure (error));

            var result = await continentsController.GetCountriesOfContinentAsync (continentName);

            var notFound = new NotFoundObjectResult (error);
            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
            Assert.AreEqual (notFound.Value, (result as NotFoundObjectResult)?.Value);

        }
        [TestMethod ()]
        public async Task GetCountriesOfContinentAsync_WithContinensAsNull_ReturnsBadRequest ()
        {
            string? continentName = null;
            var error = $"Invalid continent name";
            mockCountriesContinentsRepository.Setup (repo => repo.GetCountriesOfContinentAsync (continentName)).ReturnsAsync (Result<IList<CountryDAL>>.Failure (""));
            var badRequest = new BadRequestObjectResult (error);

            var result = await continentsController.GetCountriesOfContinentAsync(continentName);

            Assert.IsInstanceOfType (result, typeof(BadRequestObjectResult));
            Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);
            
        }
        [TestMethod ()]
        public void PostTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void PutTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void DeleteTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void GetCountriesTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void GetOfContinentTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void GetTest ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void PostTest1 ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void PutTest1 ()
        {
            Assert.Fail ();
        }

        [TestMethod ()]
        public void DeleteTest1 ()
        {
            Assert.Fail ();
        }
    }
}