using System;
using api_lrpd.Models;

namespace api_lrpd.Models
{
	public class MenuResponse
	{
        public string id { get; set; }

        public string title { get; set; } //'Example',
        public string type { get; set; } //'basic',
        public string icon { get; set; } //'heroicons_outline:chart-pie',
        public string link { get; set; } //'/example'

        public List<string> roles { get; set; }
        public List<MenuResponse> children { get; set; }
    }
}

