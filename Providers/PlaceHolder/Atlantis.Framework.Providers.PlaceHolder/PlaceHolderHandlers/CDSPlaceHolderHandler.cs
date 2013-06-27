using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.PlaceHolder
{
    internal class CDSPlaceHolderHandler : IPlaceHolderHandler
    {
        private readonly XmlDataSerializer _xmlDataSerializer = new XmlDataSerializer();

        public string Type
        {
            get { return PlaceHolderTypes.CDSDocument; }
        }

        public string GetPlaceHolderContent(string name, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, Framework.Interface.IProviderContainer providerContainer)
        {
            string renderContent = string.Empty;
            ICDSContentProvider cds;
            if (providerContainer.TryResolve<ICDSContentProvider>(out cds))
            {
                try
                {
                    IPlaceHolderData placeHolderData = DeserializeData(data);
                    renderContent = cds.GetContent(placeHolderData.App, placeHolderData.Location).Content;
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

                    debugContextErrors.Add(errorMessage);
                    ErrorLogger.LogException(errorMessage, "RenderPlaceHolderContent()", data);
                }
            }

            return renderContent;
        }

        private IPlaceHolderData DeserializeData(string data)
        {
            IPlaceHolderData placeHolderData = _xmlDataSerializer.Deserialize<XmlPlaceHolderData>(data);

            if (placeHolderData == null)
            {
                throw new Exception("Unhandled deserialization error, null returned.");
            }

            return placeHolderData;
        }
    }
}
