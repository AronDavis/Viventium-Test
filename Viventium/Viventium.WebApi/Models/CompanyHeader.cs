namespace Viventium.WebApi.Models
{
    public class CompanyHeader
    {
        /// <summary>
        /// Company ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company Code
        /// </summary>
        public string Code { get; set; }
                
        /// <summary>
        /// Company Description
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Number of Employees in the company.
        /// </summary>
        public int EmployeeCount { get; set; }
    }
}
