using System;
namespace api_lrpd.Models.DTO.Auth
{
	public class ValidarTokenRequest
	{
		public Guid Id { get; set; }
		public string Token { get; set; }
	}
}

