using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Red_and_Green_boxes.Models;

namespace Red_and_Green_boxes.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Updated Index method with dynamic box data
    public IActionResult Index()
    {
        // Define the box data dynamically
        var boxes = new List<Box>
        {
            new Box { Name = "Box 1", Color = "green" },
            new Box { Name = "Box 2", Color = "red" },
            new Box { Name = "Box 3", Color = "blue" },
            new Box { Name = "Box 4", Color = "orange" },
            new Box { Name = "Box 5", Color = "purple" },
            new Box { Name = "Box 6", Color = "cyan" },
            new Box { Name = "Box 7", Color = "yellow" },
            new Box { Name = "Box 8", Color = "pink" },
            new Box { Name = "Box 9", Color = "brown" },
            new Box { Name = "Box 10", Color = "teal" },
            new Box { Name = "Box 11", Color = "lime" },
            new Box { Name = "Box 12", Color = "gray" }
        };

        return View(boxes);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

// Define the Box model inside the controller or move it to Models/Box.cs
public class Box
{
    public string Name { get; set; }
    public string Color { get; set; }
}

