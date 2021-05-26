using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aroma_Shop.Domain.Models.ProductModels;

namespace Aroma_Shop.Application.Utilites
{
    public class Paging<T>
    {
        public Paging(IEnumerable<T> query, int pageSize, int pageNumber = 1)
        {
            var queryCount = 
                query.Count();

            var totalPages = 
                (int)Math.Ceiling(Decimal.Divide(queryCount, pageSize));
            FirstPage = 1;
            LastPage = totalPages;
            PreviousPage = 
                Math.Max(pageNumber - 1, FirstPage);
            NextPage = 
                Math.Min(pageNumber + 1, LastPage);

            QueryResult = 
                query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<T> QueryResult { get; set; }
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }
    }
}
