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
using API.DTOs;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        public string connectionString { get; set; }
        public IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public int Register(UserDTO user)
        {
            byte[] passwordSalt, passwordHash;

            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                CheckIfEmailExist(user.Email);
                try
                {
                    using (SqlCommand cmd = new SqlCommand("InsertUser", cnn))
                    {
                        using (var hmac = new HMACSHA512())
                        {
                            passwordSalt = hmac.Key;
                            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Role", 2);
                        cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        Int32 newId = (Int32)cmd.ExecuteScalar();
                        return newId;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        private void CheckIfEmailExist(string Email)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("CheckEmail", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", Email);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            throw new AppException("Email already exist");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnn != null) cnn.Close();
            }
        }


        public UserDTO Login(UserDTO userData)
        {
            UserDTO user = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("LoginUser", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", userData.Email);

                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (!reader.HasRows)
                            {
                                throw new AppException("User not found");
                            }

                            while (reader.Read())
                            {
                                byte[] passwordHash = (byte[])(reader["PasswordHash"]);
                                byte[] passwordSalt = (byte[])(reader["PasswordSalt"]);

                                using (var hmac = new HMACSHA512(passwordSalt))
                                {
                                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userData.Password));
                                    if (!computedHash.SequenceEqual(passwordHash))
                                    {
                                        throw new AppException("Password is incorrect");
                                    };
                                }
                                user.Id = Convert.ToInt32(reader["Id"]);
                                user.Name = reader["Name"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.RoleName = reader["Role"].ToString();

                            }

                            user.Token = CreateToken(user);
                            return user;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (cnn != null) cnn.Close();
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
