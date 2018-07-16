using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class DialogTreeNode
    {
        public DialogTreeNode(int id, List<Action> actions, int? parentId, Dictionary<string, DialogTreeNode> childNodes)
        {
            this.Id = id;
            this.Actions = actions;
            this.ParentId = parentId;
            this.ChildNodes = childNodes;
        }

        public int Id { get; }

        public IEnumerable<Action> Actions { get; }

        public int? ParentId { get; }

        public Dictionary<string, DialogTreeNode> ChildNodes { get; }

        public DialogTreeNode Find(int nodeId)
        {
            DialogTreeNode result = null;

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