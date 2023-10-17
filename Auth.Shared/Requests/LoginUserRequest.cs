﻿using System.ComponentModel.DataAnnotations;

namespace Auth.Shared.Requests;

public class LoginUserRequest
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}
