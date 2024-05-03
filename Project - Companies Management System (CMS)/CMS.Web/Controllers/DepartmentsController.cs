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
            var department = await dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.DepID == viewModel.DepID);

            if (department is not null)
            {
                dbContext.Departments.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Departments");
        }
    }
}
