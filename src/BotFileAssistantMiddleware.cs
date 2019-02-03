using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000;
using GameATron4000.Configuration;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

public class BotFileAssistantMiddleware : IMiddleware
{
    private static readonly HttpClient HttpClient = new HttpClient();

    private static readonly Regex CommandPattern = new Regex(
        @"^setup (?<environment>.*?) (?<instance>.*?) (?<endpoint>https\:\/\/.*?)$",
        RegexOptions.IgnoreCase);

    public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (turnContext.Activity.Type is ActivityTypes.Message)
        {
            var match = CommandPattern.Match(turnContext.Activity.Text);
            if (match.Success)
            {
                await turnContext.SendActivityAsync(
                    "Ok, I'll configure the Azure Bot Service for you. This will take a couple of seconds, please wait...");

                var environment = match.Groups["environment"].Value;
                var instance = match.Groups["instance"].Value;
                var endpoint = match.Groups["endpoint"].Value;

                // Fix the endpoint if the user didn't postfix the API path.
                if (!endpoint.ToLowerInvariant().EndsWith("/api/messages"))
                {
                    endpoint += "/api/messages";
                }

                // Call the bot file assistant to give us a link to the .bot file.
                var requestUrl = $"https://gameatronbotfileassistant.azurewebsites.net/api/HttpTrigger?env={environment}&instance={instance}&endpoint={endpoint}";
                var botFileUrl = await HttpClient.GetStringAsync(requestUrl);

                await turnContext.SendActivityAsync(
                    "Here's your personal .bot file. Save it over the existing GameATron4000.Development.bot file in the /src folder.");

                // Send the .bot file to the user as an attachment.
                await turnContext.SendActivityAsync(MessageFactory.Attachment(
                    new Attachment
                    {
                        ContentType = "application/json",
                        ContentUrl = botFileUrl.Trim('"'),
                        Name = "GameATron4000.Development.bot",
                        
                    }
                ));

                // We're done with this turn.
                return;
            }
        }
        
        // Call next middleware.
        await next(cancellationToken).ConfigureAwait(false);
    }
}