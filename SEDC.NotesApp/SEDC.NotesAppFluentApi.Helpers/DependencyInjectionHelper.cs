using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SEDC.NotesAppFluentApi.DataAccess;
using SEDC.NotesAppFluentApi.DataAccess.EFRepositories;
using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.Services.Implementations;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection serviceCollection, string connectionString)
        {
            //serviceCollection.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer("Server=.\\;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True"));
            serviceCollection.AddDbContext<NotesAppDbContext>(x => x.UseSqlServer(connectionString));
        }

        public static void InjectServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INoteService, NoteService>();
            serviceCollection.AddTransient<IUserService, UserService>();
        }

        public static void InjectRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRepository<Note>, NoteRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
