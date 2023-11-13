using Hyperspan.Settings.Domain;
using Hyperspan.Settings.Shared.Requests;
using Hyperspan.Settings.Shared.Responses;
using Hyperspan.Shared.Modals;

namespace Hyperspan.Settings.Interfaces
{
    public interface ISettingService
    {
        Task<ApiResponseModal<SettingResponse>> GetSettingValue(Guid id);
        Task<ApiResponseModal<SettingResponse>> GetSettingValue(string sectionLabel);
        Task<ApiResponseModal<SettingResponse>> UpdateSettingValue(UpdateSettingRequest request);
        Task<ApiResponseModal<SettingResponse>> ResetValue(Guid id);
        string ParseCodeGenerationSettingsValue(string settingValue);
    }
}
