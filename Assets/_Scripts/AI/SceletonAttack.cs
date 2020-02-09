using UnityEngine;
using System.Collections;

public class SceletonAttack : Node
{
    [SerializeField] private Animator animator;
    private Coroutine attackProcessCoroutine;

    public bool isAttacking = false;

    public override NodeState Evaluate()
    {
        if(attackProcessCoroutine!= null)
        {
            return NodeState.Running;
        }

        var player = FindObjectOfType<PlayerController>();
        if(player == null)
        {
            return NodeState.Failure;
        }

        if(Vector3.Distance(transform.position, player.transform.position) > 1f)
        {
            return NodeState.Failure;
        }

        attackProcessCoroutine = StartCoroutine(AttackProcess());
        return NodeState.Success;
    }

    private IEnumerator AttackProcess()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        yield return new WaitForSeconds(1f);        
        attackProcessCoroutine = null;
    }
}
