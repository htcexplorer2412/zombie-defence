using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    // The current status of the node
    public NodeStatus status;

   // Reset the state of the node
    public virtual void Reset()
    {
        status = NodeStatus.Ready;
    }
    // Execute the node
    public abstract NodeStatus Execute();
}

