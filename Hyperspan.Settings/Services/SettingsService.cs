using System.Text.RegularExpressions;
using Hyperspan.Base.Domain.DbHelpers;
using Hyperspan.Settings.Domain;
using Hyperspan.Settings.Interfaces;
using Hyperspan.Settings.Shared.Modals;
using Hyperspan.Settings.Shared.Requests;
using Hyperspan.Settings.Shared.Responses;
using Hyperspan.Shared;
using Hyperspan.Shared.Modals;
using Microsoft.EntityFrameworkCore;

namespace Hyperspan.Settings.Services
{
    public class SettingsService : ISettingService
    {
        private readonly IRepository<Guid, SettingsMaster, DbContext> _repository;
        private readonly IUnitOfWork<Guid, SettingsMaster, DbContext> _unitOfWork;

        public SettingsService(
            IRepository<Guid, SettingsMaster, DbContext> repository,
            IUnitOfWork<Guid, SettingsMaster, DbContext> unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public virtual async Task<ApiResponseModal<SettingResponse>> GetSettingValue(Guid id)
        {
            try
            {
                // Get record by id
                var settings = await _repository.GetById(id);

                // check if record is null
                if (settings == null)
                {
                    // if record is null return
                    return await ApiResponseModal<SettingResponse>.FailedAsync(BaseErrorCodes.NullValue);
                }

                // Create new response object
                var response = new SettingResponse(
                    settings.Id,
                    settings.SettingLabel,
                    settings.SettingValue ?? settings.DefaultSettingValue
                );

                // Return queried object.
                return await Task.FromResult(await ApiResponseModal<SettingResponse>.SuccessAsync(response));
            }
            catch (ApiErrorException exception)
            {
                return await ApiResponseModal<SettingResponse>.FatalAsync(exception.ErrorCode);
            }
            catch
            {
                // Return exception occurred.
                return await ApiResponseModal<SettingResponse>.FatalAsync(BaseErrorCodes.UnknownSystemException);
            }

        }

        public virtual async Task<ApiResponseModal<SettingResponse>> GetSettingValue(string sectionLabel)
        {
            var result = await _repository.GetAllAsync(SettingsMaster.GetByLabelQuery(sectionLabel));
            var setting = result.FirstOrDefault() ?? throw new ApiErrorException(BaseErrorCodes.SettingNotFound);
            var settingResponse = new SettingResponse(
                setting.Id,
                setting.SettingValue ?? setting.DefaultSettingValue,
                setting.SettingLabel);

            return await ApiResponseModal<SettingResponse>.SuccessAsync(settingResponse);
        }

        public virtual async Task<ApiResponseModal<SettingResponse>> UpdateSettingValue(UpdateSettingRequest request)
        {
            var transaction = await _unitOfWork.StartTransaction();
            try
            {
                if (request.Type == SettingsType.None)
                    throw new ApiErrorException(BaseErrorCodes.SettingTypeInvalid);

                var settingRecord = await _repository.GetById(request.Id)
                                    ?? throw new ApiErrorException(BaseErrorCodes.SettingNotFound);

                if (request.Type == SettingsType.Code)
                {
                    request.Value = await ParseCodeGenerationSettingsValue(request.Value);
                }

                settingRecord.UpdateSettings(request.Value);

                await _repository.UpdateAsync(settingRecord);
                await transaction.CommitAsync();

                var settingResponse = new SettingResponse(
                    settingRecord.Id,
                    settingRecord.SettingValue ?? settingRecord.DefaultSettingValue,
                    settingRecord.SettingLabel);

                return await ApiResponseModal<SettingResponse>.SuccessAsync(settingResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return await ApiResponseModal<SettingResponse>.FatalAsync(ex);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public virtual async Task<ApiResponseModal<SettingResponse>> ResetValue(Guid id)
        {

            var transaction = await _unitOfWork.StartTransaction();
            try
            {
                var settingRecord = await _repository.GetById(id)
                                    ?? throw new ApiErrorException(BaseErrorCodes.SettingNotFound);
                settingRecord.UpdateSettings(settingRecord.DefaultSettingValue);

                await _repository.UpdateAsync(settingRecord);
                await transaction.CommitAsync();

                var settingResponse = new SettingResponse(
                    settingRecord.Id,
                    settingRecord.DefaultSettingValue,
                    settingRecord.SettingLabel);

                return await ApiResponseModal<SettingResponse>.SuccessAsync(settingResponse);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return await ApiResponseModal<SettingResponse>.FatalAsync(ex);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        public virtual async Task<string> ParseCodeGenerationSettingsValue(string settingValue)
        {
            var regex = new Regex(@"\{([^}]+)\}");
            var variablesList = regex.Matches(settingValue);

            var value = settingValue;
            var componentTypes = typeof(ComponentType);

            var moduleIteration = 0;
            var fieldIteration = 0;

            foreach (Match item in variablesList)
            {
                var componentType = componentTypes.GetEnumNames()
                    .FirstOrDefault(x => string.Equals(x, item.Groups[1].Value,
                        StringComparison.CurrentCultureIgnoreCase));

                switch (componentType)
                {
                    case nameof(ComponentType.Date_2DYear):
                        value = value.Replace($@"{{{componentType}}}", DateTime.UtcNow.ToString("yy"));
                        break;

                    case nameof(ComponentType.Date_4DYear):
                        value = value.Replace($@"{{{componentType}}}", DateTime.UtcNow.ToString("yyyy"));
                        break;

                    case nameof(ComponentType.Time_HHMM):
                        value = value.Replace($@"{{{componentType}}}", DateTime.UtcNow.ToString("HH:mm"));
                        break;

                    case nameof(ComponentType.Time_HHMMSS):
                        value = value.Replace($@"{{{componentType}}}", DateTime.UtcNow.ToString("HH:mm:ss"));
                        break;

                    default:
                        throw new ApiErrorException(BaseErrorCodes.SettingTypeInvalid,
                            $"No component '{componentType}' was recognized by system.");
                }
            }

            return value;
        }

    }
}
