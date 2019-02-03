using System;
using System.Collections.Generic;
using GameATron4000.Configuration;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;

namespace GameATron4000
{
    public class BotServices
    {
        public string BotId { get; private set; }

        public string DirectLineSecret { get; private set; }

        public string TranslatorKey { get; private set; }

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
                    case ServiceTypes.Bot:
                    {
                        var botService = (BotService)service;
                        BotId = botService.ServiceName;
                        break;
                    }
                    case ServiceTypes.Generic:
                    {
                        if (service.Name == "DirectLine")
                        {
                            var directLineService = (GenericService)service;
                            DirectLineSecret = directLineService.Configuration["secret"];
                        }

                        // TODO Trial 2: Read translator key from configuration.

                        break;
                    }

                    // TODO Trial 3: Add LUIS service
                }
            }
        }
    }
}