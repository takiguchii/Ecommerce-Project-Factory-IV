using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(256)]
        public string? FullName { get; set; }

        [StringLength(14)]
        public string? DocumentCpfCnpj { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [StringLength(256)]
        public string? AddressLine { get; set; }

        [StringLength(100)]
        public string? AddressComplement { get; set; }

        [StringLength(100)]
        public string? Neighborhood { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }
    }
}