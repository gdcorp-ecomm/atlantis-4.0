﻿using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCVoiceAuthCapture.Interface
{
	[DataContract]
	public class QSCVoiceAuthCaptureResponseData : IResponseData
	{
		private readonly AtlantisException _ex;
		private responseDetail response;

		public QSCVoiceAuthCaptureResponseData(RequestData request, Exception ex)
		{
			_ex = new AtlantisException(request, ex.Source, ex.Message, ex.StackTrace, ex);
		}

		public QSCVoiceAuthCaptureResponseData(AtlantisException aex)
		{
			_ex = aex;
		}

		public QSCVoiceAuthCaptureResponseData(responseDetail response)
		{
			this.response = response;
		}

		private QSCStatusCodes responseStatus
		{
			get
			{
				QSCStatusCodes temp;

				if (!Enum.TryParse(response.responseStatus.statusCode.ToString(), out temp))
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
				if (this.response != null)
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
			get { return response; }
			set { response = value; }
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
