using System;
using api_lrpd.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace api_lrpd.Util
{
    public static class Inicializador
    {
        public static void ConfigurarBanco(WebApplication app)
        {
            IServiceScope serviceScope = app.Services.CreateScope();
            try
            {
                ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                applicationDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            try
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                var role = roleManager.FindByNameAsync("ADMIN").Result;
                if (role == null)
                {
                    var result = roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = "ADMIN"
                    }).Result;

                }
                role = roleManager.FindByNameAsync("ADMIN").Result;

                var repository = serviceScope.ServiceProvider.GetService<MenuRepository>();
                Menu entity = repository.GetTitle("admin").GetAwaiter().GetResult();
                if (entity == null)
                {
                    entity = new Menu()
                    {
                        title = "Admin",
                        type = "group",
                        icon = "heroicons_outline:cloud",
                        roles = new List<MenuIdentityRole>() { new MenuIdentityRole() { role = role } },
                        children = new List<Menu>()
                        {
                            new Menu()
                            {
                                link = "/menu",
                                title = "Menu",
                                type = "basic",
                                icon = "heroicons_outline:cloud",
                                roles = new List<MenuIdentityRole>() { new MenuIdentityRole() { role = role } },
                            }
                        }
                    };
                    repository.InsertAsync(entity).GetAwaiter().GetResult();
                }

                entity = repository.GetTitle("cadastros").GetAwaiter().GetResult();
                if (entity == null)
                {
                    entity = new Menu()
                    {
                        title = "Cadastros",
                        type = "group",
                        icon = "heroicons_outline:home",
                        children = new List<Menu>()
                        {
                            new Menu()
                            {
                                title = "Serviços",
                                type = "basic",
                                icon = "heroicons_outline:pencil",
                                link = "/example"
                            }
                        }
                    };
                    repository.InsertAsync(entity).GetAwaiter().GetResult();
                }

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var user = userManager.FindByEmailAsync("haryel@lrpd.com").Result;

                if (user == null)
                {
                    user = new IdentityUser()
                    {
                        Email = "haryel@lrpd.com",
                        UserName = "haryel@lrpd.com",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = "123156",
                        SecurityStamp = new Guid().ToString()
                    };

                    var result = userManager.CreateAsync(user, "123").Result;

                    Usuario u = new Usuario()
                    {
                        email = "haryel@lrpd.com",
                        cpf = "123456",
                        celular = "123156",
                        dataNascimento = new DateTime(1998, 1, 23),
                        IdentityUser = user,
                        nome = "haryel",
                        sobrenome = "assencao",
                        endereco = null
                        /*new Endereco()
                        {
                            numero = "13",
                            complemento = "52F",
                            logradouro = serviceScope.ServiceProvider.GetService<LogradouroRepository>().GetCep("11045400")
                        }*/
                    };

                    serviceScope.ServiceProvider.GetService<UsuarioRepository>().InsertAsync(u).GetAwaiter().GetResult();
                }

                if (!userManager.IsInRoleAsync(user, "ADMIN").Result)
                {
                    var result = userManager.AddToRoleAsync(user, "ADMIN").Result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }
    }
}

