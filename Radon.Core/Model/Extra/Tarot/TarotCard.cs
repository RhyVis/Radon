namespace Radon.Core.Model.Extra.Tarot;

public class TarotCard
{
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public string Loc { get; set; } = "";
    public string Img { get; set; } = "";
    public TarotDesc Desc { get; set; } = new();

    public TarotCardDrawn BuildDrawn(string imgPrefix, bool rev = false)
    {
        return new TarotCardDrawn
        {
            Index = Index,
            Name = Name,
            Loc = Loc,
            Img = $"{imgPrefix}/{Img}",
            Rev = rev,
            Desc = Desc
        };
    }

    public class TarotDesc
    {
        public string Upright { get; set; } = "";
        public string Reverse { get; set; } = "";
        public List<string> Desc { get; set; } = [];
    }
}