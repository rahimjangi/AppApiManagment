namespace AppApi.Models;

public partial class UserJobInfo
{
    public int UserId { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public UserJobInfo()
    {
        if (JobTitle == null)
        {
            JobTitle = string.Empty;
        }

        if (Department == null)
        {
            Department = string.Empty;
        }
    }
}
