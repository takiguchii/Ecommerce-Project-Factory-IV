using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string AppUserId { get; set; } 

        [ForeignKey("AppUserId")]
        public virtual IdentityUser AppUser { get; set; } 
        
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