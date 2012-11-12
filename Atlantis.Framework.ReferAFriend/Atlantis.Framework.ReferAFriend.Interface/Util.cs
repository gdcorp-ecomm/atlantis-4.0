using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	internal static class Util
	{
		public static string GetCacheMD5(params object[] args)
		{
			var oMD5 = new MD5CryptoServiceProvider();
			oMD5.Initialize();
			byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(JoinArgs(args, ":")); //string.Format("{0}:{1}:{2}", ShopperID, OrderCount, DaysToSearch));
			byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
			string sValue = BitConverter.ToString(md5Bytes, 0);
			return sValue.Replace("-", "");
		}

		internal static void DeserializeFromXML<T>(string xml, T obj)
		{
			var type = typeof(T);
			if (!string.IsNullOrEmpty(xml))
			{
				var xDoc = XDocument.Parse(xml);
				var xRoot = xDoc.Element(type.Name);

				var propList = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty |
					System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public);
				foreach (var prop in propList)
				{
					var propType = prop.PropertyType;
					if (propType.IsValueType || propType == typeof(string))
					{
						var xAttr = xRoot.Attribute(prop.Name);

						if (xAttr != null)
						{
							object nodeValue = null;
							if (propType == typeof(TimeSpan))
							{
								nodeValue = TimeSpan.Parse(xAttr.Value);
							}
							else if (propType.IsEnum)
							{
								nodeValue = Enum.Parse(propType, xAttr.Value);
							}
							else
							{
								nodeValue = Convert.ChangeType(xAttr.Value, propType);
							}
							prop.SetValue(obj, nodeValue, null);
						}
					}
				}
			}
		}

		internal static string JoinArgs(object[] args, string delim)
		{
			if (args.Length > 0)
			{
				var s = args[0].ToString();
				for (int i = 1; i < args.Length; i++)
				{
					s += delim + args[i].ToString();
				}
				return s;
			}
			return null;
		}

		internal static string SerializeToXML<T>(T obj)
		{
			var type = typeof(T);

			var xDoc = new XDocument();
			var xRoot = new XElement(type.Name);
			xDoc.Add(xRoot);

			var propList = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty |
				System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public);
			foreach (var prop in propList)
			{
				var propType = prop.PropertyType;
				if (propType.IsValueType || propType == typeof(string))
				{
					var nodeValue = prop.GetValue(obj, null);
					if (nodeValue != null)
					{
						xRoot.Add(new XAttribute(prop.Name, nodeValue.ToString()));
					}
				}
			}

			return xDoc.ToString(SaveOptions.DisableFormatting);
		}
	}
}