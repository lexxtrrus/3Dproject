using UnityEngine;
using System.Collections;

public class SceletonAI : MonoBehaviour
{
    [SerializeField] private Node rootNode;

    private void Update()
    {
        rootNode.Evaluate();
    }
}
