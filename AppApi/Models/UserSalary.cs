using Microsoft.Identity.Client;

namespace AppApi.Models;

public partial class UserSalary
{
    public int UserId { get; set; }
    public decimal Salary { get; set; }
    public decimal AvgSalary { get; set; }

}
