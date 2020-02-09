using UnityEngine;
using System.Collections;

public class CheckOutOfFiled : Node
{
    public override NodeState Evaluate()
    {
        return transform.position.z > -30f ? NodeState.Running : NodeState.Failure;
    }
}
