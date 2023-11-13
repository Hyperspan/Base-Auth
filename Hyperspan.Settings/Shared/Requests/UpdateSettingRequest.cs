using System.ComponentModel.DataAnnotations;
using Hyperspan.Settings.Domain;
using Hyperspan.Settings.Shared.Modals;

namespace Hyperspan.Settings.Shared.Requests
{
    public class UpdateSettingRequest
    {
        [Required] public Guid Id { get; set; }

        [Required] public string Value { get; set; } = string.Empty;

        [Required] public SettingsType Type { get; set; } = SettingsType.None;
    }
}
