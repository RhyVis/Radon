namespace Radon.Core.Model.Base;

/// <summary>
/// Base Api Request Model
/// </summary>
/// <typeparam name="D">Type of inner data</typeparam>
public abstract class BaseApiReq<D>()
    where D : class
{
    protected BaseApiReq(D data)
        : this()
    {
        Data = data;
    }

    /// <summary>
    /// Data
    /// </summary>
    public D Data { get; init; } = null!;
}
