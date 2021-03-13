namespace Timesheet.Domain.Models
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class StaffEmployee
    {
     

        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public StaffEmployee(string lastName, decimal salary)
        {
            LastName = lastName; //?? throw new ArgumentNullException(nameof(lastName));
            Salary = salary;
        }
    }
}
