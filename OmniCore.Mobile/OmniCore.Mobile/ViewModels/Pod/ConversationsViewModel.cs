﻿using OmniCore.Mobile.Base;
using OmniCore.Model.Enums;
using OmniCore.Model.Eros;
using OmniCore.Model.Interfaces;
using OmniCore.Model.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OmniCore.Mobile.ViewModels.Pod
{
    public class ConversationsViewModel : PageViewModel
    {
        private const int MAX_RECORDS = 10;

        public ObservableCollection<ResultViewModel> Results { get; set; }

        public ConversationsViewModel(Page page):base(page)
        {
        }

        protected async override Task<BaseViewModel> BindData()
        {
            Results = new ObservableCollection<ResultViewModel>();
            var history = await ErosRepository.Instance.GetHistoricalResultsForDisplay(MAX_RECORDS).ConfigureAwait(true);
            foreach (var result in history)
            {
                Results.Add((ResultViewModel)await new ResultViewModel(result).DataBind());
            }

            MessagingCenter.Subscribe<IMessageExchangeResult>(this, MessagingConstants.NewResultReceived,
                async (newResult) =>
                {
                    await AddNewResult(newResult);
                });

            return this;
        }

        protected override void OnDisposeManagedResources()
        {
            MessagingCenter.Unsubscribe<IMessageExchangeResult>(this, MessagingConstants.NewResultReceived);
            foreach (var result in Results)
                result.Dispose();
        }

        private async Task AddNewResult(IMessageExchangeResult newResult)
        {
            await OmniCoreServices.Application.RunOnMainThread(() =>
            {
                if (Results.Count > 0)
                    Results.Insert(0, new ResultViewModel(newResult));
                else
                    Results.Add(new ResultViewModel(newResult));

                if (Results.Count > MAX_RECORDS)
                    Results.RemoveAt(Results.Count - 1);
            });
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.IsFinished))]
        public string ConversationTitle
        {
            get
            {
                var b = Pod?.ActiveConversation?.IsFinished;
                if (!b.HasValue)
                    return "No active conversation";
                else if (b.Value)
                    return "Last Conversation";
                else
                    return "Active Conversation";
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.Intent))]
        public string ConversationIntent
        {
            get
            {
                return Pod?.ActiveConversation?.Intent;
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.Started))]
        public string Started
        {
            get
            {
                return Pod?.ActiveConversation?.Started.ToLocalTime().ToString("hh:mm:ss");
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.Ended))]
        public string Ended
        {
            get
            {
                return Pod?.ActiveConversation?.Ended?.ToLocalTime().ToString("hh:mm:ss");
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.RequestSource))]
        public string StartedBy
        {
            get
            {
                switch (Pod?.ActiveConversation?.RequestSource)
                {
                    case RequestSource.AndroidAPS:
                        return "Android APS";
                    case RequestSource.OmniCoreUser:
                        return "OmniCore User";
                    case RequestSource.OmniCoreRemoteUser:
                    case RequestSource.OmniCoreAID:
                    default:
                        return "";
                }
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Waiting))]
        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Finished))]
        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Running))]
        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Result), nameof(IMessageExchangeResult.RequestTime))]
        public string RequestPhase
        {
            get
            {
                var exchangeProgress = Pod?.ActiveConversation?.CurrentExchange;
                if (exchangeProgress == null)
                    return string.Empty;
                else
                {
                    if (exchangeProgress.Waiting)
                        return "Waiting to be run";
                    if (exchangeProgress.Finished)
                        return "Finished";
                    if (exchangeProgress.Running)
                    {
                        var t = exchangeProgress.Result.RequestTime;
                        if (t.HasValue)
                        {
                            var diff = DateTimeOffset.UtcNow - t.Value;
                            if (diff.TotalSeconds < 4)
                                return $"Running";
                            else
                                return $"Running for {diff.TotalSeconds:F0} seconds";
                        }
                        else
                            return $"Running";
                    }
                    return "Unknown";
                }
            }
        }

        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Finished))]
        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Result), nameof(IMessageExchangeResult.Success))]
        [DependencyPath(nameof(Pod), nameof(IPod.ActiveConversation), nameof(IConversation.CurrentExchange), nameof(IMessageExchangeProgress.Result), nameof(IMessageExchangeResult.Failure))]
        public string ExchangeActionResult
        {
            get
            {
                var exchangeProgress = Pod?.ActiveConversation?.CurrentExchange;
                if (exchangeProgress == null)
                    return string.Empty;
                else if (exchangeProgress.Finished)
                {
                    if (exchangeProgress.Result.Success)
                        return "Result received";
                    else
                        return $"Messsage exchange failed: {exchangeProgress.Result.Failure}";
                }
                else
                {
                    return exchangeProgress.ActionText;
                }
            }
        }
    }
}
