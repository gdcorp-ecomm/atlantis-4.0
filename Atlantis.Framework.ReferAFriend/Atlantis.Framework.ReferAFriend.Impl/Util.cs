using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	internal static class Util
	{
		internal static int GetRequestTimeout(RequestData req, Framework.Interface.ConfigElement config)
		{
			var ts = GetConfigValue<TimeSpan>("RequestTimeout", req.RequestTimeout, config);
			return (int)ts.TotalSeconds;
		}

		internal static T GetConfigValue<T>(string key, T defaultValue, ConfigElement config)
		{
			string s = config.GetConfigValue(key);
			if (!string.IsNullOrEmpty(s))
			{
				if (typeof(T) == typeof(TimeSpan))
				{
					return (T)(object)TimeSpan.Parse(s);
				}
				else if (typeof(T) == typeof(string))
				{
					return (T)(object)s;
				}
				else
				{
					return (T)Convert.ChangeType(s, typeof(T));
				}
			}
			return defaultValue;
		}

		//internal static AtlantisException CreateAtlantisException(RequestData requestData, Exception ex)
		//{
		//	//var o = ex.TargetSite;
		//	//if (o != null)
		//	//{
		//		var trace = new StackTrace(ex);

		//		//var sourceFunction = o.DeclaringType.Name + "::" + o.Name;
		//		var sourceFunction = "";
		//		var myAssembly = typeof(Util).Assembly;
		//		for (int index = 0; index < trace.FrameCount; ++index)
		//		{
		//			StackFrame frame = trace.GetFrame(index);
		//			var method = frame.GetMethod();

		//			if (method.DeclaringType.Assembly == myAssembly && method.DeclaringType != typeof(Util))
		//			{
		//				sourceFunction = method.DeclaringType.Name + "::" + method.Name;
		//				break;
		//			}
		//		}
		//		return null;

		//	//}
		//}
	}
}