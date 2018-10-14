using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Models.Actions;
using GameATron4000.Models;

namespace GameATron4000.Games
{
    public class ConversationParser
    {
        private readonly Regex _actionExpression;
        private readonly Regex _commandExpression;
        private readonly Regex _speakExpression; 

        public ConversationParser()
        {
            _actionExpression = new Regex(@"\[(?<name>.*?)(=(?<args>(\w+\s?)|(\"".*?\""\s?))+)?\]");
            _commandExpression = new Regex("- (?<command>.*)");
            _speakExpression = new Regex("(?<actor>.*?):(?<text>.*)");
        }

        public ConversationNode Parse(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return ParseStep(reader, new ParsingContext());
            }
        }

        private ConversationNode ParseStep(TextReader reader, ParsingContext context, int? parentId = null, int indentLevel = 0)
        {
            var nodeId = context.NodeId++;
            var actions = new List<CommandAction>();
            var subSteps = new Dictionary<string, ConversationNode>();

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
                        ParseStep(reader, context, nodeId, indentLevel + 1));
                    continue;
                }

                // Only allow non-command lines if we haven't encountered any so far.
                if (!subSteps.Any())
                {
                    match = _speakExpression.Match(line);
                    if (match.Success)
                    {
                        actions.Add(new SpeakAction(match.Groups["text"].Value.Trim(), match.Groups["actor"].Value));

                        context.LineIndentSize = ReadIndentation(reader);
                        continue;
                    }

                    match = _actionExpression.Match(line);
                    if (match.Success)
                    {
                        var actionBuilder = new ActionBuilder()
                            .WithName(match.Groups["name"].Value)
                            .WithArguments(match.Groups["args"].Captures.Select(c => c.Value.Trim('"')));

                        // TODO
                        // if (actionBuilder.IsValid())
                        // {
                            actions.Add(actionBuilder.Build());

                            context.LineIndentSize = ReadIndentation(reader);
                            continue;
                        // }
                        // else
                        // {
                            // TODO THIS:::
                        //     throw new IOException($"Parse error at line {context.LineNumber}: Unsupported action.");
                        // }
                    }
                }

                throw new IOException($"Parse error at line {context.LineNumber}: Unexpected line.");
            }

            return new ConversationNode(nodeId++, actions, parentId, subSteps);
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

            public int NodeId { get; set; } = 1;
        }
    }
}