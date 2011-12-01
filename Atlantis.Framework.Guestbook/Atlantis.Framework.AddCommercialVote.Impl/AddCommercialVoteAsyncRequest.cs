using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atlantis.Framework.Interface;
using Atlantis.Framework.AddCommercialVote.Interface;

namespace Atlantis.Framework.AddCommercialVote.Impl
{
  public class AddCommercialVoteAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      AddCommercialVoteRequestData oAddCommercialVoteRequestData = (AddCommercialVoteRequestData)oRequestData;

      CommercialVoteAddWS.GuestbookService oCommercialWS = new CommercialVoteAddWS.GuestbookService();
      oCommercialWS.Url = ((WsConfigElement)oConfig).WSURL;
      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oCommercialWS, oState);

      return oCommercialWS.BeginAddCommercialVote(oAddCommercialVoteRequestData.Commercial,
                                          oAddCommercialVoteRequestData.ShopperID,
                                          oAddCommercialVoteRequestData.ClientIp,
                                          oCallback,
                                          oAsyncState);
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;

      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        CommercialVoteAddWS.GuestbookService oCommercialWS = (CommercialVoteAddWS.GuestbookService)oAsyncState.Request;
        oResponseData = new AddCommercialVoteResponseData(oCommercialWS.EndAddCommercialVote(oAsyncResult));
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AddCommercialVoteResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        AtlantisException aEx = new AtlantisException(
          oAsyncState.RequestData, "AddCommercialVoteResponseData", ex.Message, oAsyncState.RequestData.ToXML(), ex);
        oResponseData = new AddCommercialVoteResponseData(aEx);
      }

      return oResponseData;
    }

    #endregion
  }
}
