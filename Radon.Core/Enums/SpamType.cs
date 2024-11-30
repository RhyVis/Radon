using System.ComponentModel;

namespace Radon.Core.Enums;

public enum SpamType
{
    [Description("未知")] N = 0,

    [Description("明日方舟")] AK = 1,

    [Description("原神")] GS = 2,

    [Description("麻辣")] ML = 3,

    [Description("普通喷子")] SN = 4,

    [Description("超级喷子")] SX = 5,

    [Description("反二")] AC = 6,

    [Description("低能")] DN = 7
}