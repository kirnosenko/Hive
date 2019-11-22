namespace Hive.SeedWorks.TacticalPatterns.Repository
{
	/// <summary>
	/// Интерфейс репозитория запросов.
	/// </summary>
	public interface IQueryRepository<TBoundedContext, TEntity> :
		IRepositoryReadCount<TBoundedContext>,
		IRepositoryReadEnumerable<TEntity>,
		IRepositoryReadQueryable<TEntity>,
		IRepositoryReadSingle<TEntity>
        where TBoundedContext : IBoundedContext
        where TEntity: class, IEntity
    {
	}
}
