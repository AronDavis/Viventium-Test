using System.ComponentModel.DataAnnotations.Schema;

namespace Viventium.Data.Models
{
    public class Company
    {
        /// <summary>
        /// Company ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        /// The employees assigned to this company.
        /// </summary>
        public ICollection<Employee> Employees { get; set; }
    }
}
