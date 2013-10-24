using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Damage.Gadget
{
    public class GadgetSettingField
    {
        public string FieldName { get; set; }
        public SettingDataTypes DataType { get; set; }
    }

    public enum SettingDataTypes
    {
        Text = 1,
        Checkbox = 2,
        Radio = 3,
        Color = 4,
        Date = 5,
        DateTime_Local = 6,
        Email = 7,
        Month = 8,
        Number = 9,
        Tel = 10,
        Time = 11,
        Url = 12,
        Week = 13
    }
}