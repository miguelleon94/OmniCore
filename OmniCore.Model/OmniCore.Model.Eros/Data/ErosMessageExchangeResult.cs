﻿using OmniCore.Model.Enums;
using OmniCore.Model.Exceptions;
using OmniCore.Model.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using OmniCore.Model.Interfaces.Data;
using OmniCore.Model.Utilities;
using OmniCore.Mobile.Base;

namespace OmniCore.Model.Eros.Data
{
    public class ErosMessageExchangeResult : PropertyChangedImpl, IMessageExchangeResult
    {
        public ErosMessageExchangeResult()
        {
        }

        public ErosMessageExchangeResult(IMessageExchangeProgress exchangeProgress)
        {
            ExchangeProgress = exchangeProgress;
        }

        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }

        public Guid PodId { get; set; }

        public DateTimeOffset? RequestTime { get; set; }
        [Indexed]
        public DateTimeOffset? ResultTime { get; set; }

        public RequestSource Source { get; set; }
        public RequestType Type { get; set; }
        public string Parameters { get; set; }

        public bool Success { get; set; }

        public FailureType Failure { get; set; }

        [Ignore]
        public Exception Exception { get; set; }

        public long? StatisticsId { get; set; }
        [Ignore]
        public IMessageExchangeStatistics Statistics { get; set; }

        public long? ParametersId { get; set; }
        [Ignore]
        public IMessageExchangeParameters ExchangeParameters { get; set; }

        public long? AlertStatesId { get; set; }
        [Ignore]
        public IAlertStates AlertStates { get; set; }

        public long? BasalScheduleId { get; set; }
        [Ignore]
        public IBasalSchedule BasalSchedule { get; set; }

        public long? FaultId { get; set; }
        [Ignore]
        public IFault Fault { get; set; }

        public long? StatusId { get; set; }
        [Ignore]
        public IStatus Status { get; set; }

        public long? UserSettingsId { get; set; }
        [Ignore]
        public IUserSettings UserSettings { get; set; }

        [Ignore]
        public IMessageExchangeProgress ExchangeProgress { get; set; }
    }
}
