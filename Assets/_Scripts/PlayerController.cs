using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Image coldownEffect;
    [SerializeField] private bool isSkillColdown = false;

    float rot = 0;

    private void Start()
    {
        charactersState = CharactersState.Idle;
        InputController.OnInputAction += OnInputCommand;
        //animator.SetFloat("Speed", 1f);
        //StartCoroutine(WaitForRun(3f));
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
                if (isSkillColdown || charactersState == CharactersState.Attack) return;
                isSkillColdown = true;
                coldownEffect.enabled = true;
                StartCoroutine(ColdownCounter());
                Skill();
                break;
        }
    }

    private IEnumerator ColdownCounter()
    {
        float timer = Time.time + 4f;
        coldownEffect.fillAmount = 1f;
        while (Time.time < timer)
        {
            coldownEffect.fillAmount -= 1f / 4f * Time.deltaTime;

            if(coldownEffect.fillAmount <= 0f)
            {
                coldownEffect.fillAmount = 0f;
                break;
            }

            yield return null;
        }

        isSkillColdown = false;
        coldownEffect.enabled = false;
    }

    public void AttackEvent()
    {
        var obj = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        var rig = obj.GetComponent<Rigidbody>();
        if (rig)
        {
            rig.AddForce(transform.forward * 15f, ForceMode.Impulse);
        }

        DelayRun.Execute(delegate { Destroy(obj); }, 1f, obj);
    }

    public void SkillEvent()
    {
        float step = (skillAngleBount * 2f) * (skillBountes - 1);

        for (int i = 0; i < skillBountes; i++)
        {
            var y = skillAngleBount - i * step;
            var rotation = Quaternion.Euler(10f, y, 0f);
            var obj = Instantiate(ballPrefab, firePoint.transform.position, rotation);
            obj.transform.Translate(obj.transform.forward * 0.7f);
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            if (rig) rig.AddForce(transform.forward * 15f, ForceMode.Impulse);
            DelayRun.Execute(delegate { Destroy(obj); }, 1f, obj);
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
        DelayRun.Execute(delegate { charactersState = CharactersState.Idle; Camera.main.GetComponent<Animator>().SetTrigger("Shake"); }, 1f, gameObject);
    }

    private void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //transform.Translate(transform.forward * Time.deltaTime);

        rot += movement.x * 180f * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0f, rot, 0f);
    }
}