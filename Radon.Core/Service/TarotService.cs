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

        var rand = new Random();
        var imgPrefix = UrlBuilder
            .Create(AppSettings.Get().ResourceEndpoint)
            .Append("tarot", "img")
            .Build();
        var deckPicked = deck.Deck.OrderBy(_ => rand.Next()).Take(count);

        var deckProcessed = deck.HasR
            ? deckPicked.Select(c => c.BuildDrawn(imgPrefix, rand.NextBool()))
            : deckPicked.Select(c => c.BuildDrawn(imgPrefix));

        return deckProcessed.ToList();
    }
}
