using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Web.Models;
using CMS.Web.Data;
using CMS.Web.Models.Entities;


namespace CMS.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public DepartmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDepartmentViewModel viewModel)
        {
            // Retrieve the company object corresponding to the provided CompID
            var company = await dbContext.Companies.FindAsync(viewModel.CompID);

            if (company == null)
            {
                // Handle the case where the company with the provided CompID does not exist
                return BadRequest("Invalid Company ID. Company not found.");
            }

            var department = new Department
            {
                Name = viewModel.Name,
                Company = company
            };

            await dbContext.Departments.AddAsync(department);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Departments");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var departments = await dbContext.Departments.ToListAsync();

            return View(departments);
        }

        [HttpGet]
        [Route("Departments/Edit/{DepartmentID}")]
        public async Task<IActionResult> Edit(Guid DepartmentID)
        {
            var department = await dbContext.Departments.FindAsync(DepartmentID);

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department viewModel)
        {
            var department = await dbContext.Departments.FindAsync(viewModel.CompID);

            if (department is not null)
            {
                department.Name = viewModel.Name;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Departments");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Department viewModel)
        {
            var department = await dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DepID == viewModel.DepID);

            if (department is not null)
            {
                dbContext.Departments.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Departments");
        }

        
        public async Task<IActionResult> Search(string searchString)
        {
            // Get all departments from the database
            var departments = await dbContext.Departments.ToListAsync();

            // Check the string
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter departments where the name starts with the search string (case-insensitive)
                departments = departments.Where(d => d.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            }

            // Pass the found departments to the view
            return View("List", departments);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortOrder)
        {
            // Get all departments from the database
            var departments = await dbContext.Departments.ToListAsync();

            // Set the ViewData to "name_desc" if sortOrder is null or empty, otherwise sets it to an empty string.
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Check the sort order
            if (sortOrder == "name_desc")
            {
                // Sort departments by name in descending order
                departments = departments.OrderByDescending(d => d.Name).ToList();
            }
            else
            {
                // Sort departments by name in ascending order by default
                departments = departments.OrderBy(d => d.Name).ToList();
            }

            // Pass the sorted departments to the view
            return View("List", departments);
        }
    }
}
