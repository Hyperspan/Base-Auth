using System.ComponentModel.DataAnnotations.Schema;
using Hyperspan.Settings.Shared.Modals;
using Hyperspan.Shared.Config;

namespace Hyperspan.Settings.Domain
{
    public class SettingsMaster : BaseEntity<Guid>
    {
        public SettingsMaster()
        {
            SettingLabel = string.Empty;
            SettingValue = string.Empty;
            DefaultSettingValue = string.Empty;
            Type = SettingsType.None;
            ParentId = null;
        }

        public SettingsMaster(
            string settingLabel,
            string? settingValue,
            string defaultSettingValue,
            SettingsType type, Guid? parentId = null)
        {
            Id = Guid.NewGuid();
            SettingLabel = settingLabel;
            SettingValue = settingValue;
            DefaultSettingValue = defaultSettingValue;
            ParentId = parentId;
            Type = type;
        }

        public string SettingLabel { get; }
        public string DefaultSettingValue { get; }
        public string? SettingValue { get; private set; }
        public SettingsType Type { get; set; }

        [ForeignKey(nameof(Parent))] public Guid? ParentId { get; }
        public SettingsMaster? Parent { get; set; }


        public static string GetByLabelQuery(string label, Guid? parentId = null) =>
            @$"
                SELECT * FROM ""Settings"".""{nameof(SettingsMaster)}""
                    WHERE ""{nameof(SettingLabel)}"" = '{label}' 
                        {(parentId.HasValue && parentId.Value != Guid.Empty ?
                                $@"AND ""{nameof(ParentId)}"" = '{parentId}'" : "")};
            ";

        public void UpdateSettings(string value)
        {
            SettingValue = value;
        }

    }
}
