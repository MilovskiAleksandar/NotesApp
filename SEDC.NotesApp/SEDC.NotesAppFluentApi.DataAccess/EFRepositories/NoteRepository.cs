using Microsoft.EntityFrameworkCore;
using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.DataAccess.EFRepositories
{
    public class NoteRepository : IRepository<Note>
    {
        private readonly NotesAppDbContext _context;

        public NoteRepository(NotesAppDbContext context)
        {
            _context = context;
        }

        public void Add(Note entity)
        {
            _context.Notes.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Note entity)
        {
            _context.Notes.Remove(entity);
            _context.SaveChanges();
        }

        public List<Note> GetAll()
        {
            return _context.Notes.Include(x => x.User).ToList(); //join with table Users
                
        }

        public Note GetById(int id)
        {
            return _context.Notes.Include(x => x.User).FirstOrDefault(x => x.Id == id);
        }

        public void Update(Note entity)
        {
            _context.Notes.Update(entity);
            _context.SaveChanges();
        }
    }
}
