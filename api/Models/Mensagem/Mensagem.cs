using System;
namespace api_lrpd.Models
{
	public class Mensagem
	{
		public enum Tipo
		{
			INFO, WARN, ERROR, FATAL
		}

		public string detail { get; set; }
		public Tipo type { get; set; }
	}
}

