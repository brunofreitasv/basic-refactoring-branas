namespace basic_refactoring_branas.Application.Models
{
    public class Account
    {
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string CarPlate { get; set; }
        public bool? IsPassenger { get; set; }
        public bool? IsDriver { get; set; }
    }
}
