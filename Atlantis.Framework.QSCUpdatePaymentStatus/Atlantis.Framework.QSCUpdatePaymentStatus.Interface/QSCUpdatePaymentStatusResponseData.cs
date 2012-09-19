using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCUpdatePaymentStatus.Interface
{
	[DataContract]
	public class QSCUpdatePaymentStatusResponseData : IResponseData
	{
		private readonly AtlantisException _ex;
		private responseDetail _response;

		public QSCUpdatePaymentStatusResponseData(RequestData request, Exception ex)
		{
			_ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
		}

		public QSCUpdatePaymentStatusResponseData(AtlantisException aex)
		{
			_ex = aex;
		}

		public QSCUpdatePaymentStatusResponseData(responseDetail response)
		{
			this._response = response;
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
				_isSuccess = false;
				if (this._response != null)
				{
					_isSuccess = (responseStatus == QSCStatusCodes.SUCCESS);
				}

				return _isSuccess;
			}
			set { _isSuccess = value; }
		}

		[DataMember]
		public responseDetail Response
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
			catch (Exception ex)
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
