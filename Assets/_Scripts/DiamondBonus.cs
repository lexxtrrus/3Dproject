using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBonus : Bonus
{
    protected override void PickUpBonus()
    {
        base.PickUpBonus();
        StartCoroutine(StartRotate());
    }

    private IEnumerator StartRotate()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * 5f * Time.deltaTime);
            yield return null;
        }
    }
}
