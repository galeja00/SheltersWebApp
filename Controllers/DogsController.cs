using System.Web.Http;
using Castle.DynamicProxy;
using cv8_ASP.NET_v2.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace cv8_ASP.NET_v2.Controllers;
//Tento object je pro API, protože při vytváření JSONu s objektem Dog, tak se vytvoří i objekt Harbar, který obsahuje všechny psy v daném útulku. Takže to hodilo Error kvůli zalůpování
public class DogApiObj
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Breed { get; set; }
    
    public string Sex { get; set; }
    
    public int Age { get; set; }
    
    public string? photoImg { get; set; }
    
    public string Description { get; set; }
    
    public int HarbarID { get; set; }
    
}

public class DogsController : BaseController
{
    private IWebHostEnvironment environment;

    public DogsController(IWebHostEnvironment env)
    {
        environment = env;
    }

    public IActionResult All()
    {
        //List<Dog> d = Hbc.Dogs.ToList();
        
        return View("All");
    }

    public IActionResult Info(int id)
    {
        Dog? d = Hbc.Dogs.Find(id);
        if (d != null)
        {
            return View(d);
        }

        return NotFound("Tento pes uz neexistuje");
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult AddForm(int id)
    {
        if (id == -1)
        {
            List<Harbar> hList = Hbc.Harbars.ToList();
            ViewBag.ListHarbars = hList;
            return View();
        }
        Harbar? h = Hbc.Harbars.Find(id);
        if (h != null)
        {
            ViewBag.Harbar = h;
            return View(); 
        }
        return NotFound("Tento utulek uz neexistuje");  
    }
    

    

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public IActionResult Add(Dog dogForm)
    {

        int i = 1;
        while (Hbc.Dogs.Find(i) != null)
        {
            i++;
        }

        dogForm.Id = i;
        Harbar? h = Hbc.Harbars.Find(dogForm.HarbarID);
        if (h != null)
        {
            dogForm.Harbar = h;
            h.UseCapacity++;
            h.Dogs.Add(dogForm);
        
            
            Hbc.Dogs.Add(dogForm);
            Hbc.SaveChanges(); 
            return RedirectToAction("AddPhoto", dogForm);
        } 
        
        return View("AddForm");
    }
    
    public IActionResult Remove(int dogId)
    {
        
        Dog? d = Hbc.Dogs.Find(dogId);
        Harbar? h = Hbc.Harbars.Find(d.HarbarID);
        if (d != null && h != null)
        {
            h.UseCapacity--;
            if (d.photoImg != null && d.photoImg != "")
            {
                string projectFolder = environment.WebRootPath;
                string pathToFile = Path.Combine(projectFolder, "images", "dogs", d.photoImg);
                if (System.IO.File.Exists(pathToFile))
                {
                    System.IO.File.Delete(pathToFile);
                }
            }
            Hbc.Dogs.Remove(d);
            Hbc.SaveChanges();
            return View(h);
        }

        if (h != null)
        {
            return RedirectToAction("All");
        }
        else
        {
            return RedirectToAction("All", "Harbar");
        }
    }

    
    public IActionResult SearchInDogs(string dogName)
    {
        List<Dog> d = Hbc.Dogs.Where(d => d.Name.ToLower().Contains(dogName.ToLower())).ToList();
        return View("All", d);
    }

    

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult AddPhoto(Dog dog)
    {
        Dog d = Hbc.Dogs.Find(dog.Id);
        if (d != null)
        {
            return View(dog); 
        }

        return View("All");
    }
    

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public IActionResult AddPhoto(int dogId)
    {
        IFormFile? photo = HttpContext.Request.Form.Files.GetFile("dogPhoto");
        Dog d = Hbc.Dogs.Find(dogId);
        
        
        if (d == null)
        {
            return RedirectToAction("All");
        }

        if (photo.Length < 5000000)
        {
            ModelState.AddModelError("dogPhoto", "Moc velká velikost souboru, musi byt menší než 5MB.");
            return RedirectToAction("AddPhoto", d);
        }
        
        string[] types = { ".jpeg", ".jpg", ".png" };
        string extension = Path.GetExtension(photo.FileName).ToLower();
        if (!types.Contains(extension))
        {
            ModelState.AddModelError("dogPhoto", "Špatný typ souboru. Jenom JPG, PNG soubory jsou povolene.");
            return RedirectToAction("AddPhoto", d);
        }

        if (!photo.ContentType.StartsWith("image/"))
        {
            ModelState.AddModelError("dogPhoto", "Špatný typ souboru. Soubor musí být obrázek.");
            return RedirectToAction("AddPhoto", d);
        }
        
        
        string fileName = Guid.NewGuid() + extension;
        string projectFolder = environment.WebRootPath;
        string pathToFile = Path.Combine(projectFolder, "images", "dogs", fileName);
        
        using (FileStream fs = new FileStream(pathToFile, FileMode.Create))
        {
            photo.CopyTo(fs);
        }

        
        d.photoImg = fileName;
        Hbc.SaveChanges();
        
        return RedirectToAction("All");
        
    }
    
    //REST API for search in dogs
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IEnumerable<DogApiObj> Search(string dogName)
    {
        
        if (dogName == "")
        {
            return Hbc.Dogs.Select(d => new DogApiObj() { Name = d.Name, Age = d.Age, Id = d.Id, photoImg = d.photoImg, HarbarID = d.HarbarID, Breed = d.Breed, Sex = d.Sex, Description = d.Description}).ToList();
        }
        var dogs = Hbc.Dogs.Where(d => d.Name.ToLower().Contains(dogName.ToLower()))
            .Select(d =>  new DogApiObj() { Name = d.Name, Age = d.Age, Id = d.Id, photoImg = d.photoImg, HarbarID = d.HarbarID, Breed = d.Breed, Sex = d.Sex, Description = d.Description}).ToList();
        if (dogs == null)
        {
            return new List<DogApiObj>();
        }
        
        return dogs;
    }
    
}

