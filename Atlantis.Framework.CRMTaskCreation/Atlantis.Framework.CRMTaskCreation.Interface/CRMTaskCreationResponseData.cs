using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRMTaskCreation.Interface
{
    public class CRMTaskCreationResponseData : IResponseData
    {
        private readonly AtlantisException _exception;
        private readonly string _responseXml = string.Empty;
        private string ResponseText;

        public CRMTaskCreationResponseData(string responseXml)
        {
          _responseXml = responseXml;
        }

        public CRMTaskCreationResponseData(AtlantisException exception)
        {
          _exception = exception;
        }

        public CRMTaskCreationResponseData(RequestData requestData, Exception ex)
        {
          _exception = new AtlantisException(requestData, "CRMTaskCreationResponseData", ex.Message, ex.StackTrace);
        }

        public string ParseResponse()
        {
            if (!String.IsNullOrEmpty(_responseXml))
            {
                var responseXml = new XmlDocument();
                string response = "<root>" + _responseXml + "</root>";
                responseXml.LoadXml(response);
                XmlNode responseNode = responseXml.SelectSingleNode("//root/TaskSubmissionStatus");
                if (responseNode != null)
                {
                    ResponseText = responseNode.InnerText;
                }
            }
            return ResponseText;
        }

        public bool IsSuccess
        {
            get { return (ParseResponse() == "Success"); }
        }

        #region IResponseData Members

        public string ToXML()
        {
            return _responseXml;
        }

        public AtlantisException GetException()
        {
            return _exception;
        }

        #endregion
    }
}
