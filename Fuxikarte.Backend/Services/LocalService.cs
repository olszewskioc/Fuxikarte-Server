using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.DTOs;
using AutoMapper;

namespace Fuxikarte.Backend.Services
{
    public class LocalService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LocalService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateLocal(NewLocalDTO model)
        {
            Local local = _mapper.Map<Local>(model);
            _context.Locals.Add(local);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<LocalDTO>> GetAllLocals()
        {
            var locals = await _context.Locals.ToListAsync();
            return _mapper.Map<IEnumerable<LocalDTO>>(locals);
        }

        public async Task<LocalDTO> GetLocalById(int id)
        {
            var local = await _context.Locals.FindAsync(id);
            return _mapper.Map<LocalDTO>(local);
        }

        public async Task<IEnumerable<LocalNavDTO>> GetAllLocalsNav()
        {
            var locals = await _context.Locals
                .Include(c => c.Sales)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LocalNavDTO>>(locals);
        }

        public async Task<LocalNavDTO> GetLocalByIdNav(int id)
        {
            var local = await _context.Locals
                .Include(c => c.Sales)
                .FirstOrDefaultAsync(c => c.LocalId == id);
            return _mapper.Map<LocalNavDTO>(local);
        }
        
        public async Task<bool> UpdateLocal(UpdateLocalDTO model, int id)
        {
            var local = await _context.Locals.FindAsync(id);
            if (local == null) return false;

            local.LocalName = model.LocalName ?? local.LocalName;
            local.Description = model.Description ?? local.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLocal(int id)
        {
            var local = await _context.Locals.FindAsync(id);
            if (local == null) return false;

            _context.Locals.Remove(local);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}