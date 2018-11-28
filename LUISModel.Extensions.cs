using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luis
{
    public static class LUISModelExtensions
    {
        public static string ToCommand(this LUISModel recognizerResult)
        {
            // parse LUIS results to get intent and entity
            string intent = GetLUISIntent(recognizerResult);
            if (intent != null)
            {
                IEnumerable<string> entities = GetLUISEntities(recognizerResult);
                if (entities.Count() > 0)
                {
                    switch(intent)
                    {
                        case "use":
                            return $"use {entities.First()} with {entities.Last()}";

                        case "give":
                            return $"give {entities.First()} to {entities.Last()}";
                        
                        default:
                            return $"{intent} {entities.First()}";        
                    }
                }
            }

            return null;
        }

        #region LUIS result parsing

        private static string GetLUISIntent(LUISModel luisResult)
        {
            string intent = null;
            var topIntent = luisResult.TopIntent();
            if (topIntent.intent  != LUISModel.Intent.None)
            {
                intent = topIntent.intent.ToString().Replace("_", " ");
            }
            return intent;
        }

        private static IEnumerable<string> GetLUISEntities(LUISModel luisResult)
        {
            List<string> entities = new List<string>();

            if (luisResult.Entities.GameObject?.Count() > 0)
            {
                entities.AddRange(luisResult.Entities.GameObject.Select(o => o[0]).ToList());
            }
            if (luisResult.Entities.GameActor?.Count() > 0)
            {
                entities.AddRange(luisResult.Entities.GameActor.Select(o => o[0]).ToList());
            }

            return entities;
        }

        #endregion
    }
}
