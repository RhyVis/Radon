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
    public TextStoreRes HandleTextStoreQuery(TextStoreReq req)
    {
        return TextStoreRes.FromEntity(Query(req.Data.Id));
    }

    public StateRes HandleTextStoreUpdate(TextStoreReq req)
    {
        Update(req.Data.Id, req.Data.Text, req.Data.Note);
        return new StateRes();
    }

    private EntryTextStorage Query(long id)
    {
        return repo.Get(id)
               ?? new EntryTextStorage
               {
                   Id = -1,
                   Text = string.Empty,
                   Note = string.Empty
               };
    }

    private void Update(long id, string text, string note)
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