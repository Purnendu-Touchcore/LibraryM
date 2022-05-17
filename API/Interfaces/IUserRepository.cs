using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DTOs;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    int Register(UserDTO user);
    UserDTO Login(UserDTO user);

  }
}