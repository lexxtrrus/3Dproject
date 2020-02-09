using UnityEngine;
using System.Collections;

public class WarCry : Node
{
    [SerializeField] private Animator animator;

    private State state;
    private Coroutine warCryCoroutine;

    private enum State
    {
        Wait,
        WarCry,
        Complete,
    }

    public override NodeState Evaluate()
    {
        switch (state)
        {
            case State.Wait:
                state = State.WarCry;
                warCryCoroutine = StartCoroutine(WarCryProcess());
                return NodeState.Running;
            case State.WarCry:
                if (warCryCoroutine != null)
                {
                    return NodeState.Running;
                }
                state = State.Complete;
                return NodeState.Success;
            case State.Complete:
                
                return NodeState.Failure;
        }

        return NodeState;
    }

    private IEnumerator WarCryProcess()
    {
        animator.SetTrigger("Cry");
        yield return new WaitForSeconds(1.5f);
        warCryCoroutine = null;
    }
}