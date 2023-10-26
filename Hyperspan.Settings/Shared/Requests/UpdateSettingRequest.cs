using System.ComponentModel.DataAnnotations;

namespace Hyperspan.Settings.Shared.Requests
{
    public class UpdateSettingRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Value { get; set; } = string.Empty;
    }
}
