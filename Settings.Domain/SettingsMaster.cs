using System.ComponentModel.DataAnnotations.Schema;
using Hyperspan.Base.Shared.Config;

namespace Hyperspan.Settings.Domain
{
    public class SettingsMaster : BaseEntity<Guid>
    {
        public SettingsMaster()
        {

        }

        public SettingsMaster(
            string settingLabel,
            string? settingValue,
            string defaultSettingValue,
            Guid? parentId = null)
        {
            Id = Guid.NewGuid();
            SettingLabel = settingLabel;
            SettingValue = settingValue;
            DefaultSettingValue = defaultSettingValue;
            ParentId = parentId;
        }

        public string SettingLabel { get; }
        public string? SettingValue { get; private set; }
        public string DefaultSettingValue { get; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; }
        public SettingsMaster? Parent { get; set; }


        public static string GetByLabelQuery(string label) =>
        @$"
            SELECT * FROM {nameof(SettingsMaster)} WHERE {nameof(SettingLabel)} = '{label}';
        ";

        public void UpdateSettings(string value)
        {
            SettingValue = value;
        }

    }
}
