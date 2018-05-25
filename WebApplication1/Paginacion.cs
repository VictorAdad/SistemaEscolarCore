﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sistema
{
    
}

public class Paginacion<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalR { get; private set; }

    public Paginacion(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        TotalR = count;
        this.AddRange(items);

    }

    public bool HasPreviousPage // Regresa un 1 si puedo retroceder la página
        => (PageIndex > 1);

    public bool HasNextPage // Determina y devuelve 1 si existe una página siguiente
        => (PageIndex < TotalPages);

    public static async Task<Paginacion<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new Paginacion<T>(items, count, pageIndex, pageSize);
    }
}