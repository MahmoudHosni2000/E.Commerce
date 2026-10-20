using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Shared
{
    public record PaginatedResult<TRusult>(int pageIndex, int pageSize, int totalCount, IEnumerable<TRusult> data)
    {
    }
}
