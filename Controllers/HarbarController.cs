using Microsoft.AspNetCore.Mvc;
using cv8_ASP.NET_v2.Models;

namespace cv8_ASP.NET_v2.Controllers;

public class HarbarController : BaseController
{
    
    public IActionResult All()
    {
        foreach (Harbar h in Hbc.Harbars)
        {
            if (h.Dogs.Count != h.UseCapacity)
            {
                h.UseCapacity = h.Dogs.Count;
            }
        }

        Hbc.SaveChanges();
        List<Harbar> harbars = Hbc.Harbars.ToList();
        return View(harbars);
    }

    public IActionResult Info(int id)
    {
        Harbar? h = Hbc.Harbars.Find(id);
        if (h != null)
        {
            return View(h);
        }
        return NotFound("Nekdo ho uz ukradl");
    }

    [HttpPost]
    public IActionResult Search(int moduleId, string nameOfDog)
    {
        Harbar? h = Hbc.Harbars.Find(moduleId);
        List<Dog> d = Hbc.Dogs.Where(d => d.HarbarID == moduleId && d.Name.ToLower().Contains(nameOfDog.ToLower())).ToList();
        if (h == null)
        {
            return NotFound("Dany utulek už neexistuje");
        }

        ViewBag.dogs = d;
        return View("Info", h);

    }


    public IActionResult SelectHarbars(string nameOfHarbar)
    {

        List<Harbar> h = Hbc.Harbars.ToList();
        List<Harbar> x = h.Where(h => h.Name.ToLower().Contains(nameOfHarbar.ToLower())).ToList();
        if (x != null)
        {
            return View("All",x);
        }

        return View("All", Hbc.Harbars.ToList());
    } 
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Harbar hForm)
    {
        if (string.IsNullOrEmpty(hForm.Name))
        {
            ModelState.AddModelError("Name", "Jmeno musí být vyplněné");
            return RedirectToAction("Add");
        }
        Address a = hForm.Address;
        int aId = Hbc.Addresses.Count();
        if (Hbc.Addresses.Find(aId) != null)
        {
            aId++;
        }

        a.State = "Česko";
        a.Id = aId;
        int hId = Hbc.Harbars.Count();
        if (Hbc.Harbars.Find(hId) != null)
        {
            hId++;
        }
        hForm.Id = hId;
        hForm.UseCapacity = 0;
        hForm.Address = a;
        Hbc.Addresses.Add(a);
        Hbc.Harbars.Add(hForm);
        Hbc.SaveChanges();
        return RedirectToAction("All");
    }
}