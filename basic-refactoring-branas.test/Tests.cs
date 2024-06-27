using Moq;
using basic_refactoring_branas.Dapper;
using System.Data;

namespace basic_refactoring_branas.test
{
    [TestFixture]
    public class Tests
    {
        private Mock<IDbConnection> _mockConnection;
        private Mock<IDatabase> _mockDatabase;

        [SetUp]
        public void Setup()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockDatabase = new Mock<IDatabase>();

            _mockDatabase.Setup(db => db.Connection).Returns(_mockConnection.Object);
        }

        [TestCase("97456321558")]
        [TestCase("71428793860")]
        [TestCase("87748248800")]
        public void ShouldValidateValidCpf(string cpf)
        {
            var isValid = CpfValidator.ValidateCpf(cpf);
            Assert.IsTrue(isValid);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("123456")]
        [TestCase("12345678901234567890")]
        [TestCase("11111111111")]
        public void ShouldValidateInvalidCpf(string cpf)
        {
            var isValid = CpfValidator.ValidateCpf(cpf);
            Assert.IsFalse(isValid);
        }

        [Test]
        public async Task SignupAsync_AccountAlreadyExists_ReturnsAlreadyExists()
        {
            // Arrange
            var account = new Account { Email = "test@example.com" };
            var existingAccount = account;

            _mockDatabase.Setup(db => db.QueryFirstOrDefaultAsync<Account>(It.IsAny<string>(), It.IsAny<object>()))
                         .ReturnsAsync(existingAccount);

            var manager = new UserManager(_mockDatabase.Object);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.AlreadyExists.Code));
        }

        [Test]
        public async Task SignupAsync_InvalidName_ReturnsInvalidName()
        {
            // Arrange
            var account = new Account { Name = "InvalidName", Email = "test@example.com" };
            var manager = new UserManager(_mockDatabase.Object);

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
            var manager = new UserManager(_mockDatabase.Object);

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
            var manager = new UserManager(_mockDatabase.Object);

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
            var manager = new UserManager(_mockDatabase.Object);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(SignupResult.InvalidCarPlate.Code));
        }

        [Test]
        public async Task SignupAsync_ValidAccount_ReturnsSuccess()
        {
            // Arrange
            var account = new Account
            {
                Name = "Valid Name",
                Email = "test@example.com",
                Cpf = "97456321558",
                IsDriver = false,
                IsPassenger = true
            };

            _mockDatabase.Setup(db => db.QueryFirstOrDefaultAsync<Account>(It.IsAny<string>(), It.IsAny<object>()))
                         .ReturnsAsync((Account)null);

            _mockDatabase.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>()))
                         .ReturnsAsync(1);

            var manager = new UserManager(_mockDatabase.Object);

            // Act
            var result = await manager.SignupAsync(account);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
            Assert.IsNotNull(result.AccountId);
        }
    }
}