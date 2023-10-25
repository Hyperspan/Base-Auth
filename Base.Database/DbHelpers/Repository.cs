using Hyperspan.Base.Shared;
using Hyperspan.Base.Shared.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            try
            {
                return await _dbContext
                    .Set<T>()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.QueryFailed, e);
            }

        }

        public async Task<List<T>> GetAllAsync(string sqlQuery)
        {
            try
            {

                return await _dbContext
                    .Set<T>()
                    .FromSqlRaw(sqlQuery)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.QueryFailed, e);
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.CreatedOn = DateTime.UtcNow;
                await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.InsertFailed, e);
            }
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            try
            {

                entities.ForEach(x =>
                {
                    x.CreatedOn = DateTime.UtcNow;
                });

                await _dbContext.Set<T>().AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.InsertFailed, e);
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var exist = _dbContext.Set<T>().Find(entity.Id);
                if (exist == null) return false;

                entity.LastModifiedOn = DateTime.UtcNow;
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.UpdateFailed, e);
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                var exist = _dbContext.Set<T>().Find(entity.Id);
                if (exist == null) return false;

                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new ApiErrorException(BaseErrorCodes.DeleteFailed, e);
            }
        }

    }
}
