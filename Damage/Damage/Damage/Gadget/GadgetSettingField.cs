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
        Text,
        Checkbox,
        Radio,
        Color,
        Date,
        DateTime,
        DateTime_Local,
        Email,
        Month,
        Number,
        Tel,
        Time,
        Url,
        Week
    }
}