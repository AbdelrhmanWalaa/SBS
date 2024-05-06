using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS.Web.Models;
using CMS.Web.Data;
using CMS.Web.Models.Entities;


namespace CMS.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel viewModel)
        {
            // Retrieve the company object corresponding to the provided CompID
            var company = await dbContext.Companies.FindAsync(viewModel.CompID);

            if (company == null)
            {
                // Handle the case where the company with the provided CompID does not exist
                return BadRequest("Invalid Company ID. Company not found.");
            }

            var employee = new Employee
            {
                Name = viewModel.Name,
                Position = viewModel.Position,
                Account = viewModel.Account,
                PhoneNumber = viewModel.PhoneNumber,
                Address = viewModel.Address,
                Company = company
            };

            await dbContext.Employees.AddAsync(employee);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Employees");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var employees = await dbContext.Employees.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        [Route("Employees/Edit/{EmployeeID}")]
        public async Task<IActionResult> Edit(Guid EmployeeID)
        {
            var employee = await dbContext.Employees.FindAsync(EmployeeID);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee viewModel)
        {
            var employee = await dbContext.Employees.FindAsync(viewModel.EmpID);

            if (employee is not null)
            {
                employee.Name = viewModel.Name;
                employee.Position = viewModel.Position;
                employee.Account = viewModel.Account;
                employee.PhoneNumber = viewModel.PhoneNumber;
                employee.Address = viewModel.Address;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Employees");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Employee viewModel)
        {
            var employee = await dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmpID == viewModel.EmpID);

            if (employee is not null)
            {
                dbContext.Employees.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Employees");
        }

        
        public async Task<IActionResult> Search(string searchString)
        {
            // Get all employees from the database
            var employees = await dbContext.Employees.ToListAsync();

            // Check the string
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter employees where the name starts with the search string (case-insensitive)
                employees = employees.Where(e => e.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            }

            // Pass the found employees to the view
            return View("List", employees);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortOrder)
        {
            // Get all employees from the database
            var employees = await dbContext.Employees.ToListAsync();

            // Set the ViewData to "name_desc" if sortOrder is null or empty, otherwise sets it to an empty string.
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Check the sort order
            if (sortOrder == "name_desc")
            {
                // Sort employees by name in descending order
                employees = employees.OrderByDescending(e => e.Name).ToList();
            }
            else
            {
                // Sort employees by name in ascending order by default
                employees = employees.OrderBy(e => e.Name).ToList();
            }

            // Pass the sorted employees to the view
            return View("List", employees);
        }
    }
}
