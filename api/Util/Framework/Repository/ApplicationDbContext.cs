using System;
using System.Security.Cryptography;
using api_lrpd.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_lrpd.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            BuildEstado(builder);
            BuildCidade(builder);
            BuildLogradouro(builder);
            BuildEndereco(builder);
            BuildUsuario(builder);
            BuildToken2FA(builder);
            BuildMenu(builder);

            base.OnModelCreating(builder);
        }

        private static void BuildMenu(ModelBuilder builder)
        {
            builder.Entity<MenuIdentityRole>((e) =>
            {
                e.HasKey(x => x.id);
                //e.Navigation(m => m.menu).AutoInclude();
                e.Navigation(i => i.role).AutoInclude();
            });

            builder.Entity<Menu>((e) =>
            {
                e.HasKey(x => x.id);
                e.Navigation(mr => mr.roles).AutoInclude();
                e.HasMany(x => x.children).WithOne().HasForeignKey(x => x.menuId);
                //e.Navigation(m => m.children).AutoInclude();
                e.Property(p => p.visible).HasDefaultValue(false);
            });
        }

        private static void BuildEstado(ModelBuilder builder)
        {
            builder.Entity<Estado>().HasKey(k => k.id);
        }

        private static void BuildCidade(ModelBuilder builder)
        {
            builder.Entity<Cidade>().HasKey(k => k.id);
            //builder.Entity<Cidade>().HasOne<Estado>();
            builder.Entity<Cidade>().Navigation(e => e.estado).AutoInclude();
        }

        private static void BuildLogradouro(ModelBuilder builder)
        {
            builder.Entity<Logradouro>().HasKey(k => k.id);
            builder.Entity<Logradouro>().HasIndex(x => x.cep).IsUnique();
            //builder.Entity<Logradouro>().HasOne<Cidade>();
            builder.Entity<Logradouro>().Navigation(e => e.cidade).AutoInclude();
        }

        private static void BuildEndereco(ModelBuilder builder)
        {
            builder.Entity<Endereco>().HasKey(k => k.id);
            //builder.Entity<Endereco>().HasOne<Logradouro>();
            builder.Entity<Endereco>().Navigation(e => e.logradouro).AutoInclude();
        }

        private static void BuildToken2FA(ModelBuilder builder)
        {
            var eBuilder = builder.Entity<Token2FA>();
            eBuilder.HasKey(x => x.Id);
            eBuilder.Navigation(e => e.IdentityUser).AutoInclude();
        }

        private static void BuildUsuario(ModelBuilder builder)
        {
            builder.Entity<Usuario>().HasKey(k => k.id);
            //builder.Entity<Usuario>().HasOne<IdentityUser>();
            //builder.Entity<Usuario>().HasOne<Endereco>(u => u.endereco).WithOne(e => e.usuario).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Usuario>().HasIndex(x => x.cpf).IsUnique();
            builder.Entity<Usuario>().HasIndex(x => x.email).IsUnique();
            builder.Entity<Usuario>().Navigation(e => e.IdentityUser).AutoInclude();
            builder.Entity<Usuario>().Navigation(e => e.endereco).AutoInclude();

            builder.Entity<Usuario>().Property(x => x.cpf).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<Usuario>().Property(x => x.email).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<Usuario>().Property(x => x.status).HasDefaultValue("online");
        }        
    }
}

