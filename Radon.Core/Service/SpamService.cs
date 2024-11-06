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

    public SpamRes Fetch(SpamFetchReq req)
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

    public bool Append(SpamAppendReq req)
    {
        var type = req.Data.Type.TryParseEnum(SpamType.N);
        try
        {
            switch (type)
            {
                case SpamType.AK:
                {
                    akRepo.Insert(new GachaAk { Text = req.Data.Text });
                    break;
                }
                case SpamType.GS:
                {
                    gsRepo.Insert(new GachaGs { Text = req.Data.Text });
                    break;
                }
                case SpamType.ML:
                {
                    mlRepo.Insert(new GachaMl { Text = req.Data.Text });
                    break;
                }
                case SpamType.AC:
                {
                    acRepo.Insert(new MemeAcgn { Text = req.Data.Text });
                    break;
                }
                case SpamType.DN:
                {
                    dnRepo.Insert(new MemeDinner { Text = req.Data.Text });
                    break;
                }
                case SpamType.SN:
                {
                    mnRepo.Insert(new SpamMin { Text = req.Data.Text });
                    break;
                }
                case SpamType.SX:
                {
                    mxRepo.Insert(new SpamMax { Text = req.Data.Text });
                    break;
                }
                case SpamType.N:
                {
                    _logger.Warn("Unrecognized spam appending type");
                    return false;
                }
                default:
                {
                    _logger.Warn("Unrecognized spam appending type");
                    return false;
                }
            }
            return true;
        }
        catch (Exception e)
        {
            _logger.Error(e, $"Failed to append spam for type {type}");
            throw;
        }
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
