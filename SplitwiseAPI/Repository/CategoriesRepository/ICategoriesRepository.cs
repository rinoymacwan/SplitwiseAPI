using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.CategoriesRepository
{
    public interface ICategoriesRepository
    {
        IEnumerable<Categories> GetCategories();
        Task<Categories> GetCategory(int id);
        void CreateCategory(Categories Category);
        void UpdateCategory(Categories Category);
        Task DeleteCategory(Categories Category);
        Task Save();
        bool CategoryExists(int id);
    }
}
