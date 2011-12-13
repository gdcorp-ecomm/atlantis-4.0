using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommScheduleAdd.Interface
{
    public class EcommScheduleAddResponseData : IResponseData
    {
        public bool ShopperHadPreExistingSchedule { get; set; }

        private AtlantisException _exception = null;

        public bool IsSuccess
        {
            get { return ShopperHadPreExistingSchedule == false && _exception == null; }
        }

        public EcommScheduleAddResponseData(bool shopperHadPreExistingSchedule)
        {
            ShopperHadPreExistingSchedule = shopperHadPreExistingSchedule;
        }

        public EcommScheduleAddResponseData(AtlantisException exAtlantis)
        {
            _exception = exAtlantis;
        }

        public EcommScheduleAddResponseData(RequestData oRequestData, Exception ex)
        {
            _exception = new AtlantisException(oRequestData, "EcommScheduleAddResponseData", ex.Message, string.Empty);
        }

        #region IResponseData Members

        public string ToXML()
        {
            StringBuilder sbResult = new StringBuilder();
            XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbResult));

            xtwRequest.WriteStartElement("response");
            xtwRequest.WriteAttributeString("success", IsSuccess.ToString());
            xtwRequest.WriteAttributeString("ShopperHadPreExistingSchedule", ShopperHadPreExistingSchedule.ToString());
            xtwRequest.WriteEndElement();

            return sbResult.ToString();
        }

        public AtlantisException GetException()
        {
            return _exception;
        }

        #endregion


    }
}
