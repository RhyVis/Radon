using System.ComponentModel;

namespace Radon.Core.Enums;

public enum DictType
{
    [Description("未知")] NONE = 0,

    [Description("抽象")] NMSL = 1,

    [Description("简繁")] TRAD = 2,

    [Description("火星")] SPRK = 3,

    [Description("差异")] DIFF = 4
}