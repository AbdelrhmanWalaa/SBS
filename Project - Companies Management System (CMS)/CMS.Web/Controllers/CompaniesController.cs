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
        public async Task<IActionResult> List()
        {
            var companies = await dbContext.Companies.ToListAsync();

            return View(companies);
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
            var company = await dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.CompID == viewModel.CompID);

            if (company is not null)
            {
                dbContext.Companies.Remove(viewModel);

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Companies");
        }
    }
}
