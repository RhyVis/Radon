using Masuit.Tools;
using Radon.Common.Core.Config;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Radon.Common.Utils;
using Radon.Core.Initializer;
using Radon.Core.Model.Extra.Tarot;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceTransient]
public class TarotService : ITarotService
{
    public TarotRes HandleTarot()
    {
        return new TarotRes(DrawShuffled("waite", 1));
    }

    public TarotRes HandleTarotComplex(TarotReq req)
    {
        return new TarotRes(HandleRequest(req));
    }

    public UnsetRes HandleTarotInfo()
    {
        return new UnsetRes(TarotData.GetDeckInfoDict());
    }

    public UnsetRes GetTarotDeckData(string name, bool full, bool shuffle)
    {
        var canBeFull = TarotData.GetDeckInfo(name)?.Full ?? false;

        var deck = TarotData.GetDeck(name + (canBeFull && !full ? "_main" : string.Empty));
        if (!shuffle) return new UnsetRes(deck);

        var newDeck = new TarotDeck
        {
            Name = deck.Name,
            Loc = deck.Loc,
            Full = deck.Full,
            HasR = deck.HasR,
            Deck = GetShuffledList(deck)
        };
        return new UnsetRes(newDeck);
    }

    private static List<TarotCardDrawn> HandleRequest(TarotReq req)
    {
        var deck = req.Data.Deck;
        var full = req.Data.Full;
        var size = req.Data.Size;
        if (!TarotData.GetDeckInfoDict().ContainsKey(deck)) return [];
        if (TarotData.IsMainOnly(deck)) return DrawShuffled(deck, size);
        return full ? DrawShuffled(deck, size) : DrawShuffled(deck + "_main", size);
    }

    private static List<TarotCardDrawn> DrawShuffled(string deckName, int count)
    {
        var deck = TarotData.GetDeck(deckName);
        var deckList = GetShuffledList(deck);

        var imgEndpoint = AppSettings.Get().ResourceEndpoint;
        var imgPrefix = UrlBuilder.Create(imgEndpoint).Append("tarot", "img").Build();
        var deckPicked = deckList.Take(count);

        var rand = new Random();

        var deckProcessed = deck.HasR
            ? deckPicked.Select(c => c.BuildDrawn(imgPrefix, rand.NextBool()))
            : deckPicked.Select(c => c.BuildDrawn(imgPrefix));

        return deckProcessed.ToList();
    }

    private static List<TarotCard> GetShuffledList(TarotDeck deck, bool cut = true)
    {
        var list = deck.Deck.DeepClone(true);
        var rand = new Random();

        list.Sort((_, _) => rand.Next(-1, 1));

        if (!cut) return list;

        var firstCutIndex = rand.Next(1, list.Count - 1);
        var secondCutIndex = rand.Next(firstCutIndex + 1, list.Count);

        var firstPart = list.Take(firstCutIndex).ToList();
        var secondPart = list.Skip(firstCutIndex).Take(secondCutIndex - firstCutIndex).ToList();
        var thirdPart = list.Skip(secondCutIndex).ToList();

        var combinedDeck = new List<TarotCard>();
        combinedDeck.AddRange(firstPart);
        combinedDeck.AddRange(secondPart);
        combinedDeck.AddRange(thirdPart);

        return combinedDeck;
    }
}