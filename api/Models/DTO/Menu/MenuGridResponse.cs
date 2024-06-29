using System;
namespace api_lrpd.Models.DTO.Menu
{
	public class MenuGridResponse
	{
        public string id { get; set; }

        public string title { get; set; } //'Example',
        public string type { get; set; } //'basic',
        public string icon { get; set; } //'heroicons_outline:chart-pie',
        public string link { get; set; } //'/example'

        //public List<MenuIdentityRole> roles { get; set; }
    }
}

