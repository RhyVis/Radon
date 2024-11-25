using Masuit.Tools;
using NLog;
using Radon.Common.Core.DI;
using Radon.Core.Data.Entity;
using Radon.Core.Data.Repository;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceScoped]
public class MarkdownService(MdIndexRepository repo) : IMarkdownService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private const string NoContent = "### No content available.";
    private const string Void = "void";

    public MdIndex CheckContent(string path)
    {
        return repo.FindByPath(path) ?? new MdIndex();
    }

    public MdRecord ProvideContent(string path)
    {
        try
        {
            var (name, desc, content) = PathRead(path);
            return new MdRecord(name, desc, content);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Exception occurred on reading markdown file on {path}.");
            throw;
        }
    }

    public string UpdateContent(string? path, string name, string desc, string content)
    {
        try
        {
            return path.IsNullOrEmpty()
                ? PathWrite(null, name, desc, content)
                : PathWrite(path, name, desc, content);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Exception occurred on updating markdown file on {path}.");
            throw;
        }
    }

    public void DeleteContent(string path)
    {
        var index =
            repo.FindByPath(path)
            ?? throw new InvalidOperationException(
                $"Path {path} not found but you are going to delete it."
            );

        repo.Delete(index);
    }

    public List<MdIndexDto> ListIndex()
    {
        return repo.Select.ToList().Select(x => x.ToDto()).ToList();
    }

    private (string, string, string) PathRead(string path)
    {
        var index = repo.FindByPath(path);

        return index is null ? (Void, Void, NoContent) : (index.Name, index.Desc, index.Content);
    }

    private string PathWrite(string? path, string name, string desc, string content)
    {
        // Create new
        if (path is null)
        {
            var newIndex = new MdIndex
            {
                Name = name,
                Desc = desc,
                Content = content,
            };

            var newEntity = repo.Insert(newIndex);
            var newPath = newEntity.Path.ToString();

            return newPath;
        }

        var index = repo.FindByPath(path);

        // Create new if not found
        if (index is null)
        {
            Logger.Warn($"Requested path {path} not found. Creating new markdown file.");

            var newIndex = new MdIndex
            {
                Name = name,
                Desc = desc,
                Content = content,
            };

            var newEntity = repo.Insert(newIndex);
            var newPath = newEntity.Path.ToString();

            return newPath;
        }

        index.Name = name;
        index.Desc = desc;
        index.Content = content;
        repo.Update(index);

        return string.Empty;
    }
}
