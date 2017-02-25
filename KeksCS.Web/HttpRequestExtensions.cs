using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace KeksCS.Web
{
	public static class HttpRequestExtensions
	{
		public static string GetRealUserHostAddress(this HttpRequestBase self, bool mapIPv6ToIPv4 = false)
		{
			return GetRealUserHostAddress(self.UserHostAddress, self.Headers, mapIPv6ToIPv4);
		}

		public static string GetRealUserHostAddress(this HttpRequest self, bool mapIPv6ToIPv4 = false)
		{
			return GetRealUserHostAddress(self.UserHostAddress, self.Headers, mapIPv6ToIPv4);
		}

		private static string GetRealUserHostAddress(string userHostAddress, NameValueCollection headers, bool mapIPv6ToIPv4)
		{
			string[] headerNames = { 
				"X-Forwarded-For"
			};
			
			//перебираем все хедеры
			foreach (var headerName in headerNames)
			{
				var hValue = headers[headerName];
				if(!String.IsNullOrEmpty(hValue))
				{
					//если хедер не пустой, то разбиваем его на части
					var subValues = hValue.Split(',');

					//проверяем кажду часть начиная слева
					foreach(var subValue in subValues)
					{
						string ipToParse = subValue.Trim();
						if(ipToParse.Length > 0 && ipToParse != "unknown")
						{
							//как только находим какой-то IP, пытаемся распарсить.
							IPAddress ip = null;
							if (IPAddress.TryParse(ipToParse, out ip))
							{
								if (mapIPv6ToIPv4 && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
								{
									ip = ip.MapToIPv4();
								}
								return ip.ToString();
							}
							else
							{
								//Если не получилось - ошибка. Это так сделано что бы собирать значения хедеров, которые я не смог обработать и возможно
								//добавлять обработку.
								throw new ApplicationException("Can't parse IP from headerName " + headerName + ". Value - " + hValue);
							}
						}
					}
				}
			}

			//если ни один не найден - возвращаем UserHostAddress
			if (mapIPv6ToIPv4)
			{
				IPAddress ip = IPAddress.Parse(userHostAddress);
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
				{
					ip = ip.MapToIPv4();
				}

				return ip.ToString();
			}
			else
			{
				return userHostAddress;
			}
		}
	}
}