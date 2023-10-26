using Hyperspan.Settings.Interfaces;
using Hyperspan.Settings.Shared.Requests;
using Hyperspan.Shared.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Hyperspan.Settings.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        [HttpGet("{id:Guid}")]
        public virtual async Task<ApiResponseModal> GetSettingValueAsync(Guid id)
            => await _settingService.GetSettingValue(id);

        [HttpGet("/get-settings-by-section/{label}")]
        public virtual async Task<ApiResponseModal> GetSettingValueAsync(string label)
            => await _settingService.GetSettingValue(label);

        [HttpPost]
        public virtual async Task<ApiResponseModal> UpdateSettings(UpdateSettingRequest id)
            => await _settingService.UpdateSettingValue(id);
    }
}
