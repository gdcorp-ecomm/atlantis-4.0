using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCGetOrderHistory.Interface
{
	[DataContract]
	public class QSCGetOrderHistoryResponseData : IResponseData
	{
		private readonly AtlantisException _ex;
		private getOrderHistoryResponseDetail _response;

		public QSCGetOrderHistoryResponseData()
		{
		}

		public QSCGetOrderHistoryResponseData(RequestData request, Exception ex)
		{
			_ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
		}

		public QSCGetOrderHistoryResponseData(AtlantisException aex)
		{
			_ex = aex;
		}

		public QSCGetOrderHistoryResponseData(getOrderHistoryResponseDetail response)
		{
			_response = response;
		}

		[DataMember]
		public getOrderHistoryResponseDetail Response
		{
			get { return _response; }
			set { _response = value; }
		}

		private QSCStatusCodes responseStatus
		{
			get
			{
				QSCStatusCodes temp;

				if (!Enum.TryParse(_response.responseStatus.statusCode.ToString(), out temp))
				{
					temp = QSCStatusCodes.FAILURE;
				}

				return temp;
			}
		}

		private bool _isSuccess;
		[DataMember]
		public bool IsSuccess
		{
			get
			{
				bool _isSuccess = false;
				if (this._response != null)
				{
					_isSuccess = (responseStatus == QSCStatusCodes.SUCCESS);
				}

				return _isSuccess;
			}
			set { _isSuccess = value; }
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
