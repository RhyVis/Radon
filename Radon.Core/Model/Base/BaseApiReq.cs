namespace Radon.Core.Model.Base;

/// <summary>
/// Base Api Request Model
/// </summary>
/// <typeparam name="T">Type of inner data</typeparam>
public abstract class BaseApiReq<T>()
    where T : notnull
{
    protected BaseApiReq(T data)
        : this()
    {
        Data = data;
    }

    /// <summary>
    /// Data
    /// </summary>
    public T Data { get; init; } = default!;
}
