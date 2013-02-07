using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RCCGetPLCategoryData.Interface
{
    public enum RCCGetPLCategoryDataResponseState
    {
        Success = 0,
        PartialSuccess = 1,
        Failure = 2
    }

    public class RCCGetPLCategoryDataResponseData : IResponseData
    {
        private AtlantisException _exception = null;
        private RCCGetPLCategoryDataRequestData _request = null;
        private List<PLCategoryDataItemResponse> _responseList = null;
        private string _responseXml = null;
        private RCCGetPLCategoryDataResponseState _state = RCCGetPLCategoryDataResponseState.Success;

        public RCCGetPLCategoryDataResponseData(RequestData request, string responseXml, List<PLCategoryDataItemResponse> plCategoryDataList)
        {
            this._request = request as RCCGetPLCategoryDataRequestData;
            this._responseXml = responseXml;
            this._responseList = plCategoryDataList;

            if (this._responseList.Count == 1)
            {
                if (!this._responseList[0].IsSuccess)
                    this._state = RCCGetPLCategoryDataResponseState.Failure;
                else
                    this._state = RCCGetPLCategoryDataResponseState.Success;
            }
            else
            {
                int numSuccesses =0;
                foreach(PLCategoryDataItemResponse responseItem in this._responseList)
                {
                    if (!responseItem.IsSuccess)
                        this._state = RCCGetPLCategoryDataResponseState.PartialSuccess;
                    else
                        numSuccesses+=1;
                }

                if (this._state == RCCGetPLCategoryDataResponseState.PartialSuccess && numSuccesses == 0)
                    this._state = RCCGetPLCategoryDataResponseState.Failure;
            }

        }

        public RCCGetPLCategoryDataResponseData(RequestData request, string responseXml, AtlantisException exception)
        {
            this._request = request as RCCGetPLCategoryDataRequestData;
            this._responseXml = responseXml;
            this._exception = exception;
            this._state = RCCGetPLCategoryDataResponseState.Failure;
        }

        public RCCGetPLCategoryDataResponseData(RequestData request, System.Net.WebExceptionStatus exceptionStatus)
        {
            this._request = request as RCCGetPLCategoryDataRequestData;
            this._exception = new AtlantisException(request, "RCCGetPLCategoryData.RequestHandler", Enum.GetName(typeof(System.Net.WebExceptionStatus), exceptionStatus), null);
            this._state = RCCGetPLCategoryDataResponseState.Failure;
        }

        public RCCGetPLCategoryDataResponseData(RequestData request, string responseXml, Exception exception)
        {
            this._request = request as RCCGetPLCategoryDataRequestData;
            this._responseXml = responseXml;
            this._exception = new AtlantisException(request, "RCCGetPLCategoryData.RequestHandler", exception.Message, exception.StackTrace);
            this._state = RCCGetPLCategoryDataResponseState.Failure;
        }

        public RCCGetPLCategoryDataResponseState Success()
        {
            return this._state;
        }

        public List<PLCategoryDataItemResponse> PLDataQueryResponseList
        {
            get { return this._responseList; }
        }

        public string ToXML()
        {
            return this._request.ToXML();
        }

        public AtlantisException GetException()
        {
            return this._exception;
        }
    }
}
