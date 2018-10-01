using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Models;
using GameATron4000.Models.Actions;

namespace GameATron4000.Games
{
    public class RoomParser
    {
        private readonly Regex _preconditionExpression;
        private readonly Regex _commandExpression; 
        private readonly Regex _speakExpression; 
        private readonly Regex _actionExpression;

        public RoomParser()
        {
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
            List<RoomAction> actions = new List<RoomAction>();
            List<Precondition> commandPreconditions = null;
            List<Precondition> actionPreconditions = null;

            var lineNumber = 0;
            foreach (var line in lines)
            {
                lineNumber += 1;

                var match = _preconditionExpression.Match(line);
                if (match.Success)
                {
                    var preconditions = match.Groups["preconditions"].Captures
                        .Select(c => new Precondition(
                            c.Value.TrimStart('!'),
                            !c.Value.StartsWith('!')))
                        .ToList();

                    if (preconditions.Count == 0)
                    {
                        actionPreconditions = null;
                    }
                    else
                    {
                        if (commandText.Length == 0)
                        {
                            commandPreconditions = preconditions;
                        }
                        else
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
                    actions = new List<RoomAction>();
                    continue;
                }
                else if (commandText.Length == 0)
                {
                    throw new IOException($"Error in script on line {lineNumber}: expected command.");
                }

                match = _actionExpression.Match(line);
                if (match.Success)
                {
                    // TODO Shouldn't preconditions be added here?
                    actions.Add(new ActionBuilder()
                        .WithName(match.Groups["name"].Value)
                        .WithArguments(match.Groups["args"].Captures.Select(c => c.Value.Trim('"', ' ')))
                        .Build());
                    continue;
                }

                match = _speakExpression.Match(line);
                if (match.Success)
                {
                    actions.Add(new SpeakAction(
                        match.Groups["text"].Value.Trim(),
                        match.Groups["actor"].Value,
                        actionPreconditions));
                    continue;
                }

                if (line.Trim().Length == 0)
                {
                    if (commandText.Length > 0)
                    {
                        result.Add(new Command(commandText, actions, commandPreconditions));

                        commandText = string.Empty;
                        actions = new List<RoomAction>();
                        commandPreconditions = null;
                    }
                    continue;
                }

                throw new IOException($"Error in script on line {lineNumber}: unexpected line.");
            }

            if (commandText.Length > 0)
            {
                result.Add(new Command(commandText, actions, commandPreconditions));
            }

            return result;
        }
        
    }
}