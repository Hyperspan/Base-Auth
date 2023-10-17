﻿
using Base.Shared.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Base.Database.DbHelpers
{
    public interface IUnitOfWork<T, TId, TContext>
        where TId : IEquatable<TId>
        where T : class
        where TContext : DbContext
    {

        IRepository<TId, T, TContext> Repository<T>() where T : BaseEntity<TId>;

        Task<int> Save(CancellationToken cancellationToken);

        Task<IDbContextTransaction> StartTransaction(bool checkIfAlreadyExists = false);

        Task Commit();

        Task Rollback(bool handleOnlyIfExists = false);

    }
}
