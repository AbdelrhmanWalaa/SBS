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
            var employee = await dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.EmpID == viewModel.EmpID);

            if (employee is not null)
            {
                dbContext.Employees.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Employees");
        }
    }
}
