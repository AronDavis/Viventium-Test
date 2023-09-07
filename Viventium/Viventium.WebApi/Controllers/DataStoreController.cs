using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Viventium.Data;
using Viventium.Data.Models;

namespace Viventium.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataStoreController : ControllerBase
    {
        private readonly ILogger<DataStoreController> _logger;

        private readonly ViventiumContext _viventiumContext;

        public DataStoreController(ILogger<DataStoreController> logger, ViventiumContext context)
        {
            _logger = logger;
            _viventiumContext = context;
        }

        [HttpPost]
        public async Task<string> Post()
        {
            if(!Request.HasFormContentType)
            {
                return "Request does not have form content type.";
            }

            //check that there is exactly one file attached
            if(Request.Form.Files.Count != 1)
            {
                return "There should be exactly one CSV file attached.";
            }

            //delete tables
            await _viventiumContext.Companies.ExecuteDeleteAsync();
            await _viventiumContext.Employees.ExecuteDeleteAsync();

            IFormFile csvFile = Request.Form.Files[0];

            using (TextFieldParser csvParser = new TextFieldParser(csvFile.OpenReadStream()))
            {
                csvParser.SetDelimiters(",");

                //skip column names
                csvParser.ReadLine();

                HashSet<int> companyIds = new HashSet<int>();
                HashSet<string> employeeNumbers = new HashSet<string>();

                int currentLine = 1;
                while (!csvParser.EndOfData)
                {
                    string[]? fields = csvParser.ReadFields();
                    if (fields == null)
                    {
                        return $"Line {currentLine} was empty";
                    }

                    if(fields.Length != 10)
                    {
                        return $"Line {currentLine} had the incorrect number of fields.";
                    }

                    if(!int.TryParse(fields[0], out int companyId))
                    {
                        return $"Line {currentLine} had an incorrect Company ID.";
                    }

                    //if the company has not been added yet
                    if(!companyIds.Contains(companyId))
                    {
                        string companyCode = fields[1];
                        string companyDescription = fields[2];

                        Company company = new Company()
                        {
                            Id = companyId,
                            Code = companyCode,
                            Description = companyDescription
                        };

                        //add the company
                        companyIds.Add(companyId);
                        _viventiumContext.Companies.Add(company);
                    }

                    string employeeNumber = fields[3];

                    if (!employeeNumbers.Contains(employeeNumber))
                    {
                        string employeeFirstName = fields[4];
                        string employeeLastName = fields[5];
                        string employeeEmail = fields[6];
                        string employeeDepartment = fields[7];


                        if(!_tryGetNullable(fields[8], out DateTime? employeeHireDate))
                        {
                            return $"Line {currentLine} had an incorrect Hire Date.";
                        }

                        string? employeeManagerEmployeeNumber = fields[9];

                        if (employeeManagerEmployeeNumber == string.Empty)
                        {
                            employeeManagerEmployeeNumber = null;
                        }

                        Employee employee = new Employee()
                        {
                            EmployeeNumber = employeeNumber,
                            FirstName = employeeFirstName,
                            LastName = employeeLastName,
                            Email = employeeEmail,
                            Department = employeeDepartment,
                            HireDate = employeeHireDate,
                            ManagerEmployeeNumber = employeeManagerEmployeeNumber,
                            CompanyId = companyId
                        };

                        //add the employee
                        employeeNumbers.Add(employeeNumber);
                        _viventiumContext.Add(employee);
                    }
                }
            }

            _viventiumContext.SaveChanges();
            return "success";
        }

        private bool _tryGetNullable(string raw, out DateTime? value)
        {
            if (raw == string.Empty)
            {
                value = null;
                return true;
            }

            if (DateTime.TryParse(raw, out DateTime tempValue))
            {
                value = tempValue;
                return true;
            }

            value = null;
            return false;
        }
    }
}