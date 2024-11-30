using Radon.Core.Data.Entity;
using Radon.Data.Repository;

namespace Radon.Core.Data.Repository;

public class TextStorageRepository(IFreeSql fsql) : BaseRepository<EntryTextStorage, long>(fsql);