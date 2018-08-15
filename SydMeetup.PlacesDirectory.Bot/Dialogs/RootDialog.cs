using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SydMeetup.PlacesDirectory.Bot.IntentProviders;
using SydMeetup.PlacesDirectory.Bot.LUIS;
using System;
using System.Threading.Tasks;

namespace SydMeetup.PlacesDirectory.Bot.Dialogs
{
    [Serializable]
    ///IDialog<string></string>
    public class RootDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var userMessage = await result;

            var providerResult = await ProcessUserMessage(userMessage);

            //Post a message to be sent to the user, using previous messages to establish a conversation context.
            await context.PostAsync(providerResult);

            context.Wait(MessageReceivedAsync);
        }

        private static async Task<string> ProcessUserMessage(IMessageActivity message)
        {
            IIntentProvider intentProvider = new NoProvider();

            var response = await LUISProvider.GetEntityFromLUIS(message.Text);

            if (response.TopScoringIntent != null)
            {
                var cmsConnector = new Connector.CmsConnector();

                switch (response.TopScoringIntent.Intent)
                {
                    case "Places.GetHours":
                        intentProvider = new GetHourProvider(cmsConnector);
                        break;

                    case "Places.GetAddress":
                        intentProvider = new GetAddressProvider(cmsConnector);
                        break;

                    case "Places.List":
                        intentProvider = new GetListProvider(cmsConnector);
                        break;
                }
            }

            var providerResult = await intentProvider.Execute(response.Entities);
            return providerResult;
        }
    }
}