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

namespace ReizenApiTests.Controllers
{
    [TestClass]
    public class ClientsControllerTests
    {
        private Mock<ILogger<ClientsController>> _mockLogger;
        private Mock<IClientsRepository> _mockClientsRepository;
        private Mock<IMapper> _mockMapper;
        private ClientsController _clientsController;

        [TestInitialize]
        public void Setup() 
        {
            _mockLogger = new Mock<ILogger<ClientsController>> ();
            _mockClientsRepository = new Mock<IClientsRepository> ();
            _mockMapper = new Mock<IMapper> ();
            _clientsController = new ClientsController (_mockClientsRepository.Object, _mockMapper.Object, _mockLogger.Object) ;
        }

        [TestMethod ()]
        public async Task GetClientsAsync_WithValidClients_ReturnsOkResult ()
        {
            var clientsDAL = new List<ClientDAL>() {
                new ClientDAL { FamilyName = "Test1FamName", FirstName = "Test1FirstName", Residence = null, Id = 1 }, 
                new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 }};
            var clientsDTO = new List<ClientDTO>() {
                new ClientDTO { FamilyName = "Test1FamName", FirstName = "Test1FirstName", Residence = null, Id = 1 },
                new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 }};
            
            _mockClientsRepository.Setup (repo => repo.GetClientsAsync ()).ReturnsAsync (Result<IList<ClientDAL>>.Success (clientsDAL));
            _mockMapper.Setup (m => m.Map<ICollection<ClientDTO>> (clientsDAL)).Returns(clientsDTO);

