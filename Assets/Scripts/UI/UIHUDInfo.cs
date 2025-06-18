using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VContainer;

public class UIHUDInfo : MonoBehaviour
{
    [SerializeField] private Text _evnLabel;
    private ServerEnv _env;
    
    [Inject]
    public void Construct(ServerEnv evn)
    {
        _env = evn;

    }

    private void Awake()
    {
        _evnLabel.text = _env.ToString();
        ApplyColor();
    }

    private void ApplyColor()
    {
        //todo 
    }
}
