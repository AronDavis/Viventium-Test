namespace Viventium.WebApi.Models
{
    public class CompanyWithEmployees : CompanyHeader
    {
        /// <summary>
        /// The Employees in the company.
        /// </summary>
        public EmployeeHeader[] Employees { get; set; }
    }
}
