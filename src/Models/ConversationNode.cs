using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class ConversationNode
    {
        public ConversationNode(int id, List<RoomAction> actions, int? parentId, Dictionary<string, ConversationNode> childNodes)
        {
            this.Id = id;
            this.Actions = actions;
            this.ParentId = parentId;
            this.ChildNodes = childNodes;
        }

        public int Id { get; }

        public IEnumerable<RoomAction> Actions { get; }

        public int? ParentId { get; }

        public Dictionary<string, ConversationNode> ChildNodes { get; }

        public ConversationNode Find(int nodeId)
        {
            ConversationNode result = null;

            if (Id == nodeId)
            {
                result = this;    
            }
            else
            {
                foreach (var node in ChildNodes)
                {
                    result = node.Value.Find(nodeId);
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}