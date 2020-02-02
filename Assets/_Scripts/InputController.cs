using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InputCommand
{
    Fire,
    Skill,
}

public class InputController : MonoBehaviour
{
    public static Action<InputCommand> OnInputAction;

    [SerializeField] private Button fireButton;
    [SerializeField] private Button skillButton;

    private void Awake()
    {
        fireButton.onClick.AddListener(OnFireButton);
        skillButton.onClick.AddListener(OnSkillButton);
    }

    private void OnFireButton()
    {
        OnInputAction?.Invoke(InputCommand.Fire);
    }

    private void OnSkillButton()
    {
        OnInputAction?.Invoke(InputCommand.Skill);
    }
}
