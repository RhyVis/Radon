using Radon.Core.Model.Base;
using Radon.Core.Model.Extra.Tarot;

namespace Radon.Core.Model.Response;

public class TarotRes(List<TarotCardDrawn> drawn) : BaseApiRes<List<TarotCardDrawn>>(drawn);