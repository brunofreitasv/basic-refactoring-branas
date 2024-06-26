namespace basic_refactoring_branas.test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
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
    }
}