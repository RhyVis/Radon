using Radon.Common.Core.Extension;
using Radon.Common.Enums;
using Radon.Core.Data.Entity;
using Radon.Core.Model.Base;

namespace Radon.Core.Model.Response;

public class SpamRes : BaseApiRes<List<SpamRes.SpamResData>>
{
    private SpamRes(SpamEntity e)
    {
        Code = ResponseCodeType.SUCCESS.ToInt();
        Msg = ResponseCodeType.SUCCESS.ToString();
        Data = [new SpamResData { Id = e.Id, Text = e.Text }];
    }

    private SpamRes(IEnumerable<SpamEntity> entities)
    {
        Code = ResponseCodeType.SUCCESS.ToInt();
        Msg = ResponseCodeType.SUCCESS.ToString();
        Data = entities.Select(e => new SpamResData { Id = e.Id, Text = e.Text }).ToList();
    }

    public class SpamResData
    {
        public long Id { get; set; }
        public string Text { get; set; } = "";
    }

    public static SpamRes FromEntity<T>(T? entity)
        where T : SpamEntity
    {
        return (entity is null) ? new SpamRes(new DummySpamEntity()) : new SpamRes(entity);
    }

    public static SpamRes FromEntity<T>(IEnumerable<T> entities)
        where T : SpamEntity
    {
        return new SpamRes(entities);
    }

    public static SpamRes FromDummy()
    {
        return new SpamRes(new DummySpamEntity());
    }
}
