using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DOMAIN.Interfaces
{
    public interface IContext
    {
        DbSet<Usuario> Usuarios { get; set; }

        
        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}