using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected GameObject[] healthObjects;

    public Action DieAction;

    private void Start()
    {
        SetupHealthObjects();
    }

    public virtual void SetDamage(int damage)
    {
        health -= damage;

        if(health <=0)
        {
            Die();
            return;
        }

        SetupHealthObjects();
    }

    
    protected void Die()
    {
        Destroy(gameObject);
        DieAction?.Invoke();
    }

    protected void SetupHealthObjects()
    {
        var nm = Mathf.Clamp(health - 1, 0, healthObjects.Length);
        for (int i = 0; i < healthObjects.Length; i++)
        {
            healthObjects[i].SetActive(i == nm);
        }

    }
}
