using UnityEngine;
using System.Collections;

public class SceletonMove : Node
{
    [SerializeField] private Transform root;
    [SerializeField] private Animator animator;
    [SerializeField] private int speed = 1;

    public override NodeState Evaluate()
    {
        animator.SetInteger("Movement", speed);
        root.Translate(Time.deltaTime * speed * Vector3.back, Space.World);
        return NodeState.Success;
    }
}
