using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SydMeetup.PlacesDirectory.Bot.IntentProviders;
using SydMeetup.PlacesDirectory.Bot.LUIS;
using System;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            IIntentProvider intentProvider = new NoProvider();

            var response = await LUISProvider.GetEntityFromLUIS(message.Text);

            if (response.TopScoringIntent != null)
            {
                switch (response.TopScoringIntent.Intent)
                {
                    case "Places.GetHours":
                        intentProvider = new GetHourProvider(new Connector.CmsConnector());
                        break;

                    case "Places.GetAddress":
                        intentProvider = new GetAddressProvider(new Connector.CmsConnector());
                        break;
                }
            }

            var providerResult = await intentProvider.Execute(response.Entities);
            await context.PostAsync(providerResult);
            context.Wait(MessageReceivedAsync);
        }
    }
}