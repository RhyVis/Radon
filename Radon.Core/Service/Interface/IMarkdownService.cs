using Radon.Core.Data.Entity;
using Radon.Core.Model.Response;

namespace Radon.Core.Service.Interface;

public interface IMarkdownService
{
    MdIndex CheckContent(string path);

    /// <summary>
    /// Read markdown content from the given path.
    /// </summary>
    /// <param name="path">name under appResource/md</param>
    MdRecord ProvideContent(string path);

    /// <summary>
    /// Update content of the markdown file.
    /// </summary>
    /// <param name="path">null if going to create new one</param>
    /// <param name="name"></param>
    /// <param name="desc"></param>
    /// <param name="content"></param>
    /// <returns>Empty if the path exists, or the inserted path</returns>
    string UpdateContent(string? path, string name, string desc, string content);

    /// <summary>
    /// Delete the markdown file, throw if any error occurs.
    /// </summary>
    void DeleteContent(string path);

    List<MdIndex> ListIndex();
}
