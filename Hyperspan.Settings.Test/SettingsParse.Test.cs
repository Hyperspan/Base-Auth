using Hyperspan.Base.Domain.DbHelpers;
using Hyperspan.Settings.Domain;
using Hyperspan.Settings.Interfaces;
using Hyperspan.Settings.Services;
using Hyperspan.Settings.Shared.Modals;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Hyperspan.Settings.Test
{
    public class SettingsParse
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ParseSettingsValue()
        {
            const string testValueString1 = $@"{{{nameof(ComponentType.Date_4DYear)}}}";
            const string testValueString2 = $@"{{{nameof(ComponentType.Date_2DYear)}}}";

            const string expectedResult1 = "2023";
            const string expectedResult2 = "23";

            var repository = new Mock<IRepository<Guid, SettingsMaster, DbContext>>();
            var unitOfWork = new Mock<IUnitOfWork<Guid, SettingsMaster, DbContext>>();

            ISettingService service = new SettingsService(repository.Object, unitOfWork.Object);
            var result1 = await service.ParseCodeGenerationSettingsValue(testValueString1);
            var result2 = await service.ParseCodeGenerationSettingsValue(testValueString2);

            Assert.AreEqual(expectedResult1, result1);
            Assert.AreEqual(expectedResult2, result2);
        }
    }
}
