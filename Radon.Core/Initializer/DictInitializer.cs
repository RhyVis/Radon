using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using NLog;
using Radon.Common.Core.DI;

namespace Radon.Core.Initializer;

public class DictInitializer : IInitializer
{
    private const string PATH = "Radon.Core.Resources.Dict.";
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private Dictionary<string, string> _dictEmojiHan = new();

    private Dictionary<string, string[]> _dictHanEmoji = new();
    private Dictionary<string, string> _dictHanSpark = new();
    private Dictionary<string, string> _dictSparkHan = new();
    private Dictionary<string, string> _dictUnicode = new();

    public async Task<object> InitAsync()
    {
        try
        {
            _dictHanEmoji = DeserializeResource<Dictionary<string, string[]>>(
                PATH + "DictHanEmoji.json"
            );
            _dictEmojiHan = DeserializeResource<Dictionary<string, string>>(
                PATH + "DictEmojiHan.json"
            );
            _dictHanSpark = DeserializeResource<Dictionary<string, string>>(
                PATH + "DictHanSpark.json"
            );
            _dictSparkHan = DeserializeResource<Dictionary<string, string>>(
                PATH + "DictSparkHan.json"
            );
            _dictUnicode = DeserializeResource<Dictionary<string, string>>(
                PATH + "DictUnicodeDiff.json"
            );

            DictData.SetupAll(
                _dictHanEmoji,
                _dictEmojiHan,
                _dictHanSpark,
                _dictSparkHan,
                _dictUnicode
            );

            _logger.Info("DictData successfully initialized.");

            return await Task.FromResult(0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return await Task.FromResult(-1);
        }
    }

    private static T DeserializeResource<T>(string resourceName)
    {
        using var stream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"{resourceName} not found in assembly resources.");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json)
               ?? throw new SerializationException($"Failed to deserialize {resourceName}.");
    }
}

public static class DictData
{
    private static Dictionary<string, string[]> _dictHanEmoji = new();
    private static Dictionary<string, string> _dictEmojiHan = new();
    private static Dictionary<string, string> _dictHanSpark = new();
    private static Dictionary<string, string> _dictSparkHan = new();
    private static Dictionary<string, string> _dictUnicode = new();

    private static bool _isInitialized;

    public static void SetupAll(
        Dictionary<string, string[]> dictHanEmoji,
        Dictionary<string, string> dictEmojiHan,
        Dictionary<string, string> dictHanSpark,
        Dictionary<string, string> dictSparkHan,
        Dictionary<string, string> dictUnicode
    )
    {
        _isInitialized = false;
        _dictHanEmoji = dictHanEmoji;
        _dictEmojiHan = dictEmojiHan;
        _dictHanSpark = dictHanSpark;
        _dictSparkHan = dictSparkHan;
        _dictUnicode = dictUnicode;
        _isInitialized = true;
    }

    public static string GetHanEmoji(string han)
    {
        if (!_isInitialized) throw new InvalidOperationException("Dict not initialized");

        _dictHanEmoji.TryGetValue(han, out var result);

        return result?[new Random().Next(result.Length)] ?? han;
    }

    public static string GetEmojiHan(string emoji)
    {
        if (!_isInitialized) throw new InvalidOperationException("Dict not initialized");

        _dictEmojiHan.TryGetValue(emoji, out var result);

        return result ?? emoji;
    }

    public static string GetHanSpark(string han)
    {
        if (!_isInitialized) throw new InvalidOperationException("Dict not initialized");

        _dictHanSpark.TryGetValue(han, out var result);

        return result ?? han;
    }

    public static string GetSparkHan(string spark)
    {
        if (!_isInitialized) throw new InvalidOperationException("Dict not initialized");

        _dictSparkHan.TryGetValue(spark, out var result);

        return result ?? spark;
    }

    public static string GetUnicodeDiff(string unicode)
    {
        if (!_isInitialized) throw new InvalidOperationException("Dict not initialized");

        _dictUnicode.TryGetValue(unicode, out var result);

        return result ?? unicode;
    }
}