

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cv8_ASP.NET_v2.Models;

public class Dog
{
    // Primary Key
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Breed { get; set; }
    
    [Required]
    public string Sex { get; set; }
    
    [Required]
    [Range(0, 25)]
    public int Age { get; set; }
    
    public string? photoImg { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public int HarbarID { get; set; }

    
    public virtual Harbar Harbar { get; set; }
    
    
}