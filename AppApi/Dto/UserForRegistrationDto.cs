﻿namespace AppApi.Dto;

public partial class UserForRegistrationDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }=string.Empty;
    public string PasswordConfirm { get; set; } = string.Empty;
}
