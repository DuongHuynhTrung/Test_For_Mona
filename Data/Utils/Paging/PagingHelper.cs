﻿using System;
using static Data.Utils.Paging.PagingConstant;

namespace Data.Utils.Paging;

public static class PagingHelper
{
    public static IQueryable<TObject> GetWithPaging<TObject>(this IQueryable<TObject> source, int page, int pageSize, int safePageSizeLimit = FixedPagingConstant.MaxPageSize)
        where TObject : class
    {
        if (pageSize > safePageSizeLimit)
        {
            throw new Exception("Input page size is over safe limitation.");
        }

        if (source == null)
        {
            return Enumerable.Empty<TObject>().AsQueryable();
        }

        pageSize = pageSize < 1 ? 1 : pageSize;
        page = page < 1 ? 1 : page;

        source = source
            .Skip(page == 1 ? 0 : pageSize * (page - 1))
            .Take(pageSize);

        return source;
    }
}

