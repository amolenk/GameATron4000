# Trial 2: Add text translation middleware

In this module you'll learn how to use middleware to intercept messages sent from the bot. We'll use the custom middleware to translate the text in the game using the Microsoft Translator Text API. Microsoft Translator Text API is a cloud-based machine translation service. With this API you can translate text in near real-time from any app or service through a simple REST API call. 

## Configure Microsoft Translator Text API key

1. First obtain a key following the instructions in the [Microsoft Translator Text API documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-text-how-to-signup).

2. Use the **msbot** command line utility to add the key to the GameATron4000.Development.bot file (make sure you're running the command from the `/src` folder):

	```
	msbot connect generic --name Translator --url "no-url" --keys "{\"key\":\"<API key>\"}" --bot GameATron4000.Development.bot
	```

3. In `/src/Core/BotServices.cs` replace the line `// TODO Trial 2: Read translator key from configuration.` with the following code-snippet to read the API key from the .bot file:

	```csharp
	if (service.Name == "Translator")
	{
	    var translatorService = (GenericService)service;
	    TranslatorKey = translatorService.Configuration["key"];
	}
	```

## Add middleware to the bot

1. In the *Translator* folder, add a new *TranslatorMiddleware.cs* file with the following class:

	```csharp
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Threading.Tasks;
	using GameATron4000;
	using GameATron4000.Configuration;
	using GameATron4000.Translator;
	using Microsoft.Bot.Builder;
	using Microsoft.Bot.Schema;
	
	public class TranslatorMiddleware : IMiddleware
	{
	    private readonly TranslatorOptions _options;
	    private readonly TranslatorClient _client;
	
	    public TranslatorMiddleware(TranslatorOptions options, BotServices connectedServices)
	    {
	        _options = options;
	        _client = new TranslatorClient(connectedServices.TranslatorKey);
	    }
	
	    public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default(CancellationToken))
	    {
	        // TODO Insert code to translate sent message activities here.
	        
	        // Call next middleware.
	        await next(cancellationToken).ConfigureAwait(false);
	    }
	}
	```

The `IMiddleware` interface declares an `OnTurnAsync` method where we can place our code to intercept the bot activities.  

2. Replace the line `// TODO Insert code to translate sent message activities here.` in the `OnTurnAsync` method with the following code-snippet:

	```csharp
	if (_options.Enabled)
	{
	    turnContext.OnSendActivities(async (context, activities, nextSend) =>
	    {
	        IEnumerable<Task> translatorTasks = activities
	            .Where(a => a.Type == ActivityTypes.Message)
	            .Select(a => a.AsMessageActivity())
	            .Select(async activity =>
	            {
	                // The activity also contains the name of the actor, we don't want to translate those.
	                var match = Regex.Match(activity.Text, "(?<ActorName>.*?) > (?<Text>.*)");
	                if (match.Success)
	                {
	                    var translatedText = await _client.TranslateAsync(match.Groups["Text"].Value, _options.Language)
	                        .ConfigureAwait(false);
	
	                    activity.Text = $"{match.Groups["ActorName"].Value} > {translatedText}";
	                }
	            });
	
	        await Task.WhenAll(translatorTasks).ConfigureAwait(false);
	
	        return await nextSend();
	    });
	}
	```

This code listens to all activities sent by the bot, filters out the Message activities and then translates the text within the Message activities using the Translator Text API. 

3. Now we just need to register the middleware with the bot. In *Startup.cs* replace the line `// TODO Trial 2: Register middleware here.` with the following code-snippet:

	```csharp
	options.Middleware.Add(new TranslatorMiddleware(translatorOptions, connectedServices));
	```

## Run the game

1. Open `appsettings.Development.json` and enable the Translator. You can also specify the target language here.

	```json
	  "Translator": {
	    "Enabled": true,
	    "Language": "nl"
	  },
	```

2. In Visual Studio Code, restart the debugger to let the bot use the new settings.

2. In a browser window, navigate to *https://localhost:5000/*.

    The game GUI will now load and any Message activities sent from the bot will be translated. Note that not all text in the game is translated yet. For example, the user still needs to input the commands in English. This is mainly because user input must exactly match the scripted commands for the game to react. In the next lab, we'll look at LUIS to improve the bot to accept a more diverse range of commands.