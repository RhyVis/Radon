using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class UnsetRes(dynamic data) : BaseApiRes<dynamic>((object)data);
