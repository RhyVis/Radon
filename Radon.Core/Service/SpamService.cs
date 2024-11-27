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

    public SpamRes Fetch(SpamFetchReq req)
    {
        var size = req.Data.Size;
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var dict = req.Data.Dict.TryParseEnum(DictType.NONE);
        Logger.Info($"Recieved request for {size} spam(s) of type {type}");
        if (size == 1)
        {
            return type switch
            {
                SpamType.AK => HandleTransform(akRepo.FindRand(), dict),
                SpamType.GS => HandleTransform(gsRepo.FindRand(), dict),
                SpamType.ML => HandleTransform(mlRepo.FindRand(), dict),
                SpamType.AC => HandleTransform(acRepo.FindRand(), dict),
                SpamType.DN => HandleTransform(dnRepo.FindRand(), dict),
                SpamType.SN => HandleTransform(mnRepo.FindRand(), dict),
                SpamType.SX => HandleTransform(mxRepo.FindRand(), dict),
                _ => SpamRes.FromEntity(new DummySpamEntity()),
            };
        }
        return type switch
        {
            SpamType.AK => HandleTransform(akRepo.FindRand(size), dict),
            SpamType.GS => HandleTransform(gsRepo.FindRand(size), dict),
            SpamType.ML => HandleTransform(mlRepo.FindRand(size), dict),
            SpamType.AC => HandleTransform(acRepo.FindRand(size), dict),
            SpamType.DN => HandleTransform(dnRepo.FindRand(size), dict),
            SpamType.SN => HandleTransform(mnRepo.FindRand(size), dict),
            SpamType.SX => HandleTransform(mxRepo.FindRand(size), dict),
            _ => SpamRes.FromEntity(new DummySpamEntity()),
        };
    }

    public SpamRes FetchPrecise(SpamFetchPreciseReq req)
    {
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var dict = req.Data.Dict.TryParseEnum(DictType.NONE);
        var ids = req.Data.Ids;
        Logger.Info($"Recieved request for [{ids.Join()}] precise spam of type {type}");
        if (ids.IsNullOrEmpty())
        {
            return SpamRes.FromDummy();
        }
        if (ids.Length == 1)
        {
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
                _ => SpamRes.FromDummy(),
            };
        }
        return type switch
        {
            SpamType.AK => HandleTransform(akRepo.FindIn(ids), dict),
            SpamType.GS => HandleTransform(gsRepo.FindIn(ids), dict),
            SpamType.ML => HandleTransform(mlRepo.FindIn(ids), dict),
            SpamType.AC => HandleTransform(acRepo.FindIn(ids), dict),
            SpamType.DN => HandleTransform(dnRepo.FindIn(ids), dict),
            SpamType.SN => HandleTransform(mnRepo.FindIn(ids), dict),
            SpamType.SX => HandleTransform(mxRepo.FindIn(ids), dict),
            _ => SpamRes.FromDummy(),
        };
    }

    public long Append(SpamAppendReq req)
    {
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        try
        {
            switch (type)
            {
                case SpamType.AK:
                {
                    var e = akRepo.Insert(new GachaAk { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.GS:
                {
                    var e = gsRepo.Insert(new GachaGs { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.ML:
                {
                    var e = mlRepo.Insert(new GachaMl { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.AC:
                {
                    var e = acRepo.Insert(new MemeAcgn { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.DN:
                {
                    var e = dnRepo.Insert(new MemeDinner { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.SN:
                {
                    var e = mnRepo.Insert(new SpamMin { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.SX:
                {
                    var e = mxRepo.Insert(new SpamMax { Text = req.Data.Text });
                    return e?.Id ?? -1;
                }
                case SpamType.N:
                {
                    Logger.Warn("Empty spam appending type");
                    return -2;
                }
                default:
                {
                    Logger.Warn("Unrecognized spam appending type");
                    return -10;
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Failed to append spam for type {type}");
            return -6;
        }
    }

    private SpamRes HandleTransform<T>(T? entity, DictType type)
        where T : SpamEntity
    {
        if (entity is null)
        {
            return SpamRes.FromDummy();
        }
        entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        return SpamRes.FromEntity(entity);
    }

    private SpamRes HandleTransform<T>(List<T> entities, DictType type)
        where T : SpamEntity
    {
        if (entities.Count == 0)
        {
            return SpamRes.FromDummy();
        }
        foreach (var entity in entities)
        {
            entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        }
        return SpamRes.FromEntity(entities);
    }
}
