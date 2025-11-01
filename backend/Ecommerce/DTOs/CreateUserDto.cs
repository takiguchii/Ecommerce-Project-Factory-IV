namespace Ecommerce.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string? FullName { get; set; }
        public string? DocumentCpfCnpj { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
        public string? AddressLine { get; set; }
        public string? AddressComplement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}