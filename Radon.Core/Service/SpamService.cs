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
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public SpamRes HandleSpam(SpamReq req)
    {
        var size = req.Data.Size;
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        var dict = req.Data.Dict.TryParseEnum(DictType.NONE);
        _logger.Info($"Recieved request for {size} spam(s) of type {type}");
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

    private SpamRes HandleTransform<T>(T? entity, DictType type)
        where T : SpamEntity
    {
        if (entity is null)
        {
            return SpamRes.FromEntity(new DummySpamEntity());
        }
        entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        return SpamRes.FromEntity(entity);
    }

    private SpamRes HandleTransform<T>(List<T> entities, DictType type)
        where T : SpamEntity
    {
        if (entities.Count == 0)
        {
            return SpamRes.FromEntity(new DummySpamEntity());
        }
        foreach (var entity in entities)
        {
            entity.Text = dictProcessor.ProcessDict(type, entity.Text, true);
        }
        return SpamRes.FromEntity(entities);
    }
}
