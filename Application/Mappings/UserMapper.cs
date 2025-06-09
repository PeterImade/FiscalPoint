using Application.Features.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class UserMapper
    {
        public static User ToEntity(this RegisterUserDto registerUser)
        {
            return new User
            {
                Email = registerUser.Email,
                UserName = registerUser.Email,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                PasswordHash = registerUser.Password,
                Currency = registerUser.Currency
            };
        }
    }
}
