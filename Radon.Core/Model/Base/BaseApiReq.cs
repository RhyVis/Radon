namespace Radon.Core.Model.Base;

/// <summary>
/// Base Api Request Model
/// </summary>
/// <typeparam name="D">Type of inner data</typeparam>
public abstract class BaseApiReq<D>()
    where D : class
{
    protected BaseApiReq(int code, string message, D data)
        : this()
    {
        Code = code;
        Meta = message;
        Data = data;
    }

    /// <summary>
    /// Status Code
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Assigned Message
    /// </summary>
    public string Meta { get; set; } = "";

    /// <summary>
    /// Data
    /// </summary>
    public D Data { get; init; } = null!;
}
