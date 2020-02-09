using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;
    [SerializeField] private float speed;
    [SerializeField] private int damage = 1;

    private float baseSpeed;

    private void Reset()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        baseSpeed = speed;
    }

    private void FixedUpdate()
    {
        var normalizedVelocity = rig.velocity.normalized;
        rig.velocity = Vector3.Lerp(rig.velocity, normalizedVelocity * speed, Time.deltaTime * 5f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health)
        {
            health.SetDamage(damage);
        }
    }


}
