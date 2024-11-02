using Radon.Data.Entity.Core;
using Radon.Data.Repository.Base;

namespace Radon.Data.Repository.Core;

public class TextStorageRepository(IFreeSql fsql) : BaseRepository<EntryTextStorage, long>(fsql) { }
