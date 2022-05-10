using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    int Register(User user);
    User Login(User user);

  }
}