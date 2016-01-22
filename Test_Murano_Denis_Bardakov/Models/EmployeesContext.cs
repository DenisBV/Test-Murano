using System.Data.Entity;

namespace Test_Murano_Denis_Bardakov.Models
{
    public class EmployeesContext : DbContext
    {
        public virtual DbSet<Employees> Employees { get; set; }
    }
}