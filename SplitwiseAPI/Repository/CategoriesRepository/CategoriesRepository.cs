using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Repository.CategoriesRepository
{
    public class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public CategoriesRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool CategoryExists(int id)
        {
            return context.Categories.Any(e => e.Id == id);
        }

        public void CreateCategory(Categories Category)
        {
            context.Categories.Add(Category);
        }

        public async Task DeleteCategory(Categories Category)
        {
            context.Categories.Remove(Category);
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<Categories> GetCategories()
        {
            return context.Categories;
        }

        public Task<Categories> GetCategory(int id)
        {
            return context.Categories.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateCategory(Categories Category)
        {
            context.Entry(Category).State = EntityState.Modified;
        }
    }
}
