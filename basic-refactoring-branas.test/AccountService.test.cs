using basic_refactoring_branas.Application;
using basic_refactoring_branas.Application.Models;
using basic_refactoring_branas.Resource;

namespace basic_refactoring_branas.test
{
    [TestFixture]
    public class AccountServiceTest
    {
        private IAccountDAO _fakeAccountDao;

        [SetUp]
        public void Setup()
        {
            _fakeAccountDao = new AccountDAOMemory();
        }

        [Test]
        public async Task SignupAsync_InvalidName_ReturnsInvalidName()
        {
            // Arrange
            var account = new Account { Name = "InvalidName", Email = "test@example.com" };
            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.InvalidName.Code));
        }

        [Test]
        public async Task SignupAsync_InvalidEmail_ReturnsInvalidEmail()
        {
            // Arrange
            var account = new Account { Name = "Valid Name", Email = "invalid-email" };
            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.InvalidEmail.Code));
        }

        [Test]
        public async Task SignupAsync_InvalidCpf_ReturnsInvalidCpf()
        {
            // Arrange
            var account = new Account { Name = "Valid Name", Email = "test@example.com", Cpf = "123456789" };
            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.InvalidCpf.Code));
        }

        [Test]
        public async Task SignupAsync_InvalidCarPlate_ReturnsInvalidCarPlate()
        {
            // Arrange
            var account = new Account { Name = "Valid Name", Email = "test@example.com", Cpf = "97456321558", IsDriver = true, CarPlate = "123ABC" };
            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.InvalidCarPlate.Code));
        }

        [Test]
        public async Task SignupAsync_Passenger_ReturnsSuccess()
        {
            // Arrange
            var account = new Account
            {
                Name = "Valid Name",
                Email = "test@example.com",
                Cpf = "97456321558",
                IsPassenger = true
            };

            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.IsSuccessCode, Is.EqualTo(true));
            Assert.IsNotNull(result.AccountId);

            // Act
            var outputAccout = await manager.GetAccontAsync(result.AccountId);

            // Assert
            Assert.IsNotNull(outputAccout);
            Assert.That(outputAccout.Name, Is.EqualTo(account.Name));
            Assert.That(outputAccout.Email, Is.EqualTo(account.Email));
            Assert.That(outputAccout.Cpf, Is.EqualTo(account.Cpf));
        }

        [Test]
        public async Task SignupAsync_Driver_ReturnsSuccess()
        {
            // Arrange
            var account = new Account
            {
                Name = "Valid Name",
                Email = "test@example.com",
                Cpf = "97456321558",
                CarPlate = "AAA9999",
                IsDriver = true
            };

            var manager = new AccountService(_fakeAccountDao);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.IsSuccessCode, Is.EqualTo(true));
            Assert.IsNotNull(result.AccountId);

            // Act
            var outputAccout = await manager.GetAccontAsync(result.AccountId);

            // Assert
            Assert.IsNotNull(outputAccout);
            Assert.That(outputAccout.Name, Is.EqualTo(account.Name));
            Assert.That(outputAccout.Email, Is.EqualTo(account.Email));
            Assert.That(outputAccout.Cpf, Is.EqualTo(account.Cpf));
            Assert.That(outputAccout.CarPlate, Is.EqualTo(account.CarPlate));
        }

        [Test]
        public async Task SignupAsync_AccountAlreadyExists_ReturnsAlreadyExists()
        {
            // Arrange
            var account = new Account
            {
                Name = "Valid Name",
                Email = "test@example.com",
                Cpf = "97456321558",
                IsPassenger = true
            };
            var manager = new AccountService(_fakeAccountDao);

            // Act
            await manager.SignupAsync(account);
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.AlreadyExists.Code));
        }
    }
}