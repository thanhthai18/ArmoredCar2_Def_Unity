using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredCar_ArmoredCarMinigame2 : MonoBehaviour
{
    public Vector2 prePos, currentPos;
    public bool isHoldMouse;

    private void Start()
    {
        currentPos = transform.position;
        prePos = currentPos;
        StartCoroutine(UpdatePos());
    }

    IEnumerator UpdatePos()
    {
        while (true)
        {
            if (isHoldMouse)
            {
                currentPos = transform.position;
                yield return new WaitForSeconds(0.01f);
                prePos = currentPos;
                yield return new WaitForSeconds(0.01f);
                currentPos = transform.position;
            }
            else
                yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        if (!GameController_ArmoredCarMinigame2.instance.isLose)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHoldMouse = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isHoldMouse = false;
            }

            if (isHoldMouse)
            {
                if (currentPos.x > prePos.x)
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                if (currentPos.x < prePos.x)
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
    }
}
