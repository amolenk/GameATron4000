using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Models;

namespace GameATron4000.Dialogs
{
    public class ConversationScriptParser
    {
        private readonly Regex _actionExpression;
        private readonly Regex _commandExpression;
        private readonly Regex _speakExpression; 

        public ConversationScriptParser()
        {
            _actionExpression = new Regex(@"\[(?<name>.*?)(=(?<args>(\w+\s?)|(\"".*?\""\s?))+)?\]");
            _commandExpression = new Regex("- (?<command>.*)");
            _speakExpression = new Regex("(?<actor>.*?):(?<text>.*)");
        }

        public ConversationStep Parse(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return ParseStep(reader, new ParsingContext());
            }
        }

        private ConversationStep ParseStep(TextReader reader, ParsingContext context, int indentLevel = 0)
        {
            var actions = new List<Action>();
            var subSteps = new Dictionary<string, ConversationStep>();

            context.LineIndentSize = ReadIndentation(reader);
            string line;
            while (context.LineIndentSize == indentLevel
                && (line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                context.LineNumber += 1;

                var match = _commandExpression.Match(line);
                if (match.Success)
                {
                    subSteps.Add(
                        match.Groups["command"].Value,
                        ParseStep(reader, context, indentLevel + 1));
                    continue;
                }

                // Only allow non-command lines if we haven't encountered any so far.
                if (!subSteps.Any())
                {
                    match = _speakExpression.Match(line);
                    if (match.Success)
                    {
                        actions.Add(new ActionBuilder()
                            .WithName(Action.Speak)
                            .WithArgument(match.Groups["text"].Value.Trim())
                            .WithArgument(match.Groups["actor"].Value)
                            .Build());

                        context.LineIndentSize = ReadIndentation(reader);
                        continue;
                    }

                    match = _actionExpression.Match(line);
                    if (match.Success)
                    {
                        var actionBuilder = new ActionBuilder()
                            .WithName(match.Groups["name"].Value)
                            .WithArguments(match.Groups["args"].Captures.Select(c => c.Value.Trim('"')));

                        if (actionBuilder.IsValid())
                        {
                            actions.Add(actionBuilder.Build());

                            context.LineIndentSize = ReadIndentation(reader);
                            continue;
                        }
                        else
                        {
                            throw new IOException($"Parse error at line {context.LineNumber}: Unsupported action.");
                        }
                    }
                }

                throw new IOException($"Parse error at line {context.LineNumber}: Unexpected line.");
            }

            return new ConversationStep(actions, subSteps);
        }

        private int ReadIndentation(TextReader reader)
        {
            var result = 0;
            while (reader.Peek() == (int)' ')
            {
                reader.Read();
                result += 1;
            }
            return result / 4;
        }

        private class ParsingContext
        {
            public int LineIndentSize { get; set; }

            public int LineNumber { get; set; }
        }
    }
}