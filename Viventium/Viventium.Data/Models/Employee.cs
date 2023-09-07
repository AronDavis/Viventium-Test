using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Viventium.Data.Models
{
    public class Employee
    {
        /// <summary>
        /// Employee Number
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// Employee First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Employee Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Employee Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Employee Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Employee Hire Date
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Manager Employee Number
        /// </summary>
        [ForeignKey("Employee")]
        public string? ManagerEmployeeNumber { get; set; }

        /// <summary>
        /// The ID of the company this employee belongs to.
        /// </summary>
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
    }
}
