using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class PlainTextRes(string data) : BaseApiRes<string>(data);
