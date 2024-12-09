using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ShopService(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    /* public async Task<BaseCategories>  GetBaseCategories() => await _context.baseCategories.AsNoTracking().ToListAsync();
    public async Task<BaseCategory>  GetBaseCategories() => await _context.baseCategories.AsNoTracking().ToListAsync(); */
    
}