using DG.Tweening;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_ArmoredCarMinigame2 : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;
    public GameObject VFXPrefab;
    public bool isTut;


    private void Start()
    {
        isTut = true;
        if (pathCreator == null)
        {
            transform.DOMove(GameController_ArmoredCarMinigame2.instance.castle.transform.position, 5);
        }
    }

    private void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.eulerAngles = new Vector3(pathCreator.path.GetRotationAtDistance(distanceTravelled).x, 0, pathCreator.path.GetRotationAtDistance(distanceTravelled).z * 150);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameController_ArmoredCarMinigame2.instance.rockTutorial != null && isTut)
            {
                isTut = false;
                GameController_ArmoredCarMinigame2.instance.BeginGame();
                if (GameController_ArmoredCarMinigame2.instance.tutorial.activeSelf)
                {
                    GameController_ArmoredCarMinigame2.instance.tutorial.SetActive(false);
                    GameController_ArmoredCarMinigame2.instance.tutorial.transform.DOKill();
                }
            }
            GameObject tmpVFX = Instantiate(VFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            tmpVFX.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
            {
                Destroy(tmpVFX);
            });


        }

        if (collision.gameObject.CompareTag("BodyCar"))
        {
            GameController_ArmoredCarMinigame2.instance.Bar.isDecreasingHP = true;
            if (GameController_ArmoredCarMinigame2.instance.Bar.tmpHP == 1)
            {
                GameController_ArmoredCarMinigame2.instance.Bar.tmpHP = 0;
                if (!GameController_ArmoredCarMinigame2.instance.isWin)
                {
                    Debug.Log("Thua");
                    GameController_ArmoredCarMinigame2.instance.StopAllCoroutines();
                    GameController_ArmoredCarMinigame2.instance.armoredCar.StopAllCoroutines();
                }
            }
            if (GameController_ArmoredCarMinigame2.instance.Bar.currentHP > 1)
            {
                GameController_ArmoredCarMinigame2.instance.Bar.tmpHP--;
            }
            GameObject tmpVFX = Instantiate(VFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            tmpVFX.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() =>
            {
                Destroy(tmpVFX);
            });
        }
    }
}
