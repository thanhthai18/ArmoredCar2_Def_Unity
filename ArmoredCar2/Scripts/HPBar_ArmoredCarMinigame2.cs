using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_ArmoredCarMinigame2 : MonoBehaviour
{
    public Image fillHP;
    public float currentHP;
    public float maxHP;
    public bool isDecreasingHP;
    public float tmpHP;

    private void Awake()
    {
        isDecreasingHP = false;
        currentHP = 10;
        maxHP = 10;
        tmpHP = 10;
    }

    private void Update()
    {
        if (isDecreasingHP)
        {
            if (currentHP > tmpHP)
            {
                currentHP -= Time.deltaTime * 10;
            }
            if (currentHP == tmpHP)
            {
                isDecreasingHP = false;
            }
            fillHP.fillAmount = currentHP / maxHP;
        }
    }
}
