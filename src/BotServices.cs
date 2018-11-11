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
        /// Gets the set of LUIS Services used.
        /// LuisServices is represented as a dictionary.  
        public Dictionary<string, LuisRecognizer> LuisServices { get; } = new Dictionary<string, LuisRecognizer>();

        public string DirectLinkSecret { get; private set; }

        /// Initializes a new instance of the BotServices class
        public BotServices(BotConfiguration botConfiguration, LUISOptions luisOptions)
        {
            foreach (var service in botConfiguration.Services)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Generic:
                    {
                        if (service.Name == "DirectLink")
                        {
                            var directLinkService = (GenericService)service;
                            DirectLinkSecret = directLinkService.Configuration["Secret"];
                        }

                        break;
                    }
                    case ServiceTypes.Luis:
                    {
                        // if (luisOptions.Enabled)
                        // {
                        //     var luis = (LuisService)service;
                        //     if (luis == null)
                        //     {
                        //         throw new InvalidOperationException("The LUIS service is not configured correctly in your '.bot' file.");
                        //     }

                        //     var app = new LuisApplication(luisOptions.AppId, luisOptions.Key, luisOptions.Endpoint);
                        //     var recognizer = new LuisRecognizer(app);
                        //     this.LuisServices.Add(luis.Name, recognizer);
                        // }
                        break;
                    }
                }
            }
        }
    }
}