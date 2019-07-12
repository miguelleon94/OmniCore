﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using OmniCore.Model.Enums;

namespace OmniCore.Model.Interfaces.Data
{
    public interface IPod : INotifyPropertyChanged
    {
        Guid Id { get; set; }
        DateTimeOffset Created { get; set; }

        uint? Lot { get; set; }
        uint? Serial { get; set; }
        uint RadioAddress { get; set; }
        int MessageSequence { get; set; }

        DateTimeOffset? ActivationDate { get; set; }
        DateTimeOffset? InsertionDate { get; set; }
        string VersionPi { get; set; }
        string VersionPm { get; set; }
        string VersionUnknown { get; set; }
        decimal? ReservoirUsedForPriming { get; set; }

        bool Archived { get; set; }

        IConversation ActiveConversation { get; set; }
        IMessageExchangeResult LastTempBasalResult { get; set; }
        IAlertStates LastAlertStates { get; set; }
        IBasalSchedule LastBasalSchedule { get; set; }
        IFault LastFault { get; set; }
        IStatus LastStatus { get; set; }
        IUserSettings LastUserSettings { get; set; }
    }
}