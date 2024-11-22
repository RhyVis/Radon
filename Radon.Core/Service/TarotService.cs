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
    public TarotRes HandleTarot() => new(DrawShuffled("waite", 1));

    public TarotRes HandleTarotComplex(TarotReq req) => new(HandleRequest(req));

    public UnsetRes HandleTarotInfo() => new(TarotData.GetDeckInfoDict());

    public UnsetRes GetTarotDeckData(string name, bool full, bool shuffle)
    {
        var canBeFull = TarotData.GetDeckInfoDict()[name].Full;
        var deck = TarotData.GetDeck(name + (canBeFull && !full ? "_main" : ""));
        if (shuffle)
        {
            var newDeck = new TarotDeck
            {
                Name = deck.Name,
                Loc = deck.Loc,
                Full = deck.Full,
                HasR = deck.HasR,
                Deck = GetShuffledList(deck),
            };
            return new UnsetRes(newDeck);
        }

        return new UnsetRes(deck);
    }

    private static List<TarotCardDrawn> HandleRequest(TarotReq req)
    {
        var deck = req.Data.Deck;
        var full = req.Data.Full;
        var size = req.Data.Size;
        if (!TarotData.GetDeckInfoDict().ContainsKey(deck))
        {
            return [];
        }
        if (TarotData.IsMainOnly(deck))
        {
            return DrawShuffled(deck, size);
        }
        return full ? DrawShuffled(deck, size) : DrawShuffled(deck + "_main", size);
    }

    private static List<TarotCardDrawn> DrawShuffled(string deckName, int count)
    {
        var deck = TarotData.GetDeck(deckName);
        var deckList = GetShuffledList(deck);

        var imgEndpoint = AppSettings.Get().ResourceEndpoint;
        var imgPrefix = UrlBuilder.Create(imgEndpoint).Append("tarot", "img").Build();
        var deckPicked = deckList.Take(count);

        var deckProcessed = deck.HasR
            ? deckPicked.Select(c => c.BuildDrawn(imgPrefix, new Random().NextBool()))
            : deckPicked.Select(c => c.BuildDrawn(imgPrefix));

        return deckProcessed.ToList();
    }

    private static List<TarotCard> GetShuffledList(TarotDeck deck)
    {
        var list = deck.Deck.DeepClone(true);
        list.Sort((_, _) => new Random().Next(-1, 1));
        return list;
    }
}
