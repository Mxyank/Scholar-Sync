using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;
using Scholarship_Plaatform_Backend.Models;
using Scholarship_Plaatform_Backend.Data;

namespace Scholarship_Plaatform_Backend.Services
{
     public class AuthService: IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuartion,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            this._context = context;
            this._configuration = configuartion;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public async Task<(int, string)> Registration(User model, string role)
        {
            var userNameExists = await _userManager.FindByNameAsync(model.Username);
            if (userNameExists != null)
            {
                return (0, "User name already exists");
            }
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                return (0, "A user with this email already exists");
            }
            ApplicationUser applicationUser = new()
            {
                Email = model.Email,
                UserName = model.Username
            };
            IdentityResult result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (result.Succeeded && (role == UserRoles.Admin || role == UserRoles.User))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(applicationUser, role);
                await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();
                return (1, "User Created Successfully");
            }
            return (0, "Failed to register user, please check user details");
        }
        public async Task<(int, string)> Login(LoginModel model)
        {
            ApplicationUser savedUser = await _userManager.FindByEmailAsync(model.Email);
            if (savedUser == null)
            {
                return (0, "Invalid email");
            }
            var result = await _signInManager.PasswordSignInAsync(savedUser.UserName, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //Retreiving User from Db to save UserId in Claims
                var customUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (customUser == null)
                {
                    return (0, "Invalid email");
                }
                IList<string> roles = await _userManager.GetRolesAsync(savedUser);
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, savedUser.UserName),
                    new Claim(ClaimTypes.Email, savedUser.Email),
                    //Saving UserId in token
                    new Claim(ClaimTypes.NameIdentifier, customUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault("User"))
                };
                string token = GenerateJwtToken(claims);
                return (1, token);
            }
            return (0, "Invalid password");
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "S6781:JWT secret keys should not be disclosed.", Justification = "Key is securely stored.")]
        private string GenerateJwtToken(List<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT key is missing");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var crendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = crendentials
            };
            SecurityToken generatedToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(generatedToken);
        }
    }

}
