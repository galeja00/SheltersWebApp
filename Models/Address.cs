using System.ComponentModel.DataAnnotations;

namespace cv8_ASP.NET_v2.Models;

public class Address
{
   // Primary Key
   public int Id { get; set; }
   
   // Data
   public string State { get; set; }
   
   [Required]
   public string City { get; set; }
   [Required]
   public string Street { get; set; }
   [Required]
   public int OrientationNumber { get; set; }
   [Required]
   public int DescriptiveNumber { get; set; }
    
   public virtual ICollection<Harbar> Harbers { get; set; }
}

