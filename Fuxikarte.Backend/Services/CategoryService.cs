using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCategory(NewCategoryDTO model)
        {
            Category category = _mapper.Map<Category>(model);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryNavDTO>> GetAllCategoriesNav()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CategoryNavDTO>>(categories);
        }

        public async Task<CategoryNavDTO> GetCategoryByIdNav(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            return _mapper.Map<CategoryNavDTO>(category);
        }
        
        public async Task<bool> UpdateCategory(UpdateCategoryDTO model, int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.CategoryName = model.CategoryName ?? category.CategoryName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}