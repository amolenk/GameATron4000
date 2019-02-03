using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Scripting.Actions;
using GameATron4000.Scripting;
using GameATron4000.Models;

namespace GameATron4000.Scripting
{
    public class ConversationParser
    {
        private readonly ActionFactory _actionFactory;
        private readonly Regex _actionExpression;
        private readonly Regex _commandExpression;
        private readonly Regex _speakExpression; 

        public ConversationParser(GameInfo gameInfo)
        {
            _actionFactory = new ActionFactory(gameInfo);
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

                // Skip comment lines.
                if (line.StartsWith("#"))
                {
                    continue;
                }

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
                        actions.Add(_actionFactory.Speak(
                            match.Groups["actor"].Value,
                            match.Groups["text"].Value.Trim()));

                        context.LineIndentSize = ReadIndentation(reader);
                        continue;
                    }

                    match = _actionExpression.Match(line);
                    if (match.Success)
                    {
                        actions.Add(_actionFactory.CreateAction(
                            match.Groups["name"].Value,
                            match.Groups["args"].Captures.Select(c => c.Value.Trim('"')).ToList()));

                        context.LineIndentSize = ReadIndentation(reader);
                        continue;
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