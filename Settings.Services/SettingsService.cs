using Hyperspan.Base.Database;
using Hyperspan.Base.Database.DbHelpers;
using Hyperspan.Base.Shared;
using Hyperspan.Base.Shared.Modals;
using Hyperspan.Settings.Domain;
using Hyperspan.Settings.Interfaces;
using Hyperspan.Settings.Shared.Requests;
using Hyperspan.Settings.Shared.Responses;

namespace Hyperspan.Settings.Services
{
    public class SettingsService : ISettingService
    {
        private readonly IRepository<Guid, SettingsMaster, Contexts> _repository;
        private readonly IUnitOfWork<SettingsMaster, Guid, Contexts> _unitOfWork;

        public SettingsService(
            IRepository<Guid, SettingsMaster, Contexts> repository,
            IUnitOfWork<SettingsMaster, Guid, Contexts> unitOfWork
        )
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
                var settingRecord = await _repository.GetById(request.Id)
                                    ?? throw new ApiErrorException(BaseErrorCodes.SettingNotFound);
                settingRecord.UpdateSettings(request.Value);

                await _repository.UpdateAsync(settingRecord);
                await transaction.CommitAsync();

                var settingResponse = new SettingResponse(
                    settingRecord.Id,
                    settingRecord.SettingValue ?? settingRecord.DefaultSettingValue,
                    settingRecord.SettingLabel);

                return await ApiResponseModal<SettingResponse>.SuccessAsync(settingResponse);
            }
            catch
            {
                await transaction.RollbackAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
            }

            return null;
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
            catch
            {
                await transaction.RollbackAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
            }

            return null;
        }
    }
}
