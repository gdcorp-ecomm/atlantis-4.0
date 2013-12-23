using System;
using Atlantis.Framework.Deployments.IntegrationTests.Attributes;
using Atlantis.Framework.Testing.UnitTesting.Interfaces;

namespace Atlantis.Framework.Deployments.IntegrationTests.Collections
{
  public class AdvisoryTests : IUnitTestCollection
  {
    public Type CollectionAttribute
    {
      get { return typeof(DeploymentAdvisoryAttribute); }
    }
  }
}