            var result = await _clientsController.GetClientsAsync ();

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (clientsDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientsAsync_WhereNoClientsFount_ReturnsNotFound ()
        {
            var clientsDAL = new List<ClientDAL> ();
            var error = "No clients found";
            _mockClientsRepository.Setup (m => m.GetClientsAsync ()).ReturnsAsync (Result<IList<ClientDAL>>.Failure (error));
            var notFound = new NotFoundObjectResult (error);

            var result = await _clientsController.GetClientsAsync ();

            Assert.IsInstanceOfType(result, typeof (NotFoundObjectResult));
            //Assert.AreEqual (notFound.Value, (result as NotFoundObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientWithIdAsync_WithValidId_ReturnsOkResult ()
        {
            var idArg = 2;
            var clientDAL = 
                new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };
            var clientDTO = 
                new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };

            _mockClientsRepository.Setup (repo => repo.GetClientWithIdAsync (idArg)).ReturnsAsync (Result<ClientDAL>.Success (clientDAL));
            _mockMapper.Setup (m => m.Map<ClientDTO> (clientDAL)).Returns (clientDTO);

            var result = await _clientsController.GetClientWithIdAsync (idArg);

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            //Assert.AreEqual (clientDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientWithIdAsync_WithNegativeId_ReturnsBadRequest ()
        {
            var idArg = -2;
            var error = "Invalid id";

            _mockClientsRepository.Setup (repo => repo.GetClientWithIdAsync (idArg)).ReturnsAsync (Result<ClientDAL>.Failure (error));
            var badRequest = new BadRequestObjectResult (error);

            var result = await _clientsController.GetClientWithIdAsync (idArg);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
            //Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientWithIdAsync_WithNoClientsAndPositiveId_ReturnsNotFound ()
        {
            var idArg = 1;
            var error = "No clients found";

            _mockClientsRepository.Setup (repo => repo.GetClientWithIdAsync (idArg)).ReturnsAsync (Result<ClientDAL>.Failure (error));
            var badRequest = new NotFoundObjectResult (error);

            var result = await _clientsController.GetClientWithIdAsync (idArg);

            Assert.IsInstanceOfType (result, typeof (NotFoundObjectResult));
            //Assert.AreEqual (badRequest.Value, (result as NotFoundObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientsWithNameAsync_WithValidName_ReturnsOkResult ()
        {
            var nameArg = "2FamName";
            var clientsDAL = new List<ClientDAL> () {
                new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test1FirstName", Residence = null, Id = 1 },
                new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 }};
            var clientsDTO = new List<ClientDTO> () {
                new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test1FirstName", Residence = null, Id = 1 },
                new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 }};

            _mockClientsRepository.Setup (repo => repo.GetClientsWithNameAsync (nameArg)).ReturnsAsync (Result<IList<ClientDAL>>.Success (clientsDAL));
            _mockMapper.Setup (m => m.Map<ICollection<ClientDTO>> (clientsDAL)).Returns (clientsDTO);

            var result = await _clientsController.GetClientsWithNameAsync (nameArg);

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (clientsDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientWithNameAsync_WithNameAsEmptyString_ReturnsBadRequest ()
        {
            var nameArg = String.Empty;
            var error = "Invalid name";

            _mockClientsRepository.Setup (repo => repo.GetClientsWithNameAsync (nameArg)).ReturnsAsync (Result<IList<ClientDAL>>.Failure (error));
            var badRequest = new BadRequestObjectResult (error);

            var result = await _clientsController.GetClientsWithNameAsync (nameArg);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
            //Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task GetClientWithNameAsync_WithNameAsNull_ReturnsBadRequest ()
        {
            string? nameArg = null;
            var error = "Invalid name";

            _mockClientsRepository.Setup (repo => repo.GetClientsWithNameAsync (nameArg)).ReturnsAsync (Result<IList<ClientDAL>>.Failure (error));
            var badRequest = new BadRequestObjectResult (error);

            var result = await _clientsController.GetClientsWithNameAsync (nameArg);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
            //Assert.AreEqual (badRequest.Value, (result as BadRequestObjectResult)?.Value);
        }
        [TestMethod ()]
        public async Task AddClientAsync_WithValidData_ReturnsCreatedAtActionResult ()
        {            
            var clientDTO = new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };
            var clientDAL = new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };
            var createdClientDAL = new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };
            var createdClientDTO = new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };

            _mockMapper.Setup (m => m.Map<ClientDAL> (clientDTO)).Returns (clientDAL);
            _mockClientsRepository.Setup (repo => repo.AddClientAsync (clientDAL)).ReturnsAsync (Result<ClientDAL>.Success (createdClientDAL));
            _mockMapper.Setup (m => m.Map<ClientDTO> (createdClientDAL)).Returns (createdClientDTO);

            var result = await _clientsController.Post (clientDTO);

            Assert.IsInstanceOfType (result, typeof (CreatedAtActionResult));
            Assert.AreEqual (createdClientDTO, (result as CreatedAtActionResult)?.Value);
        }
        [TestMethod ()]
        public async Task AddClientAsync_WithClientDTONull_ReturnsBadRequest ()
        {
            ClientDTO? clientDTO = null;
            var error = "Invalid client data";

            var result = await _clientsController.Post (clientDTO);

            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
        }
        [TestMethod ()]
        public async Task AddClientAsync_WhenRepoFails_Returns ()
        {
            var clientDTO = new ClientDTO { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };
            var clientDAL = new ClientDAL { FamilyName = "Test2FamName", FirstName = "Test2FirstName", Residence = null, Id = 2 };

            _mockMapper.Setup (m => m.Map<ClientDAL> (clientDTO)).Returns (clientDAL);
            _mockClientsRepository.Setup (repo => repo.AddClientAsync (clientDAL)).ReturnsAsync (Result<ClientDAL>.Failure ("Some Error"));

            var result = await _clientsController.Post (clientDTO);

            Assert.IsInstanceOfType (result, typeof (ObjectResult));
            Assert.AreEqual (500, (result as ObjectResult)?.StatusCode);
        }
        [TestMethod ()]
        public async Task Put_WithValidData_ReturnsOkResult ()
        {
            int id = 2;
            var clientDTO = new ClientDTO { FamilyName = "New2FamName", FirstName = "New2FirstName", Residence = null, Id = 2 };
            var clientDAL = new ClientDAL { FamilyName = "New2FamName", FirstName = "New2FirstName", Residence = null, Id = 2 };
            var updatedClientDAL = new ClientDAL { FamilyName = "New2FamName", FirstName = "New2FirstName", Residence = null, Id = 2 };
            var updatedClientDTO = new ClientDTO { FamilyName = "New2FamName", FirstName = "New2FirstName", Residence = null, Id = 2 };
            _mockMapper.Setup (m => m.Map<ClientDAL> (clientDTO)).Returns (clientDAL);
            _mockClientsRepository.Setup (repo => repo.GetClientWithIdAsync (id)).ReturnsAsync (Result<ClientDAL>.Success(clientDAL));
            _mockClientsRepository.Setup (repo => repo.UpdateClientAsync (id, clientDAL)).ReturnsAsync(Result<ClientDAL>.Success(updatedClientDAL));
            _mockMapper.Setup (m => m.Map<ClientDTO> (updatedClientDAL)).Returns (updatedClientDTO);

            var result = await _clientsController.Put (clientDTO, id, CancellationToken.None);

            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (updatedClientDTO, (result as OkObjectResult)?.Value);
        }
        [TestMethod]
        public async Task Put_WithNullClientDto_ReturnsBadRequest ()
        {
            // Arrange
            ClientDTO clientDto = null;
            var id = 1;
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _clientsController.Put (clientDto, id, cancellationToken);

            // Assert
            Assert.IsInstanceOfType (result, typeof (BadRequestObjectResult));
        }
        [TestMethod]
        public async Task Delete_WithValidId_ReturnsOkResult ()
        {
            // Arrange
            var id = 1;
            var client = new ClientDAL { Id = 1, FirstName = "John", FamilyName = "Doe" };
            var clientDTO = new ClientDTO { Id = 1, FirstName = "John", FamilyName = "Doe" };
            _mockClientsRepository.Setup (repo => repo.GetClientWithIdAsync (id))
                .ReturnsAsync (Result<ClientDAL>.Success (client));
            _mockClientsRepository.Setup (repo => repo.DeleteClientAsync (id))
                .ReturnsAsync (Result<ClientDAL>.Success (client));
            _mockMapper.Setup (m => m.Map<ClientDTO> (client)).Returns (clientDTO);
            // Act
            var result = await _clientsController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (OkObjectResult));
            Assert.AreEqual (clientDTO, (result as OkObjectResult)?.Value);
        }

        [TestMethod]
        public async Task Delete_WithNegativeId_ReturnsBadRequest ()
        {
            // Arrange
            var id = -1;

            // Act
            var result = await _clientsController.Delete (id);

            // Assert
            Assert.IsInstanceOfType (result, typeof (BadRequestResult));
        }
    }
}
