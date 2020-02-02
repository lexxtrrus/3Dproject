using UnityEngine;
using System.Collections;
using System;

public class DelayRun : MonoBehaviour
{
    public static void Execute(Action callback, float timer, GameObject targer)
    {
        var component = targer.AddComponent<DelayRun>();
        component.StartCoroutine(component.WaitAndExecute(callback, timer));
    }

    private IEnumerator WaitAndExecute(Action callback, float timer)
    {
        yield return new WaitForSeconds(timer);
        callback?.Invoke();
        Destroy(this);
    }
}
