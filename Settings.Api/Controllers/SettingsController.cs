using System.Runtime.InteropServices;
using Hyperspan.Base.Shared.Modals;
using Hyperspan.Settings.Interfaces;
using Hyperspan.Settings.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Hyperspan.Settings.Api.Controllers
{
    [ApiController]
    [Route("api/settings`")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        [HttpGet("{id:Guid}")]
        public virtual async Task<ApiResponseModal> GetSettingValueAsync(Guid id)
            => await _settingService.GetSettingValue(id);

        [HttpGet("/get-settings-by-section/{label:string}")]
        public virtual async Task<ApiResponseModal> GetSettingValueAsync(string label)
            => await _settingService.GetSettingValue(label);

        [HttpPost]
        public virtual async Task<ApiResponseModal> UpdateSettings(UpdateSettingRequest id)
            => await _settingService.UpdateSettingValue(id);
    }
}
