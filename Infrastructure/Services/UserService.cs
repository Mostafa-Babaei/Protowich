
using Application.Interfaces;
 
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    //public class UserService : IUserService
    //{
    //    private readonly AppDbContext _context;
    //    private readonly IConfiguration _config;

    //    public UserService(AppDbContext context, IConfiguration config)
    //    {
    //        _context = context;
    //        _config = config;
    //    }

    //    public async Task<string?> LoginAsync(string email, string password)
    //    {
    //        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    //        if (user == null) return null;

    //        // Claims
    //        var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //            new Claim(ClaimTypes.Email, user.Email),
    //            new Claim("accountId", user.AccountId.ToString()) // 👈 مهم برای Multi-Tenant
    //        };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _config["Jwt:Issuer"],
    //            audience: _config["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddHours(2),
    //            signingCredentials: creds
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }

    //    public async Task<User?> GetByIdAsync(Guid id) =>
    //        await _context.Users.FindAsync(id);

    //    public async Task<User?> GetByEmailAsync(string email) =>
    //        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    //}
}
