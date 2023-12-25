using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Mappers.Users
{
    public static class UserMapper
    {
        public static User ToUser(this RegisterUserDto userDto, string hash)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                Password = hash,
                Role = userDto.Role
            };
        }
    }
}
