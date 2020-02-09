using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusContainer : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject bonus;

    private void Reset()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        if (health)
        {
            health.DieAction += OnDie;
            return;
        }

        Destroy(this);
    }

    private void OnDie()
    {
        if (bonus)
        {
            Instantiate(bonus, transform.position + (Vector3.up * 0.5f), transform.rotation);
        }
    }

}
