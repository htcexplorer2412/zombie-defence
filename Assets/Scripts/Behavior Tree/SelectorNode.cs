using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node 
{
    private List<Node> childern = new List<Node>();

    public void AddChild(Node child)
    {
        childern.Add(child);
    }

    public override void Reset()
    {
        foreach(Node child in childern)
        {
            child.Reset();
        }

        base.Reset();
    }

    public override NodeStatus Execute()
    {
        foreach (Node Child in childern)
        {
            switch(Child.Execute())
            {
                case NodeStatus.Success:
                status = NodeStatus.Success;
                return status;

                case NodeStatus.Running:
                status = NodeStatus.Running;
                return status;

                case NodeStatus.Failure:
                continue;

                default:
                continue;
                
            }

        }

        status = NodeStatus.Failure;
        return status;
        
    }
}