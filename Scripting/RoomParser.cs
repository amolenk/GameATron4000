using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Models;
using GameATron4000.Scripting;
using GameATron4000.Scripting.Actions;

namespace GameATron4000.Scripting
{
    public class RoomParser
    {
        private readonly ActionFactory _actionFactory;
        private readonly Regex _preconditionExpression;
        private readonly Regex _commandExpression; 
        private readonly Regex _speakExpression; 
        private readonly Regex _actionExpression;

        public RoomParser(GameInfo gameInfo)
        {
            _actionFactory = new ActionFactory(gameInfo);
            _preconditionExpression = new Regex(@"{(?<preconditions>!?\w+\s?)*}");
            _commandExpression = new Regex("player:(?<text>.*)", RegexOptions.IgnoreCase);
            _actionExpression = new Regex(@"\[(?<name>.*)=(?<args>(\w+\s?)|(\"".*?\""\s?))+\]");
            _speakExpression = new Regex("(?<actor>.*?):(?<text>.*)");
        }

        public List<Command> Parse(string path)
        {
            var result = new List<Command>();
            var lines = File.ReadAllLines(path);

            string commandText = string.Empty;
            List<CommandAction> actions = new List<CommandAction>();
            List<ActionPrecondition> actionPreconditions = null;

            var lineNumber = 0;
            foreach (var line in lines)
            {
                lineNumber += 1;

                // Skip comment lines.
                if (line.StartsWith("#"))
                {
                    continue;
                }

                var match = _preconditionExpression.Match(line);
                if (match.Success)
                {
                    var preconditions = match.Groups["preconditions"].Captures
                        .Select(c => new ActionPrecondition(
                            c.Value.Trim().TrimStart('!'),
                            c.Value.StartsWith('!')))
                        .ToList();

                    if (preconditions.Count == 0)
                    {
                        actionPreconditions = null;
                    }
                    else
                    {
                        if (commandText.Length != 0)
                        {
                            actionPreconditions = preconditions;
                        }
                    }
                    continue;
                }

                match = _commandExpression.Match(line);
                if (match.Success)
                {
                    if (commandText.Length > 0)
                    {
                        throw new IOException($"Error in script on line {lineNumber}: unexpected command.");
                    }

                    commandText = match.Groups["text"].Value.Trim();
                    actions = new List<CommandAction>();
                    actionPreconditions = null;
                    continue;
                }
                else if (commandText.Length == 0)
                {
                    throw new IOException($"Error in script on line {lineNumber}: expected command.");
                }

                match = _actionExpression.Match(line);
                if (match.Success)
                {
                    // TODO Check that command text isn't empty!

                    actions.Add(_actionFactory.CreateAction(
                        match.Groups["name"].Value,
                        match.Groups["args"].Captures.Select(c => c.Value.Trim('"', ' ')).ToList(),
                        actionPreconditions));
                    continue;
                }

                match = _speakExpression.Match(line);
                if (match.Success)
                {
                    actions.Add(_actionFactory.Speak(
                        match.Groups["actor"].Value,
                        match.Groups["text"].Value.Trim(),
                        actionPreconditions));
                    continue;
                }

                if (line.Trim().Length == 0)
                {
                    if (commandText.Length > 0)
                    {
                        result.Add(new Command(commandText, actions));
                        commandText = string.Empty;
                    }
                    continue;
                }

                throw new IOException($"Error in script on line {lineNumber}: unexpected line.");
            }

            if (commandText.Length > 0)
            {
                result.Add(new Command(commandText, actions));
            }

            return result;
        }
        
    }
}