using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Impl.Types
{
  internal class BranchGraph
  {
    public string RootNodeId { get; private set; }
    public string BranchId { get; private set; }

    private readonly Dictionary<string, BranchNode> _nodes;
    public Dictionary<string, BranchNode> BranchNodes
    {
      get
      {
        return _nodes;
      }
    }

    private readonly XElement _branch;

    public BranchGraph(XElement branch)
    {
      _nodes = new Dictionary<string, BranchNode>();
      _branch = branch;
      Build();
    }

    private void Build()
    {
      BranchId = _branch.Attribute("BranchID").Value;
      RootNodeId = _branch.Element("Node").Attribute("NodeID").Value;

      var reachableNodes = new HashSet<string>();
      GetReachableNodes(_branch.Element("Node"), reachableNodes);

      foreach (var node in _branch.Elements("Node"))
      {
        string nodeId = node.Attribute("NodeID").Value;

        if (nodeId != RootNodeId && !reachableNodes.Contains(nodeId))
        {
          continue;
        }

        List<string> transitions = GetNodeTransitions(node);
        BranchNode newNode = BuildBranchNode(node, transitions);

        _nodes[nodeId] = newNode;
      }
    }

    private void GetReachableNodes(XElement reachableNode, HashSet<string> reachableNodes)
    {
      if (!reachableNodes.Add(reachableNode.Attribute("NodeID").Value))
        return;

      var transitionsToFollow = reachableNode.Elements("Transition");
      foreach (var transition in transitionsToFollow)
      {
        string transitionNodeId = transition.Attribute("NodeID").Value;
        XElement matchingNode = _branch.Elements("Node").First(node => node.Attribute("NodeID").Value.Equals(transitionNodeId));
        GetReachableNodes(matchingNode, reachableNodes);
      }
    }

    private BranchNode BuildBranchNode(XElement nodeXml, List<string> transitions)
    {
      string nodeId = nodeXml.Attribute("NodeID").Value;
      int categoryId = XmlConvert.ToInt32(nodeXml.Attribute("CategoryID").Value);
      string unifiedProductId = nodeXml.Attribute("UnifiedProductID").Value;
      int minQuantity = XmlConvert.ToInt32(nodeXml.Attribute("MinQty").Value);
      int maxQuantity = XmlConvert.ToInt32(nodeXml.Attribute("MaxQty").Value);
      int currentQuantity = XmlConvert.ToInt32(nodeXml.Attribute("CurrentQty").Value);
      int minDuration = XmlConvert.ToInt32(nodeXml.Attribute("MinDuration").Value);
      int maxDuration = XmlConvert.ToInt32(nodeXml.Attribute("MaxDuration").Value);
      int increment = XmlConvert.ToInt32(nodeXml.Attribute("Increment").Value);
      bool isQuantityBased = nodeXml.Attribute("IsQuantityBased").Value == "True";
      bool isFinal = nodeXml.Attribute("Final").Value == "True";
      bool isCurrent = nodeXml.Attribute("Path").Value == "True";
      bool isDefault = nodeXml.Attribute("Default").Value == "True";

      return new BranchNode(BranchId, nodeId, categoryId, isCurrent, isFinal, isDefault, currentQuantity,
                            unifiedProductId, isQuantityBased, minQuantity, maxQuantity, minDuration, 
                            maxDuration, increment, transitions);
    }

    private static List<string> GetNodeTransitions(XElement nodeXml)
    {
      var transitions = new List<string>();
      foreach (var nodeTransition in nodeXml.Elements("Transition"))
      {
        transitions.Add(nodeTransition.Attribute("NodeID").Value);
      }
      return transitions;

      
    }
  }
}
