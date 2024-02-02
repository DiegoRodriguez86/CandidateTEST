using Microsoft.EntityFrameworkCore;

namespace CandidateTEST.Models.DB
{
    public class AppDBContext: DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
    }

    public class Employee
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string RFC { get; set; }
        public DateTime BornDate { get; set; }
        public EmployeeStatus Status { get; set; }
    }

    public enum EmployeeStatus
    {
        NotSet,
        Active,
        Inactive,
    }
}
