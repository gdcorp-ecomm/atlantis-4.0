﻿using System.Collections.Generic;

namespace Atlantis.Framework.RuleEngine.Results
{
    public sealed class RuleEngineResult : IRuleEngineResult
    {
        public RuleEngineResult(RuleEngineResultStatus status)
        {
            Status = status;
        }

        private string _exceptionMessage;
        public string ExceptionMessage
        {
            get
            {
                if (_exceptionMessage == null)
                {
                    _exceptionMessage = string.Empty;
                }

                return _exceptionMessage;
            }
            set
            {
                _exceptionMessage = value;
            }
        }

        private IList<IModelResult> _validationResults;
        public IList<IModelResult> ValidationResults
        {
            get
            {
                if (_validationResults == null)
                {
                    _validationResults = new List<IModelResult>(0);
                }

                return _validationResults;
            }
            set
            {
                _validationResults = value;
            }
        }

        public RuleEngineResultStatus Status { get; set; }
    }
}
