using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using API.Middlewares;
using Newtonsoft.Json;
using API.DTOs;

namespace API.Repositories
{
    public class UserRepositoryEF : IUserRepository
    {
        public DataContext _context;
        public IConfiguration _configuration;

        public ILogger<UserRepositoryEF> _logger;

        public UserRepositoryEF(DataContext context, IConfiguration configuration, ILogger<UserRepositoryEF> logger)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public int Register(UserDTO userDTO)
        {
            byte[] passwordSalt, passwordHash;

            try
            {
                User user = _context.Users.SingleOrDefault(u => u.Email == userDTO.Email);
                if (user != null)
                {
                    throw new AppException("User already exists");
                }
                using (var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDTO.Password));
                }
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Role = 2;
                _context.Users.Add(user);
                _context.SaveChanges();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public UserDTO Login(UserDTO userData)
        {
            try
            {
                User user = _context.Users.First(u => u.Email == userData.Email);
                if (user == null && user?.PasswordSalt == null)
                {
                    throw new AppException("Invalid Email");
                }
                
                byte[]? passwordHash = (byte[]) user.PasswordHash;
                byte[]? passwordSalt = (byte[]) user.PasswordSalt;

                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userData.Password));
                    if (!computedHash.SequenceEqual(passwordHash))
                    {
                        throw new AppException("Password is incorrect");
                    };
                }
                string RoleName = _context.Roles.First(r => r.Id == user.Role).Name;
                UserDTO userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    RoleName = RoleName
                };
                userDTO.Token = CreateToken(userDTO);
                return userDTO;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string CreateToken(UserDTO user)
        {
            if (user.Email == null || user.RoleName == null) return "";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
