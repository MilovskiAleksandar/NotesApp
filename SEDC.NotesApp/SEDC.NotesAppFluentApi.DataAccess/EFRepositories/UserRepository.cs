using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.DataAccess.EFRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NotesAppDbContext _context;

        public UserRepository(NotesAppDbContext context)
        {
            _context = context;
        }
        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
           return _context.Users.FirstOrDefault(x => x.UserName == username);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower()  && x.Password == password);
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
