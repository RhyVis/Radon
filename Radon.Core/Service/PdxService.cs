using Radon.Common.Core.DI;
using Radon.Core.Data.Entity;
using Radon.Core.Data.Repository;
using Radon.Core.Enums;
using Radon.Core.Service.Interface;

namespace Radon.Core.Service;

[ServiceScoped]
public class PdxService(TextStorageRepository textRepo) : IPdxService
{
    public string GetLangParserReplacer(long userId)
    {
        return textRepo
            .Select.Where(e =>
                e.Id == (long)PreservedStrStorageId.PDX_LANG_PARSER_REPLACER + userId
            )
            .ToOne()
            ?.Text ?? "{}";
    }

    public bool SetLangParserReplacer(long userId, string jsonData)
    {
        var entity = new EntryTextStorage
        {
            Id = (long)PreservedStrStorageId.PDX_LANG_PARSER_REPLACER + userId,
            Text = jsonData,
            Note = $"LangParserReplacer for user {userId}"
        };
        textRepo.InsertOrUpdate(entity);
        return true;
    }
}