namespace Radon.Core.Model.Extra.Tarot;

public class TarotCardDrawn
{
    public int Index { get; set; }
    public string Name { get; set; } = "";
    public string Loc { get; set; } = "";
    public string Img { get; set; } = "";
    public bool Rev { get; set; }
    public TarotCard.TarotDesc Desc { get; set; } = new();
}