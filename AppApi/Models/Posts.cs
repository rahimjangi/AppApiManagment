﻿namespace AppApi.Models;

public partial class Posts
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string PostTitle { get; set; } = string.Empty;
    public string PostContent { get; set; } = string.Empty;
    public DateTime PostCreate { get; set; }
    public DateTime PostUpdated { get; set; }
}
