using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Servicios.Api.Libreria.Core;
using Servicios.Api.Libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.Api.Libreria.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);

            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<TDocument> GetById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertDocument(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateDocument(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);

            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task DeleteById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);

            await _collection.DeleteOneAsync(filter);
        }

        public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination)
        {
            long totalDocuments = 0;
            var sort = pagination.SortDirection == "desc" ?
                Builders<TDocument>.Sort.Descending(pagination.Sort) :
                Builders<TDocument>.Sort.Ascending(pagination.Sort);

            if (pagination.FilterValue == null)
            {
                pagination.Data = await _collection.Find(x => true)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();

                totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            }
            else
            {
                var valueFilter = ".*" + pagination.FilterValue.Valor + ".*";
                var filter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Priopiedad, new MongoDB.Bson.BsonRegularExpression(valueFilter, "i"));
                pagination.Data = await _collection.Find(filter)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();

                totalDocuments = await _collection.CountDocumentsAsync(filter);
            }

            var rounded = Math.Ceiling(totalDocuments / Convert.ToDecimal(pagination.PageSize));
            pagination.PageQuantity = Convert.ToInt32(rounded);

            return pagination;
        }

        #region Private Methods
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }
        #endregion
    }
}
