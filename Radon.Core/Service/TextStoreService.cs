using Radon.Common.Core.DI;
using Radon.Core.Data.Entity;
using Radon.Core.Data.Repository;
using Radon.Core.Model.Request;
using Radon.Core.Model.Response;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceTransient]
public class TextStoreService(TextStorageRepository repo) : ITextStoreService
{
    public TextStoreRes Query(long id)
    {
        return TextStoreRes.FromEntity(QueryData(id));
    }

    public UnsetRes QueryAll()
    {
        var brief = repo.Select.ToList(e => new
        {
            e.Id,
            e.Note
        });
        return new UnsetRes(brief);
    }

    public StateRes Update(TextStoreReq req)
    {
        UpdateData(req.Data.Id, req.Data.Text, req.Data.Note);
        return new StateRes();
    }

    public StateRes Delete(long id)
    {
        repo.Delete(id);
        return new StateRes();
    }

    private EntryTextStorage QueryData(long id)
    {
        return repo.Get(id)
               ?? new EntryTextStorage
               {
                   Id = -1,
                   Text = string.Empty,
                   Note = string.Empty
               };
    }

    private void UpdateData(long id, string text, string note)
    {
        var entity = repo.Get(id);
        if (entity is not null)
        {
            entity.Text = text;
            entity.Note = note;
            repo.Update(entity);
        }
        else
        {
            repo.Insert(
                new EntryTextStorage
                {
                    Id = id,
                    Text = text,
                    Note = note
                }
            );
        }
    }
}
