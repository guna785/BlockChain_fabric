using BL.service;
using DAL.DALService;
using DAL.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repository
{
    public class RepoBL<TDocument> : IGenericBL<TDocument> where TDocument : CommonModel
    {
        IGenericService<TDocument> _service;
        public RepoBL(IGenericService<TDocument> service)
        {
            _service = service;
        }
        public IQueryable<TDocument> AsQueryable()
        {
            return _service.AsQueryable();
        }

        public void DeleteById(string id)
        {
            _service.DeleteById(id);
        }

        public Task DeleteByIdAsync(string id)
        {
            return _service.DeleteByIdAsync(id);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _service.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _service.DeleteManyAsync(filterExpression);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _service.DeleteOne(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _service.DeleteOneAsync(filterExpression);
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _service.FilterBy(filterExpression);
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _service.FilterBy<TProjected>(filterExpression, projectionExpression);
        }

        public TDocument FindById(string id)
        {
            return _service.FindById(id);
        }

        public Task<TDocument> FindByIdAsync(string id)
        {
            return _service.FindByIdAsync(id);
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _service.FindOne(filterExpression);
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _service.FindOneAsync(filterExpression);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _service.InsertMany(documents);
        }

        public Task InsertManyAsync(ICollection<TDocument> documents)
        {
            return _service.InsertManyAsync(documents);
        }

        public void InsertOne(TDocument document)
        {
            _service.InsertOne(document);
        }

        public Task<bool> InsertOneAsync(TDocument document)
        {
            return _service.InsertOneAsync(document);
        }

        public void ReplaceOne(TDocument document)
        {
            _service.ReplaceOne(document);
        }

        public async Task<bool> ReplaceOneAsync(TDocument document)
        {
            return await _service.ReplaceOneAsync(document);
        }
    }
}
