using System;
using System.Reflection;
using api_lrpd.Models;
using Mapster;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;

namespace api_lrpd.Util.Mapper
{
    public static class MapperConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Menu, MenuResponse>
                .NewConfig()
                .Map(dest => dest.roles,
                     src => src.roles.Select(x => x.roleName).ToList())
                .Map(dest => dest.id,
                     src => src.title.RemoverCaracteresEspeciais().Replace(' ', '-').Trim().ToLower())
                .Map(dest => dest.title,
                     src => src.visible ? src.title : $"{src.title} - não visível")
                .AfterMapping(res =>
                {
                    Action<MenuResponse, string> act = null;

                    act = (MenuResponse menu, string paiId) =>
                    {
                        if (!string.IsNullOrWhiteSpace(paiId))
                            menu.id = $"{paiId}.{menu.id}";

                        if (menu.children != null && menu.children.Count > 0)
                            menu.children.ForEach(m => act(m, menu.id));
                    };

                    act(res, null);
                });

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        }
    }
}

