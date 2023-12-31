﻿using System.Collections.Generic;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Interface
{
  public sealed class DomainTransferCollection
  {
    private readonly bool _status;
    private readonly int _resultCount;
    private readonly int _totalPages;
    private readonly List<DomainTransfer> _domainTransferList;

    public int ResultCount { get { return _resultCount; } }
    public int TotalPages { get { return _totalPages; } }
    public bool IsSuccess { get { return _status; } }

    public DomainTransferCollection(bool callStatus, int resultCount, int totalPages)
    {
      _status = callStatus;
      _resultCount = resultCount;
      _totalPages = totalPages;
      _domainTransferList = new List<DomainTransfer>();
    }

    public List<DomainTransfer> DomainTransferList
    {
      get { return _domainTransferList; }
    }
  }
}
