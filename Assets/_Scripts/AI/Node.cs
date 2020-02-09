using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Success,
    Failure,
    Running,
}

public abstract class Node : MonoBehaviour
{
    protected NodeState NodeState;
    public abstract NodeState Evaluate();
}
   
