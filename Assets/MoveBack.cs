using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : Node
{
    [SerializeField] private Transform root;
    [SerializeField] private Animator animator;
    [SerializeField] private int speed = 1;

    [SerializeField] private SceletonAttack sct;

    public override NodeState Evaluate()
    {
        if (!sct.isAttacking)
        {
            return NodeState.Failure;
        }
        animator.SetInteger("Movement", speed);
        root.Translate(Time.deltaTime * speed * Vector3.forward, Space.World);
        return NodeState.Success;
    }
}
