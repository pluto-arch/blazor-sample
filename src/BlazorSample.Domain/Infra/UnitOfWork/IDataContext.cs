﻿using System.Data;
using Microsoft.Extensions.Logging;

namespace BlazorSample.Uow
{
    public interface IDataContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IDbConnection DbConnection { get; }

        IDbTransaction DbTransaction { get; }

        int? CommandTimeOut { get; set; }

        ILogger GetLogger<TSourceContext>();


        bool HasChanges();
    }
}