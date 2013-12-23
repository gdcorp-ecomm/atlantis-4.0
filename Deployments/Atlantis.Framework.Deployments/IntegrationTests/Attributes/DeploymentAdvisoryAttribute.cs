using System;

namespace Atlantis.Framework.Deployments.IntegrationTests.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public class DeploymentAdvisoryAttribute : Attribute
  {
  }
}