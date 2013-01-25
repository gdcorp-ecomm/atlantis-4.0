using System;
using System.Net;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RCCUpdatePLCategoryData.Interface
{
    public enum UpdatePLCategoryDataResponseType
    {
        Success = 0,
        PartialSuccess = 1,
        Failure = 2
    }

    public class RCCUpdatePLCategoryDataResponseData : IResponseData
    {
        private AtlantisException _exception = null;
        private RequestData _requestData = null;
        private string _responseXml = string.Empty;
        private UpdatePLCategoryDataResponseType _isSuccessful = UpdatePLCategoryDataResponseType.Success;
        private List<PLDataItem> _exceptionList = new List<PLDataItem>();

        public RCCUpdatePLCategoryDataResponseData(RequestData request, string responseXml)
        {
            this._requestData = request;
            this._responseXml = responseXml;
        }

        public RCCUpdatePLCategoryDataResponseData(RequestData request, List<PLDataItem> exceptionList)
        {
            this._requestData = request;

            if (exceptionList != null && exceptionList.Count > 0)
            {
                this._exceptionList = exceptionList;
                this._isSuccessful = UpdatePLCategoryDataResponseType.PartialSuccess;
            }
        }

        public RCCUpdatePLCategoryDataResponseData(RequestData request, AtlantisException exception)
        {
            this._isSuccessful = UpdatePLCategoryDataResponseType.Failure;
            this._requestData = request;
            this._exception = exception;
        }

        public RCCUpdatePLCategoryDataResponseData(RequestData request, WebExceptionStatus exception)
        {
            this._isSuccessful = UpdatePLCategoryDataResponseType.Failure;
            this._requestData = request;
            this._exception = new AtlantisException(request, "RCCUpdatePLCategoryDataResponseData", exception.ToString(), null);
        }

        public UpdatePLCategoryDataResponseType IsSuccessful()
        {
            return this._isSuccessful; 
        }

        public string ToXML()
        {
            return this._responseXml;
        }

        public List<PLDataItem> GetPartialSuccessExceptionList()
        {
            return this._exceptionList;
        }

        public AtlantisException GetException()
        {
            return this._exception;
        }
    }
}
