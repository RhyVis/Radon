namespace Radon.Core.Model.Extra.Tarot;

public class TarotDeck
{
    public string Name { get; init; } = "";
    public string Loc { get; init; } = "";
    public bool Full { get; init; }
    public bool HasR { get; init; }
    public List<TarotCard> Deck { get; init; } = [];

    public TarotDeckInfo BuildInfo()
    {
        return new TarotDeckInfo
        {
            Name = Name,
            Loc = Loc,
            Full = Full
        };
    }
}