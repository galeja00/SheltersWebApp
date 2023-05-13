using cv8_ASP.NET_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;


namespace cv8_ASP.NET_v2.Controllers;

public class BaseController : Controller
{
    protected HarborContext Hbc { get; set; }

    public BaseController()
    {
        Hbc = new HarborContext();
        
    }

    
}