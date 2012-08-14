using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCGetOrder.Interface
{
	[DataContract]
	public class QSCGetOrderResponseData : IResponseData
	{
		private readonly AtlantisException _ex;
		private getOrderResponseDetail _response;

		public QSCGetOrderResponseData()
		{
		}

		public QSCGetOrderResponseData(RequestData request, Exception ex)
		{
			_ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
		}

		public QSCGetOrderResponseData(AtlantisException aex)
		{
			_ex = aex;
		}

		public QSCGetOrderResponseData(getOrderResponseDetail response)
		{
			_response = response;
		}

		//public QSCStatusCodes responseStatus
		//{
		//  get
		//  {
		//    QSCStatusCodes temp;

		//    if (!Enum.TryParse(_response.responseStatus.statusCode.ToString(), out temp))
		//    {
		//      temp = QSCStatusCodes.FAILURE;
		//    }

		//    return temp;
		//  }
		//}

		[DataMember]
		public getOrderResponseDetail Response
		{
			get { return _response; }
			set { _response = value; }
		}

		#region Implementation of IResponseData

		public string ToXML()
		{
			string xml;
			try
			{
				var serializer = new DataContractSerializer(this.GetType());
				using (var backing = new System.IO.StringWriter())
				using (var writer = new System.Xml.XmlTextWriter(backing))
				{
					serializer.WriteObject(writer, this);
					xml = backing.ToString();
				}
			}
			catch (Exception)
			{
				xml = string.Empty;
			}
			return xml;
		}

		public AtlantisException GetException()
		{
			return _ex;
		}

		#endregion

	}
}
