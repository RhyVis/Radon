using System.Text.RegularExpressions;
using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Data.Entity;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public partial class SpamRes : BaseApiRes<List<SpamRes.SpamResData>>
{
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

    [GeneratedRegex(@"\r\n|\r|\n|\\r\\n|\\r|\\n|\0|\\0")]
    private static partial Regex Filter();

    private static string Normalize(string text)
    {
        return Filter().Replace(text, "\n");
    }

    public static SpamRes FromEntity<T>(T? entity)
        where T : SpamEntity
    {
        return entity is null ? FromDummy() : new SpamRes(entity);
    }

    public static SpamRes FromEntity<T>(IEnumerable<T> entities)
        where T : SpamEntity
    {
        return new SpamRes(entities);
    }

    public static SpamRes FromDummy()
    {
        return new SpamRes(SpamEntity.Dummy());
    }

    public class SpamResData
    {
        public long Id { get; set; }
        public string Text { get; set; } = "";

        public static SpamResData FromEntity(SpamEntity e)
        {
            return new SpamResData { Id = e.Id, Text = Normalize(e.Text) };
        }
    }
}