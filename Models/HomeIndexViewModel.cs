using System.Collections.Generic;

namespace WebApplication1.Models;

public class HomeIndexViewModel
{
    public IList<Category> Categories { get; set; } = new List<Category>();
    public IList<Product> Products { get; set; } = new List<Product>();
}