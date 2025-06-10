using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIHUDInfo : MonoBehaviour
{
    [SerializeField] private Text _evnLabel;
    private ServerEnv _env;
    public void Construct(ServerEnv evn)
    {
        _evnLabel.text = evn.ToString();
        ApplyColor();
    }
    
    private void ApplyColor()
    {
        //todo 
    }
}
