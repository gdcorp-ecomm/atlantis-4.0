using System;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.ProximaDomainName.Interface
{
    public class ProximaDomainNameResponseData : IResponseData
    {
        private AtlantisException _ex;
        private bool _isDeluxe = false;
        private string _xml = string.Empty;

        public bool IsDeluxe
        {
            get
            {
                return _isDeluxe;
            }
        }

        public ProximaDomainNameResponseData(string xml)
        {
            _xml = xml;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlElement root = xmlDoc.DocumentElement;

            _isDeluxe = (root.GetAttribute("Active") != "0");
        }

        public ProximaDomainNameResponseData(RequestData oRequestData, Exception ex)
        {
            _ex = new AtlantisException(oRequestData, 
                                        "ProximaDomainNameResponseData", 
                                        ex.Message.ToString(), 
                                        oRequestData.ToXML());
        }

        #region IResponseData Members

        public string ToXML()
        {
            return _xml;
        }

        public AtlantisException GetException()
        {
            return _ex;
        }

        #endregion IResponseData Members
    }
}