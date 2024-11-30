using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class MdRes : BaseApiRes<MdRecord>;

public record MdRecord(string Name, string Desc, string Content);