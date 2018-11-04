using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;

namespace GameATron4000
{
    public class BotServices
    {
        /// Gets the set of LUIS Services used.
        /// LuisServices is represented as a dictionary.  
        public Dictionary<string, LuisRecognizer> LuisServices { get; } = new Dictionary<string, LuisRecognizer>();

        /// Initializes a new instance of the BotServices class
        public BotServices(BotConfiguration botConfiguration)
        {
            foreach (var service in botConfiguration.Services)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Luis:
                    {
                        var luis = (LuisService)service;
                        if (luis == null)
                        {
                            throw new InvalidOperationException("The LUIS service is not configured correctly in your '.bot' file.");
                        }

                        // get secrets from environment variables (don't push to Github)
                        var appId = Environment.GetEnvironmentVariable("GAMEATRON4000_LUIS_APPID");
                        var key = Environment.GetEnvironmentVariable("GAMEATRON4000_LUIS_KEY");
                        var ep = Environment.GetEnvironmentVariable("GAMEATRON4000_LUIS_EP");

                        var app = new LuisApplication(appId, key, ep);
                        var recognizer = new LuisRecognizer(app);
                        this.LuisServices.Add(luis.Name, recognizer);
                        break;
                    }
                }
            }
        }
    }
}