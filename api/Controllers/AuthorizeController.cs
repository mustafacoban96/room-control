using System;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto.User;
using api.Helper;
using api.Interface;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public AuthorizeController(ApplicationDbContext context, ITokenService tokenService, IMapper mapper, IRoleService roleService)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists by email
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return Conflict("A user with this email already exists.");

            // Map DTO to User entity and hash password
            var user = _mapper.Map<User>(registerDto);
            user.Password = PasswordHasherBCrypt.HashPassword(registerDto.Password);

            try
            {
                // Add user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var Roles = registerDto.Roles;
                var roleResult = false;
                foreach (var role in Roles)
                {
                    roleResult = await _roleService.AddToRoleAsync(user,role.Name);
                }

                if(roleResult){
                    var token = _tokenService.GenerateToken(user);
                    return Ok(new
                    {
                        Message = "User registered successfully.",
                        Token = token
                    });
                }
                else{
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(500, "Failed to assign role to the user.");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
          
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user exists by email
            var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            // Check if the password is correct
            bool isPasswordValid = PasswordHasherBCrypt.VerifyPassword(loginDto.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid credentials.");

            try
            {
                 var token = _tokenService.GenerateToken(user);
                    return Ok(new
                    {
                        Message = "Login successfully.",
                        Token = token
                    });
            }
            catch (Exception ex)
            {
                //return StatusCode(500, user);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
