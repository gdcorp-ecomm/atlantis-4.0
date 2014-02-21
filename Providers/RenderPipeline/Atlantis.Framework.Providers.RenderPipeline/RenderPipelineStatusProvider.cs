using System;
using System.Collections.ObjectModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.RenderPipeline
{
  public class RenderPipelineStatusProvider : ProviderBase, IRenderPipelineStatusProvider
  {
    internal Lazy<Collection<IRenderPipelineStatus>> _statuses;



    public RenderPipelineResult Status { get; private set; }



    public RenderPipelineStatusProvider(IProviderContainer container)
      : base(container)
    {
      Status = RenderPipelineResult.Success;
      _statuses = new Lazy<Collection<IRenderPipelineStatus>>();
    }



    public void Reset()
    {
      _statuses.Value.Clear();
      Status = RenderPipelineResult.Success;
    }

    public IEnumerable<IRenderPipelineStatus> GetStatuses()
    {
      return _statuses.Value;
    }

    public void Add(IRenderPipelineStatus renderPipelineStatus)
    {
      switch (renderPipelineStatus.Status)
      {
        case RenderPipelineResult.Success:
          break;
        
        case RenderPipelineResult.SuccessWithErrors:
          if (Status != RenderPipelineResult.Error)
          {
            Status = RenderPipelineResult.SuccessWithErrors;
          }
          break;

        case RenderPipelineResult.Error:
          Status = RenderPipelineResult.Error;
          break;
      }

      _statuses.Value.Add(renderPipelineStatus);
    }

    public IRenderPipelineStatus CreateNewStatus(RenderPipelineResult status, string source, IEnumerable<AtlantisException> exceptions = null, IEnumerable<KeyValuePair<string, string>> data = null)
    {
      return new RenderPipelineStatus(status, source, exceptions, data);
    }
  }
}
