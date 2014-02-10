using System;

namespace Damage.Gadget
{
    public class GadgetSettingField
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public SettingDataTypes DataType { get; set; }
        public Validators Validators { get; set; }
    }

    public enum SettingDataTypes
    {
        Text = 1,
        Checkbox = 2,
        Radio = 3,
        Color = 4,
        Date = 5,
        DateTime = 6,
        Email = 7,
        Month = 8,
        Number = 9,
        Telephone = 10,
        Time = 11,
        Url = 12,
        Week = 13,
        TextArea = 14
    }

    [Flags]
    public enum Validators
    {
        None = 0,
        Required = 1,
        Number = 2,
        Url = 4,
        Date = 8,
        DateISO = 16,
        Email = 32,
        Integer = 64
    }
}