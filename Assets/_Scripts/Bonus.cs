using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Bonus : MonoBehaviour
{
    protected void Rest()
    {
        var rig = GetComponent<Rigidbody>();
        rig.isKinematic = true;

        var collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
    }

    protected void Update()
    {
        transform.Translate(Time.deltaTime * 2f * Vector3.back, Space.World);
    }

    protected virtual void PickUpBonus()
    {
        StartCoroutine(MoveUp());
        var col = GetComponent<SphereCollider>();
        if (col)
        {
            col.enabled = false;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player)
        {
            PickUpBonus();
        }
    }

    protected void OnMouseDown()
    {
        PickUpBonus();
    }

    protected IEnumerator MoveUp()
    {
        var height = 0f;
        while(height < 10f)
        {
            height += Time.deltaTime * 10f;
            var pos = transform.position;
            pos.y = height;
            transform.position = pos;
            yield return null;
        }

        Destroy(gameObject);
    }
}
