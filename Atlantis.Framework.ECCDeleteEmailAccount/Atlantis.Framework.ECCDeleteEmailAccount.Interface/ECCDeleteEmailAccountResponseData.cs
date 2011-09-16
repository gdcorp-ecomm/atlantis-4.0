using System;
using Atlantis.Framework.Ecc.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetEmailAccount.Interface
{
	public class ECCDeleteEmailAccountResponseData: EccResponseDataBase<object>
	{

		public ECCDeleteEmailAccountResponseData(string resultJson) : base(resultJson)
		{
		}


		public ECCDeleteEmailAccountResponseData(AtlantisException atlantisException) : base(atlantisException)
		{
		}

    public ECCDeleteEmailAccountResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }
	}
}
