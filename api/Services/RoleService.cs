using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interface;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class RoleService : IRoleService
    {
         private readonly ApplicationDbContext _context;

         public RoleService(ApplicationDbContext context){
            _context = context;
         }
        public async Task<bool> AddToRoleAsync(User user, string roleName)
        {
            //Role exist check
            var roleList = await _context.Roles.Select(role => role.Name).ToListAsync();
            if (!roleList.Contains(roleName))
            {
                return false;
            }
            
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return false;

            // Create a UserRole and add it to the context
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}