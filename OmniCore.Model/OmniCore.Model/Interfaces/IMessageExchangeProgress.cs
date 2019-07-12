﻿using OmniCore.Model.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniCore.Model.Interfaces
{
    public interface IMessageExchangeProgress : INotifyPropertyChanged
    {
        IConversation Conversation { get; }

        string ActionText { get; set; }

        bool CanBeCanceled { get; set; }

        bool Waiting { get; set; }
        bool Running { get; set; }
        bool Finished { get; set; }

        int Progress { get; set; }

        IMessageExchangeResult Result { get; set; }

        CancellationToken Token { get; }
        void SetException(Exception exception);
    }
}
