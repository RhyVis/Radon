using Masuit.Tools;
using NLog;
using Radon.Common.Core.DI;
using Radon.Common.Core.Extension;
using Radon.Core.Data.Entity;
using Radon.Core.Data.Repository;
using Radon.Core.Enums;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Processor.Interface;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceTransient]
public class SpamService(
    GachaAkRepository akRepo,
    GachaGsRepository gsRepo,
    GachaMlRepository mlRepo,
    MemeAcgnRepository acRepo,
    MemeDinnerRepository dnRepo,
    SpamMinRepository mnRepo,
    SpamMaxRepository mxRepo,
    IDictProcessor dictProcessor
) : ISpamService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private enum SpamAppendCode
    {
        EntityNull = -1,
        Empty = -2,
        Exception = -6,
        Duplicate = -8,
        Unrecognized = -10,
    }

    public SpamRes Fetch(SpamFetchReq req)
    {
        var size = req.Data.Size;
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var dict = req.Data.Dict.TryParseEnum(DictType.NONE);
        Logger.Info($"Recieved request for {size} spam(s) of type {type}");
        if (size == 1)
            return type switch
            {
                SpamType.AK => HandleTransform(akRepo.FindRand(), dict),
                SpamType.GS => HandleTransform(gsRepo.FindRand(), dict),
                SpamType.ML => HandleTransform(mlRepo.FindRand(), dict),
                SpamType.AC => HandleTransform(acRepo.FindRand(), dict),
                SpamType.DN => HandleTransform(dnRepo.FindRand(), dict),
                SpamType.SN => HandleTransform(mnRepo.FindRand(), dict),
                SpamType.SX => HandleTransform(mxRepo.FindRand(), dict),
                _ => SpamRes.FromEntity(new DummySpamEntity())
            };
        return type switch
        {
            SpamType.AK => HandleTransform(akRepo.FindRand(size), dict),
            SpamType.GS => HandleTransform(gsRepo.FindRand(size), dict),
            SpamType.ML => HandleTransform(mlRepo.FindRand(size), dict),
            SpamType.AC => HandleTransform(acRepo.FindRand(size), dict),
            SpamType.DN => HandleTransform(dnRepo.FindRand(size), dict),
            SpamType.SN => HandleTransform(mnRepo.FindRand(size), dict),
            SpamType.SX => HandleTransform(mxRepo.FindRand(size), dict),
            _ => SpamRes.FromEntity(new DummySpamEntity())
        };
    }

    public SpamRes FetchPrecise(SpamFetchPreciseReq req)
    {
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var dict = req.Data.Dict.TryParseEnum(DictType.NONE);
        var ids = req.Data.Ids;
        Logger.Info($"Recieved request for [{ids.Join()}] precise spam of type {type}");
        if (ids.IsNullOrEmpty()) return SpamRes.FromDummy();
        if (ids.Length != 1)
            return type switch
            {
                SpamType.AK => HandleTransform(akRepo.FindIn(ids), dict),
                SpamType.GS => HandleTransform(gsRepo.FindIn(ids), dict),
                SpamType.ML => HandleTransform(mlRepo.FindIn(ids), dict),
                SpamType.AC => HandleTransform(acRepo.FindIn(ids), dict),
                SpamType.DN => HandleTransform(dnRepo.FindIn(ids), dict),
                SpamType.SN => HandleTransform(mnRepo.FindIn(ids), dict),
                SpamType.SX => HandleTransform(mxRepo.FindIn(ids), dict),
                _ => SpamRes.FromDummy()
            };
        var id = ids[0];
        return type switch
        {
            SpamType.AK => HandleTransform(akRepo.Find(id), dict),
            SpamType.GS => HandleTransform(gsRepo.Find(id), dict),
            SpamType.ML => HandleTransform(mlRepo.Find(id), dict),
            SpamType.AC => HandleTransform(acRepo.Find(id), dict),
            SpamType.DN => HandleTransform(dnRepo.Find(id), dict),
            SpamType.SN => HandleTransform(mnRepo.Find(id), dict),
            SpamType.SX => HandleTransform(mxRepo.Find(id), dict),
            _ => SpamRes.FromDummy()
        };
    }

    public long Append(SpamAppendReq req)
    {
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var text = req.Data.Text;
        try
        {
            switch (type)
            {
                case SpamType.AK: return HandleInsert<GachaAkRepository, GachaAk>(akRepo, text);
                case SpamType.GS: return HandleInsert<GachaGsRepository, GachaGs>(gsRepo, text);
                case SpamType.ML: return HandleInsert<GachaMlRepository, GachaMl>(mlRepo, text);
                case SpamType.AC: return HandleInsert<MemeAcgnRepository, MemeAcgn>(acRepo, text);
                case SpamType.DN: return HandleInsert<MemeDinnerRepository, MemeDinner>(dnRepo, text);
                case SpamType.SN: return HandleInsert<SpamMinRepository, SpamMin>(mnRepo, text);
                case SpamType.SX: return HandleInsert<SpamMaxRepository, SpamMax>(mxRepo, text);
                case SpamType.N:
                {
                    Logger.Warn("Empty spam appending type");
                    return SpamAppendCode.Empty.ToInt();
                }
                default:
                {
                    Logger.Warn("Unrecognized spam appending type");
                    return SpamAppendCode.Unrecognized.ToInt();
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Failed to append spam for type {type}");
            return SpamAppendCode.Exception.ToInt();
        }
    }

    private static long HandleInsert<TRepo, TEntity>(TRepo repo, string text)
        where TRepo : BaseSpamRepository<TEntity>
        where TEntity : SpamEntity, new()
    {
        if (repo.CheckIfDuplicate(text))
        {
            Logger.Warn($"Duplicate spam for type {typeof(TEntity)} with {text}");
            return SpamAppendCode.Duplicate.ToInt();
        }

        var e = repo.Insert(new TEntity { Text = text });
        return e?.Id ?? SpamAppendCode.EntityNull.ToInt();
    }

    private SpamRes HandleTransform<T>(T? entity, DictType type)
        where T : SpamEntity
    {
        if (entity is null) return SpamRes.FromDummy();
        entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        return SpamRes.FromEntity(entity);
    }

    private SpamRes HandleTransform<T>(List<T> entities, DictType type)
        where T : SpamEntity
    {
        if (entities.Count == 0) return SpamRes.FromDummy();
        foreach (var entity in entities) entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        return SpamRes.FromEntity(entities);
    }
}
