using System.Text.RegularExpressions;
using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Data.Entity;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public partial class SpamRes : BaseApiRes<List<SpamRes.SpamResData>>
{
    [GeneratedRegex(@"\r\n|\r|\n|\\r\\n|\\r|\\n|\0|\\0")]
    private static partial Regex Filter();

    private static string Normalize(string text) => Filter().Replace(text, "\n");

    private SpamRes(SpamEntity e)
    {
        Code = ResponseCodeType.SUCCESS.ToInt();
        Msg = ResponseCodeType.SUCCESS.ToString();
        Data = [SpamResData.FromEntity(e)];
    }

    private SpamRes(IEnumerable<SpamEntity> entities)
    {
        Code = ResponseCodeType.SUCCESS.ToInt();
        Msg = ResponseCodeType.SUCCESS.ToString();
        Data = entities.Select(SpamResData.FromEntity).ToList();
    }

    public class SpamResData
    {
        public long Id { get; set; }
        public string Text { get; set; } = "";

        public static SpamResData FromEntity(SpamEntity e) =>
            new() { Id = e.Id, Text = Normalize(e.Text) };
    }

    public static SpamRes FromEntity<T>(T? entity)
        where T : SpamEntity => entity is null ? FromDummy() : new SpamRes(entity);

    public static SpamRes FromEntity<T>(IEnumerable<T> entities)
        where T : SpamEntity => new(entities);

    public static SpamRes FromDummy() => new(SpamEntity.Dummy());
}
