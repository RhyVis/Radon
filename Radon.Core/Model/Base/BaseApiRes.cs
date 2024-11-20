using Radon.Common.Core.Extension;
using Radon.Common.Enums;

namespace Radon.Core.Model.Base;

/// <summary>
/// Base Api Response Model
/// </summary>
/// <typeparam name="D">Type of inner data</typeparam>
public abstract class BaseApiRes<D>()
{
    protected BaseApiRes(D data)
        : this()
    {
        Code = ResponseCodeType.SUCCESS.ToInt();
        Msg = ResponseCodeType.SUCCESS.ToString();
        Data = data;
    }

    protected BaseApiRes(int code, string msg, D data)
        : this()
    {
        Code = code;
        Msg = msg;
        Data = data;
    }

    /// <summary>
    /// Status Code
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Assigned Message
    /// </summary>
    public string Msg { get; set; } = "";

    /// <summary>
    /// Data
    /// </summary>
    public D Data { get; set; } = default!;
}
