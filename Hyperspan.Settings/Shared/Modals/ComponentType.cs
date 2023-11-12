// ReSharper disable InconsistentNaming

using System.ComponentModel;

namespace Hyperspan.Settings.Shared.Modals
{
    public enum ComponentType
    {
        None = 0,
        [Description("Date - 2 Digit Year")] Date_2DYear = 30,
        [Description("Date - 4 Digit Year")] Date_4DYear = 40,
        [Description("Time - Hours:Minutes")] Time_HHMM = 50,
        [Description("Time - Hours:Minutes:Seconds")] Time_HHMMSS = 60,
    }
}
