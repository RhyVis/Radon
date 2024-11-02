namespace Radon.Core.Model.Extra.Tarot;

public class TarotDeck
{
    public string Name { get; set; } = "";
    public string Loc { get; set; } = "";
    public bool Full { get; set; }
    public bool HasR { get; set; }
    public List<TarotCard> Deck { get; set; } = new();

    public TarotDeckInfo BuildInfo() =>
        new()
        {
            Name = Name,
            Loc = Loc,
            Full = Full,
        };
}
