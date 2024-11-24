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

    public bool CheckContent(string path)
    {
        if (path != Void)
        {
            return PathExists(path);
        }

        Logger.Warn("Trying to access void markdown content.");
        return false;
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

    public string UpdateContent(string path, string name, string desc, string content)
    {
        if (path == Void)
        {
            Logger.Warn("Trying to access void markdown content.");
            return string.Empty;
        }

        try
        {
            return PathWrite(path, name, desc, content);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Exception occurred on updating markdown file on {path}.");
            throw;
        }
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

    private string PathWrite(string path, string name, string desc, string content)
    {
        var index = repo.FindByPath(path);

        if (index is null)
        {
            var newIndex = new MdIndex { Name = name, Desc = desc };

            FileWrite(path, content);

            repo.Insert(newIndex);

            return newIndex.Path.ToString();
        }

        FileWrite(index.Path.ToString(), content);

        return string.Empty;
    }

    private static void FileWrite(string path, string content)
    {
        var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppResDir, "md");
        var targetPath = Path.Combine(baseDir, $"{path}.md");

        File.WriteAllText(targetPath, content);
    }
}
