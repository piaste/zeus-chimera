using Chimera.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ZenProgramming.Chakra.Core.Data.Repositories;
using ZenProgramming.Chakra.Core.Entities;

namespace Chimera.Authentication.MongoDb
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {

        private const string USER_TABLE_NAME = "users";
        private readonly MongoDB.Driver.IMongoDatabase db;

        public MongoRepository(MongoDB.Driver.IMongoDatabase db)
        {
            this.db = db;
        }

        public int Count(Expression<Func<TEntity, bool>> filterExpression = null) => throw new NotImplementedException();
        public void Delete(TEntity entity) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
        public IList<TEntity> Fetch(Expression<Func<TEntity, bool>> filterExpression = null, int? startRowIndex = null, int? maximumRows = null, Expression<Func<TEntity, object>> sortExpression = null, bool isDescending = false) => throw new NotImplementedException();
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression) => throw new NotImplementedException();
        public bool IsValid(TEntity entity) => throw new NotImplementedException();
        public void Save(TEntity entity) => throw new NotImplementedException();
        public IList<ValidationResult> Validate(TEntity entity) => throw new NotImplementedException();
    }
}
