using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Atlantis.Framework.BonsaiGetPlanOptions.Impl.Types;
using Atlantis.Framework.BonsaiGetPlanOptions.Interface;
using Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types;
using Atlantis.Framework.GetAccountXML.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Impl
{
  public class BonsaiGetPlanOptionsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var planOptionsRequest = (BonsaiGetPlanOptionsRequestData)requestData;
      var accountXmlResponseData = GetAccountXmlResponseData(planOptionsRequest);
      
      BonsaiGetPlanOptionsResponseData planOptionsResponse;
      if (accountXmlResponseData.AtlException == null)
      {
        try
        {
          planOptionsResponse = BuildPlanOptionsResponseData(accountXmlResponseData.AccountXML);
        }
        catch (Exception ex)
        {
          var atlEx = new AtlantisException(requestData, "RequestHandler", "Error building PlanOptionsResponseData", string.Empty, ex);
          planOptionsResponse = new BonsaiGetPlanOptionsResponseData(atlEx);
        }
      }
      else
      {
        planOptionsResponse = new BonsaiGetPlanOptionsResponseData(accountXmlResponseData.AtlException);
      }

      return planOptionsResponse;
    }

    private static GetAccountXMLResponseData GetAccountXmlResponseData(BonsaiGetPlanOptionsRequestData planOptionsRequest)
    {
      var requestData = new GetAccountXMLRequestData(planOptionsRequest.ShopperID,
                                                     planOptionsRequest.SourceURL,
                                                     planOptionsRequest.OrderID,
                                                     planOptionsRequest.Pathway,
                                                     planOptionsRequest.PageCount,
                                                     planOptionsRequest.ResourceId,
                                                     planOptionsRequest.ResourceType,
                                                     planOptionsRequest.IdType,
                                                     planOptionsRequest.TreeId,
                                                     planOptionsRequest.PrivateLabelId);
      
      var response = (GetAccountXMLResponseData)Engine.Engine.ProcessRequest(requestData, planOptionsRequest.BonsaiGetAccountXmlRequestType);
      return response;
    }

    private static BonsaiGetPlanOptionsResponseData BuildPlanOptionsResponseData(string accountXml)
    {
      if (String.IsNullOrEmpty(accountXml))
      {
        throw new ArgumentException("AccountXml is empty");
      }

      XElement currentTreeXml = GetCurrentTreeXml(accountXml);
      if (currentTreeXml == null)
      {
        throw new Exception("AccountXml does not contain the current tree information");
      }

      List<ProductPlan> plans = GetProductPlans(currentTreeXml);
      List<FilteredProductPlan> filteredPlans = GetFilteredProductPlans(currentTreeXml);
      CategoryAddonCollection addons = GetProductAddons(currentTreeXml);
      List<PrepaidAddon> prepaids = GetPrepaidAddons(currentTreeXml);

      return new BonsaiGetPlanOptionsResponseData(accountXml, plans, filteredPlans, addons, prepaids);
    }

    private static XElement GetCurrentTreeXml(string accountXml)
    {
      try
      {
        var doc = XDocument.Parse(accountXml, LoadOptions.None);
        return doc.Root.Element("Bonsai").Element("Tree");
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to parse AccountXml", ex);
      }
    }

    private static List<ProductPlan> GetProductPlans(XElement currentTreeXml)
    {
      const string TREEID_ATTR = "TreeID";
      const string UNIFIED_PRODUCTID_ATTR = "UnifiedProductID";
      const string ISFREE_ATTR = "IsFree";

      if (currentTreeXml == null)
      {
        throw new ArgumentNullException("currentTreeXml");
      }

      string treeId = currentTreeXml.Attribute(TREEID_ATTR).Value;
      string productId = currentTreeXml.Attribute(UNIFIED_PRODUCTID_ATTR).Value;
      bool isFree;
      bool.TryParse((currentTreeXml.Attribute(ISFREE_ATTR) ?? new XAttribute(ISFREE_ATTR, "False")).Value, out isFree);

      var plans = new List<ProductPlan> {new ProductPlan(treeId, productId, isFree, isCurrent:true)};

      var transitions = currentTreeXml.Elements("Transition");
      foreach (var transition in transitions)
      {
        bool isFreeTransition;
        bool.TryParse((transition.Attribute(ISFREE_ATTR) ?? new XAttribute(ISFREE_ATTR, "False")).Value, out isFreeTransition);

        plans.Add(new ProductPlan(transition.Attribute(TREEID_ATTR).Value, transition.Attribute(UNIFIED_PRODUCTID_ATTR).Value, isFreeTransition, isCurrent:false));
      }

      return plans;
    }

    private static List<FilteredProductPlan> GetFilteredProductPlans(XElement currentTreeXml)
    {
      if (currentTreeXml == null)
      {
        throw new ArgumentNullException("currentTreeXml");
      }

      var filteredPlans = new List<FilteredProductPlan>();
      var filteredTransitions = currentTreeXml.Element("FilteredTransitions").Elements("FilteredTransition");
      foreach (var filteredTransition in filteredTransitions)
      {
        string treeId = filteredTransition.Attribute("TreeID").Value;
        string productId = filteredTransition.Attribute("UnifiedProductID").Value;
        bool isFree;
        bool.TryParse((filteredTransition.Attribute("IsFree") ?? new XAttribute("IsFree", "False")).Value, out isFree);

        var reason = filteredTransition.Element("Reason");
        int reasonCode = int.Parse(reason.Attribute("MessageCode").Value);
        string reasonMessage = reason.Attribute("Message").Value;

        filteredPlans.Add(new FilteredProductPlan(treeId, productId, reasonCode, reasonMessage, isFree));
      }

      return filteredPlans;
    }

    private static CategoryAddonCollection GetProductAddons(XElement currentTreeXml)
    {
      if (currentTreeXml == null)
      {
        throw new ArgumentNullException("currentTreeXml");
      }

      var addons = new CategoryAddonCollection();

      var branches = currentTreeXml.Elements("Branch");
      foreach (var branch in branches)
      {
        var rootAddonNode = branch.Element("Node");
        if (rootAddonNode == null)
        {
          continue;
        }

        var graph = new BranchGraph(branch);
        ProductAddon branchRoot = BuildAddonRecursive(graph, rootAddonNode.Attribute("NodeID").Value);
        addons.AddProductAddons(branchRoot.CategoryId, branchRoot.ChildAddons[branchRoot.CategoryId]);
      }

      return addons;
    }

    private static List<PrepaidAddon> GetPrepaidAddons(XElement currentTreeXml)
    {
      if (currentTreeXml == null)
      {
        throw new ArgumentNullException("currentTreeXml");
      }

      var prepaids = new List<PrepaidAddon>();

      var prepaidItems = currentTreeXml.Elements("Prepaid").Elements("Item");
      foreach(var prepaidItem in prepaidItems)
      {
        if (prepaidItem == null)
        {
          continue;
        }

        prepaids.Add(new PrepaidAddon(prepaidItem.Attribute("Name").Value,
                                      prepaidItem.Attribute("UnifiedProductID").Value,
                                      bool.Parse(prepaidItem.Attribute("IsQuantityBased").Value),
                                      int.Parse(prepaidItem.Attribute("MinQty").Value),
                                      int.Parse(prepaidItem.Attribute("MaxQty").Value),
                                      int.Parse(prepaidItem.Attribute("MinDuration").Value),
                                      int.Parse(prepaidItem.Attribute("MaxDuration").Value),
                                      int.Parse(prepaidItem.Attribute("Increment").Value)));
      }

      return prepaids;
    }

    private static ProductAddon BuildAddonRecursive(BranchGraph branchGraph, string addonNodeId)
    {
      if (branchGraph == null)
      {
        throw new ArgumentNullException("branchGraph");
      }
      if (string.IsNullOrEmpty(addonNodeId))
      {
        throw new ArgumentNullException("addonNodeId");
      }

      var addon = branchGraph.BranchNodes[addonNodeId];
      addon.IsVisited = true;

      addon.TransitionNodeIds.ForEach(nodeId =>
                                        {
                                          if (!branchGraph.BranchNodes[nodeId].IsVisited)
                                          {
                                            addon.ChildAddons.AddProductAddon(BuildAddonRecursive(branchGraph, nodeId));
                                          }
                                        });
      foreach (BranchNode branchNode in branchGraph.BranchNodes.Values)
      {
        if (branchNode.IsCurrent && !branchNode.IsVisited && branchNode.TransitionNodeIds.Exists(targetId => targetId == addonNodeId))
        {
          addon.ChildAddons.AddProductAddon(BuildAddonRecursive(branchGraph, branchNode.NodeId));
        }
      }

      return addon.ToProductAddon();
    }

    
  }
}
