using Hyperspan.Base.Shared.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hyperspan.Base.Database.DbHelpers
{
    public class Repository<TId, T, TContext> : IRepository<TId, T, TContext>
        where TId : IEquatable<TId>
        where T : class, IBaseEntity<TId>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public Repository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public Task<int> GetCount()
        {
            return Task.FromResult(Entities.Count());
        }

        public Task<int> GetCount(string sqlQuery)
        {
            using var objCommand = _dbContext.Database.GetDbConnection().CreateCommand();
            objCommand.CommandText = sqlQuery;
            objCommand.CommandType = System.Data.CommandType.Text;

            if (objCommand.Connection != null && objCommand.Connection.State != System.Data.ConnectionState.Open)
                objCommand.Connection.Open();
            if (_dbContext.Database.CurrentTransaction != null)
            {
                objCommand.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();
            }

            var intCount = Convert.ToInt32(objCommand.ExecuteScalar());

            objCommand.Dispose();

            return Task.FromResult(intCount);
        }

        public async Task<T?> GetById(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(string sqlQuery)
        {
            return await _dbContext
                .Set<T>()
                .FromSqlRaw(sqlQuery)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            //entity.IsDeleted = false;
            //entity.LastModifiedOn = DateTime.Now;
            //entity.CreatedOn = DateTime.Now;
            //entity.CreatedBy = _currentUserService.UserId;
            //entity.IsActive = true;

            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task<bool> AddRangeAsync(List<T> entities)
        {
            _dbContext.Set<T>().AddRangeAsync(entities);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            var exist = _dbContext.Set<T>().Find(entity.Id);
            //entity.LastModifiedOn = DateTime.Now;
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(T entity)
        {
            var exist = _dbContext.Set<T>().Find(entity.Id);
            //entity.IsDeleted = true;
            //entity.LastModifiedOn = DateTime.Now;
            //entity.IsActive = false;
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.FromResult(true);
        }

    }
}
