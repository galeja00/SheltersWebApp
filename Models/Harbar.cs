using System.ComponentModel.DataAnnotations;

namespace cv8_ASP.NET_v2.Models;

public class Harbar
{
    // Primary Key
    public int Id { get; set; }
    
    // Data
    [Required]
    public string Name { get; set; }
    [Required]
    public int MaxCapacity { get; set; }
    public int UseCapacity { get; set; }
    
    public string? LogoImg { get; set; }

    // Foreign Key
    public int AddressID { get; set; }
    
    public virtual Address Address { get; set; }
    
    public virtual ICollection<Dog> Dogs { get; set; }

    
}