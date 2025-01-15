using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using NLog;
using Radon.Common.Core.DI;

namespace Radon.Core.Initializer;

public class DictInitializer : IInitializer
{
    private const string DictPathPrefix = "Radon.Core.Resources.Dict.";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
                DictPathPrefix + "DictHanEmoji.json"
            );
            _dictEmojiHan = DeserializeResource<Dictionary<string, string>>(
                DictPathPrefix + "DictEmojiHan.json"
            );
            _dictHanSpark = DeserializeResource<Dictionary<string, string>>(
                DictPathPrefix + "DictHanSpark.json"
            );
            _dictSparkHan = DeserializeResource<Dictionary<string, string>>(
                DictPathPrefix + "DictSparkHan.json"
            );
            _dictUnicode = DeserializeResource<Dictionary<string, string>>(
                DictPathPrefix + "DictUnicodeDiff.json"
            );

            DictData.SetupAll(
                _dictHanEmoji,
                _dictEmojiHan,
                _dictHanSpark,
                _dictSparkHan,
                _dictUnicode
            );

            Logger.Info("DictData successfully initialized.");

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

    private const string DictNotInitialized = "Dict not initialized";
    private static readonly Random RandInstance = new();

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
        if (!_isInitialized) throw new InvalidOperationException(DictNotInitialized);
        return _dictHanEmoji.TryGetValue(han, out var result) ? result[RandInstance.Next(result.Length)] : han;
    }

    public static string GetEmojiHan(string emoji)
    {
        return GetSingleValue(_dictEmojiHan, emoji);
    }

    public static string GetHanSpark(string han)
    {
        return GetSingleValue(_dictHanSpark, han);
    }

    public static string GetSparkHan(string spark)
    {
        return GetSingleValue(_dictSparkHan, spark);
    }

    public static string GetUnicodeDiff(string unicode)
    {
        return GetSingleValue(_dictUnicode, unicode);
    }

    private static string GetSingleValue(Dictionary<string, string> dict, string key)
    {
        if (!_isInitialized) throw new InvalidOperationException(DictNotInitialized);
        return dict.TryGetValue(key, out var result) ? result : key;
    }
}
