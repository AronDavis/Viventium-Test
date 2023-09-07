using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Viventium.Data;
using Viventium.Data.Models;
using Viventium.WebApi.Models;

namespace Viventium.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<DataStoreController> _logger;

        private readonly ViventiumContext _viventiumContext;

        public CompaniesController(ILogger<DataStoreController> logger, ViventiumContext context)
        {
            _logger = logger;
            _viventiumContext = context;
        }

        [HttpGet]
        public IEnumerable<CompanyHeader> Get()
        {
            return _viventiumContext.Companies
                .Include(c => c.Employees)
                .Select(_toCompanyHeader);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyWithEmployees?>> Get(int id)
        {
            Company? company = await _viventiumContext.Companies
                .Include(c => c.Employees)
                .SingleOrDefaultAsync(c => c.Id == id);

            if(company == null)
            {
                return NotFound(null);
            }

            return _toCompanyWithEmployees(company);
        }

        private CompanyHeader _toCompanyHeader(Company company)
        {
            return new CompanyHeader()
            {
                Id = company.Id,
                Code = company.Code,
                Description = company.Description,
                EmployeeCount = company.Employees.Count
            };
        }

        private EmployeeHeader _toEmployeeHeader(Employee employee)
        {
            return new EmployeeHeader()
            {
                Id = employee.EmployeeNumber,
                FullName = $"{employee.FirstName} {employee.LastName}"
            };
        }

        private CompanyWithEmployees _toCompanyWithEmployees(Company company)
        {
            return new CompanyWithEmployees()
            {
                Id = company.Id,
                Code = company.Code,
                Description = company.Description,
                EmployeeCount = company.Employees.Count,
                Employees = company.Employees.Select(_toEmployeeHeader).ToArray()
            };
        }

    }
}