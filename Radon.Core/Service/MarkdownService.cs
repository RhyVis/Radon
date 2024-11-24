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

    private const string AppResDir = "appResource";
    private const string NoContent = "### No content available.";
    private const string Void = "void";

    public MdIndex CheckContent(string path)
    {
        return repo.FindByPath(path) ?? new MdIndex();
    }

    public MdRecord ProvideContent(string path)
    {
        if (path == Void)
        {
            Logger.Warn("Trying to access void markdown content.");
            return new MdRecord(Void, NoContent);
        }

        try
        {
            var (name, content) = PathRead(path);
            return new MdRecord(name, content);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Exception occurred on reading markdown file on {path}.");
            throw;
        }
    }

    public string UpdateContent(string? path, string name, string desc, string content)
    {
        if (path == Void)
        {
            Logger.Warn("Trying to access void markdown content.");
            return string.Empty;
        }

        try
        {
            return (path.IsNullOrEmpty())
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

        FileDelete(index.Path.ToString());

        repo.Delete(index);
    }

    public List<MdIndex> ListIndex()
    {
        return repo.Select.ToList();
    }

    private bool PathExists(string path)
    {
        var index = repo.FindByPath(path);

        if (index is null)
        {
            return false;
        }

        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppResDir, "md");
        var targetPath = Path.Combine(baseDir, $"{index}.md");

        return File.Exists(targetPath);
    }

    private (string, string) PathRead(string path)
    {
        var index = repo.FindByPath(path);

        if (index is null)
        {
            return (Void, NoContent);
        }

        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppResDir, "md");
        var targetPath = Path.Combine(baseDir, $"{path}.md");

        return (index.Name, File.Exists(targetPath) ? File.ReadAllText(targetPath) : NoContent);
    }

    private string PathWrite(string? path, string name, string desc, string content)
    {
        if (path is null)
        {
            var newIndex = new MdIndex { Name = name, Desc = desc };

            var newEntity = repo.Insert(newIndex);
            var newPath = newEntity.Path.ToString();

            FileWrite(newPath, content);

            return newPath;
        }

        var index = repo.FindByPath(path);

        if (index is null)
        {
            Logger.Warn($"Requested path {path} not found. Creating new markdown file.");

            var newIndex = new MdIndex { Name = name, Desc = desc };

            var newEntity = repo.Insert(newIndex);
            var newPath = newEntity.Path.ToString();

            FileWrite(newPath, content);

            return newPath;
        }

        index.Name = name;
        index.Desc = desc;

        FileWrite(index.Path.ToString(), content);

        return string.Empty;
    }

    private static void FileWrite(string path, string content)
    {
        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppResDir, "md");
        var targetPath = Path.Combine(baseDir, $"{path}.md");

        File.WriteAllText(targetPath, content);
    }

    private static void FileDelete(string path)
    {
        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppResDir, "md");
        var targetPath = Path.Combine(baseDir, $"{path}.md");

        File.Delete(targetPath);
    }
}
