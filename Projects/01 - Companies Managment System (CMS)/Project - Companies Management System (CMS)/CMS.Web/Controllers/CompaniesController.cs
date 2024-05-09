using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Web.Models;
using CMS.Web.Data;
using CMS.Web.Models.Entities;


namespace CMS.Web.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CompaniesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;     
        }

        [HttpGet]
        public async Task<IActionResult> List(int pageNumber)
        {
            // Define the page size
            int pageSize = 10;

            // Get all companies from the database
            var companies = await dbContext.Companies.ToListAsync();

            // Calculate total number of items
            int totalItems = companies.Count;

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Ensure pageNumber is within valid range
            pageNumber = Math.Max(1, Math.Min(totalPages, pageNumber));

            // Retrieve items for the current page
            var items = companies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Pass the retrieved items, page number, and total pages to the view
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            // Pass the view model to the view
            return View(items);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCompanyViewModel viewModel)
        {
            var company = new Company
            {
                Name = viewModel.Name,
                Address = viewModel.Address
            };

            await dbContext.Companies.AddAsync(company);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Companies");
        }

        [HttpGet]
        [Route("Companies/Edit/{CompanyID}")]
        public async Task<IActionResult> Edit(Guid CompanyID)
        {
            var company = await dbContext.Companies.FindAsync(CompanyID);

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Company viewModel)
        {
            var company = await dbContext.Companies.FindAsync(viewModel.CompID);

            if (company is not null)
            {
                company.Name = viewModel.Name;
                company.Address = viewModel.Address;

                await dbContext.SaveChangesAsync();
            }            

            return RedirectToAction("List", "Companies");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Company viewModel)
        {
            var company = await dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.CompID == viewModel.CompID);

            if (company is not null)
            {
                dbContext.Companies.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Companies");
        }

        
        public async Task<IActionResult> Search(string searchString)
        {
            // Get all companies from the database
            var companies = await dbContext.Companies.ToListAsync();

            // Check the string
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter companies where the name starts with the search string (case-insensitive)
                companies = companies.Where(c => c.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            }

            // Pass the found companies to the view
            return View("List", companies);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortOrder)
        {
            // Get all companies from the database
            var companies = await dbContext.Companies.ToListAsync();

            // Set the ViewData to "name_desc" if sortOrder is null or empty, otherwise sets it to an empty string.
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Check the sort order
            if (sortOrder == "name_desc")
            {
                // Sort companies by name in descending order
                companies = companies.OrderByDescending(c => c.Name).ToList();
            }
            else
            {
                // Sort companies by name in ascending order by default
                companies = companies.OrderBy(c => c.Name).ToList();
            }

            // Pass the sorted companies to the view
            return View("List", companies);
        }
    }
}
