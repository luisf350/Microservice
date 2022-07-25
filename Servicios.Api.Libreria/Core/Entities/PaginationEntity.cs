using System.Collections.Generic;

namespace Servicios.Api.Libreria.Core.Entities
{
    public class PaginationEntity<TDocument> where TDocument : IDocument
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public FilterValue FilterValue { get; set; }
        public int PageQuantity { get; set; }
        public int TotalRows { get; set; }
        public IEnumerable<TDocument> Data { get; set; }
    }
}
