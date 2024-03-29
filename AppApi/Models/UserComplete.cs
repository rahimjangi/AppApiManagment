﻿namespace AppApi.Models;

public class UserComplete
{

    public int UserId { get; set; }
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public bool Active { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal averageSalary { get; set; }
}
