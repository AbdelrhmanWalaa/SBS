using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Web.Models;
using CMS.Web.Data;
using CMS.Web.Models.Entities;


namespace CMS.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel viewModel)
        {
            // Retrieve the company object corresponding to the provided CompID
            var company = await dbContext.Companies.FindAsync(viewModel.CompID);

            if (company == null)
            {
                // Handle the case where the company with the provided CompID does not exist
                return BadRequest("Invalid Company ID. Company not found.");
            }

            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                Company = company
            };

            await dbContext.Products.AddAsync(product);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Products");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var products = await dbContext.Products.ToListAsync();

            return View(products);
        }

        [HttpGet]
        [Route("Products/Edit/{ProductID}")]
        public async Task<IActionResult> Edit(Guid ProductID)
        {
            var product = await dbContext.Products.FindAsync(ProductID);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product viewModel)
        {
            var product = await dbContext.Products.FindAsync(viewModel.ProdID);

            if (product is not null)
            {
                product.Name = viewModel.Name;
                product.Description = viewModel.Description;
                product.Price = viewModel.Price;             

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product viewModel)
        {
            var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProdID == viewModel.ProdID);

            if (product is not null)
            {
                dbContext.Products.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Products");
        }

        
        public async Task<IActionResult> Search(string searchString)
        {
            // Get all products from the database
            var products = await dbContext.Products.ToListAsync();

            // Check the string
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter products where the name starts with the search string (case-insensitive)
                products = products.Where(p => p.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            }

            // Pass the found products to the view
            return View("List", products);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortOrder)
        {
            // Get all products from the database
            var products = await dbContext.Products.ToListAsync();

            // Set the ViewData to "name_desc" if sortOrder is null or empty, otherwise sets it to an empty string.
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Check the sort order
            if (sortOrder == "name_desc")
            {
                // Sort products by name in descending order
                products = products.OrderByDescending(p => p.Name).ToList();
            }
            else
            {
                // Sort products by name in ascending order by default
                products = products.OrderBy(p => p.Name).ToList();
            }

            // Pass the sorted products to the view
            return View("List", products);
        }
    }
}
