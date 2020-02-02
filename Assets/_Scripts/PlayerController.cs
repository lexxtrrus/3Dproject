using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactersState
{
    Idle,
    Move,
    Attack,
    Skill,
    Hit,
    Dead,
}

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharactersState charactersState;
    [SerializeField] private int skillBountes;
    [SerializeField] private float skillAngleBount = 45; 
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform firePoint;
    //private static int AttackTrigger;
    


    private void Start()
    {
        charactersState = CharactersState.Idle;
        InputController.OnInputAction += OnInputCommand;
        animator.SetFloat("Speed", 1f);
        StartCoroutine(WaitForRun(3f));
    }

    private IEnumerator WaitForRun(float time)
    {
        charactersState = CharactersState.Attack;
        var timer = Time.time + time;
        while (Time.time < timer)
        {
            transform.Translate(transform.forward * Time.deltaTime);
            yield return null;
        }
        animator.SetFloat("Speed", 0f);
        charactersState = CharactersState.Idle;
    }

    private void Reset()
    {
        animator = GetComponent<Animator>();        
    }

    private void OnDestroy()
    {
        InputController.OnInputAction -= OnInputCommand;
    }

    private void OnInputCommand(InputCommand inputCommand)
    {
        switch (inputCommand)
        {
            case InputCommand.Fire:
                Attack();
                break;
            case InputCommand.Skill:
                Skill();
                break;
        }
    }

    public void AttackEvent()
    {
        var obj = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        var rig = obj.GetComponent<Rigidbody>();
        if (rig)
        {
            rig.AddForce(Vector3.forward * 5f, ForceMode.Impulse);
        }
    }

    public void SkillEvent()
    {
        float step = (skillAngleBount * 2f) * (skillBountes - 1);

        for (int i = 0; i < skillBountes; i++)
        {
            var y = skillAngleBount - i * step;
            var rotation = Quaternion.Euler(15f, y, 0f);
            var obj = Instantiate(ballPrefab, firePoint.transform.position, rotation);
            obj.transform.Translate(obj.transform.forward * 0.3f);
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            if (rig) rig.AddForce(transform.forward * 15f, ForceMode.Impulse);
        }
    }

    private void Attack()
    {
        if (charactersState == CharactersState.Attack || charactersState == CharactersState.Skill) return;
        charactersState = CharactersState.Attack;
        animator.SetTrigger("Attack");
        DelayRun.Execute(delegate {charactersState = CharactersState.Idle;}, 0.5f, gameObject);
    }

    private void Skill()
    {
        if (charactersState == CharactersState.Attack || charactersState == CharactersState.Skill) return;
        charactersState = CharactersState.Skill;
        animator.SetTrigger("Skill");
        DelayRun.Execute(delegate { charactersState = CharactersState.Idle; }, 1f, gameObject);
    }
}