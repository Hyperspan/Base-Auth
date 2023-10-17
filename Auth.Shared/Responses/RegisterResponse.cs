using System.ComponentModel.DataAnnotations;
using Auth.Shared.Enums;

namespace Auth.Shared.Responses;

public class RegisterResponse
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public RegistrationStages RegistrationStage { get; set; }
}
