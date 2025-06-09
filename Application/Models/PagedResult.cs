using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PagedResult<T> where T : class
    {
        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public List<T>? Data { get; private set; } = new List<T>();

        public PagedResult(List<T> items, int pageSize, int pageNumber, int totalCount)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            Data.AddRange(items);   
        }


        public static async Task<PagedResult<T>> ToPagedListAsync(IQueryable<T> source, int pageSize, int pageNumber, bool shouldPaginate = true)
        {
            if (pageSize > 10) pageSize = 10;
            if (pageNumber < 1) pageSize = 1;

            int totalCount = source.Count();

            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            if (!shouldPaginate)
            {
                items = await source.ToListAsync();
                return new PagedResult<T>(items, 1, 1, totalCount);
            }

            items = items.ToList();
 
            return new PagedResult<T>(items, pageSize, pageNumber, totalCount);
        }
    }

    public static class PagedListExtensions
    {
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> items, int pageSize, int pageNumber) where T : class 
        {
            return await PagedResult<T>.ToPagedListAsync(items, pageSize, pageNumber);
        }
    }
}
