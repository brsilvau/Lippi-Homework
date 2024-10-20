using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace lippi_homework.Tests.Extensions
{
    public static class DbSetExtensions
    {
        public static Task<List<T>> ToListAsyncFake<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(queryable.ToList());
        }
    }
}
