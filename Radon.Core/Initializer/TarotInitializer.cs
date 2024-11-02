using System.Text.Json;
using NLog;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Common.Utils;
using Radon.Core.Model.Extra.Tarot;

namespace Radon.Core.Initializer;

public class TarotInitializer(IHttpClientFactory httpClientFactory) : IInitializer
{
    private Logger _logger = LogManager.GetCurrentClassLogger();

    private Dictionary<string, TarotDeck> _deckDict = new();
    private Dictionary<string, bool> _deckMainOnlyDict = new();
    private Dictionary<string, TarotDeckInfo> _deckInfoDict = new();

    public async Task<object> InitAsync()
    {
        try
        {
            var baseUrl = AppSettings.Get().ResourceEndpoint;
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var client = httpClientFactory.CreateClient("TarotClient");
            var mappingUrl = UrlBuilder
                .Create(baseUrl)
                .Append("tarot", "conf", "_conf.json")
                .Build();
            var json = await client.GetStringAsync(mappingUrl);
            var mainConf = JsonSerializer.Deserialize<TarotConfMapping>(json, serializerOptions)!; //解不出来就滚

            var loadings = mainConf.Mappings.Select(async mapping =>
            {
                var deckConfUrl = UrlBuilder
                    .Create(baseUrl)
                    .Append("tarot", "conf", mapping.Value)
                    .Build();
                var deckJson = await client.GetStringAsync(deckConfUrl);
                var deck = JsonSerializer.Deserialize<TarotDeck>(deckJson, serializerOptions)!;
                _deckDict[mapping.Key] = deck;
                _deckInfoDict[mapping.Key] = deck.BuildInfo();
                if (deck.Full)
                {
                    var altName = deck.Name + "_main";
                    var altDeck = new TarotDeck
                    {
                        Name = altName,
                        Loc = deck.Loc,
                        Full = false,
                        HasR = deck.HasR,
                        Deck = deck.Deck.Take(22).ToList(),
                    };
                    _deckDict[altName] = altDeck;
                    _deckMainOnlyDict[mapping.Key] = false;
                }
                else
                {
                    _deckMainOnlyDict[mapping.Key] = true;
                }

                _logger.Debug($"Init deck {deck.Name} with {deck.Deck.Count} cards");
                return 0;
            });

            await Task.WhenAll(loadings);

            TarotData.Init(_deckDict, _deckMainOnlyDict, _deckInfoDict);

            _logger.Info($"Successfully initialized {_deckDict.Count} decks");

            return await Task.FromResult(0);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to initialize TarotInitializer");

            return await Task.FromResult(-1);
        }
    }

    private class TarotConfMapping
    {
        public Dictionary<string, string> Mappings { get; init; } = null!;
    }
}

public static class TarotData
{
    private static Dictionary<string, TarotDeck> _deckDict = new();
    private static Dictionary<string, bool> _deckMainOnlyDict = new();
    private static Dictionary<string, TarotDeckInfo> _deckInfoDict = new();

    private static bool _initialized;

    public static void Init(
        Dictionary<string, TarotDeck> deckDict,
        Dictionary<string, bool> deckMainOnlyDict,
        Dictionary<string, TarotDeckInfo> deckInfoDict
    )
    {
        if (_initialized)
        {
            throw new InvalidOperationException("TarotConfig already initialized");
        }
        _deckDict = deckDict;
        _deckMainOnlyDict = deckMainOnlyDict;
        _deckInfoDict = deckInfoDict;
        _initialized = true;
    }

    public static TarotDeck GetDeck(string deckName)
    {
        if (!_initialized)
        {
            throw new InvalidOperationException("TarotConfig not initialized");
        }
        return _deckDict[deckName];
    }

    public static bool IsMainOnly(string deckName)
    {
        if (!_initialized)
        {
            throw new InvalidOperationException("TarotConfig not initialized");
        }
        return _deckMainOnlyDict[deckName];
    }

    public static Dictionary<string, TarotDeckInfo> GetDeckInfoDict()
    {
        if (!_initialized)
        {
            throw new InvalidOperationException("TarotConfig not initialized");
        }
        return _deckInfoDict;
    }
}
