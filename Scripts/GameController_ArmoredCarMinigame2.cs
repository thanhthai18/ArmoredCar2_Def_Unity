using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;
using UnityEngine.UI;

public class GameController_ArmoredCarMinigame2 : MonoBehaviour
{
    public static GameController_ArmoredCarMinigame2 instance;
    public Camera mainCamera;
    public ArmoredCar_ArmoredCarMinigame2 armoredCar;
    public List<PathCreator> listPath = new List<PathCreator>();
    public List<Transform> listPosEasy = new List<Transform>();
    public RaycastHit2D[] hit;
    public Vector2 mouseCurrentPos;
    public float f2;
    public bool isHoldCar;
    public Vector2 tmpPos_ArmoredCar;
    public GameObject castle, flag;
    public Rocket_ArmoredCarMinigame2 rocketPrefab;
    public Text txtTime;
    public int time = 60;
    public int stage;
    public List<int> listCheckSameEasy = new List<int>();
    public List<int> listCheckSameHard = new List<int>();
    public Coroutine spawnCoroutine;
    public bool isLose, isWin, isBegin, isTutorial;
    public HPBar_ArmoredCarMinigame2 Bar;
    public Canvas canvas;
    public GameObject tutorial;
    public Rocket_ArmoredCarMinigame2 rockTutorial;
    public Transform[] arrayWaypointTutorial;
    public Vector3[] arrayVector3Tutorial;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isLose = false;
        isWin = false;
        isTutorial = true;
        isBegin = false;
        arrayVector3Tutorial = new Vector3[6];
    }

    private void Start()
    {
        SetSizeCamera();
        mainCamera.orthographicSize *= 0.7f;
        tutorial.SetActive(false);
        SetUpPathTutorial();
        Intro();
        isHoldCar = false;
        txtTime.text = time.ToString();
        ResetListCheckSameEasy();
        ResetListCheckSameHard();

    }

    void SetSizeCamera()
    {
        float f1;
        f1 = 16f / 9;
        f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void SetUpPathTutorial()
    {
        for (int i = 0; i < arrayWaypointTutorial.Length; i++)
        {
            arrayVector3Tutorial[i] = arrayWaypointTutorial[i].position;
        }
    }

    void Intro()
    {
        canvas.gameObject.SetActive(false);
        armoredCar.transform.DOMoveX(castle.transform.position.x - 3.45f, 3).OnComplete(() =>
        {
            mainCamera.DOOrthoSize(mainCamera.orthographicSize * 1f / 0.7f, 2).OnComplete(() =>
            {
                canvas.gameObject.SetActive(true);
                rockTutorial = Instantiate(rocketPrefab, listPosEasy[9].transform.position, Quaternion.Euler(0, 180, 0));
                Invoke(nameof(ShowTutorial), 1);
            });
        });
    }

    void SpawnRocketEasy()
    {
        int ran = Random.Range(0, listCheckSameEasy.Count);
        Rocket_ArmoredCarMinigame2 tmpRocket = Instantiate(rocketPrefab, listPosEasy[listCheckSameEasy[ran]].transform.position, Quaternion.identity);
        listCheckSameEasy.RemoveAt(ran);
        if (tmpRocket.transform.position.x > castle.transform.position.x)
        {
            if (tmpRocket.transform.position.y < castle.transform.position.y)
            {
                tmpRocket.transform.eulerAngles = new Vector3(0, 0, -15 + 180f / (2 * Mathf.PI) * Mathf.Atan((-tmpRocket.transform.position.y + castle.transform.position.y) / (-tmpRocket.transform.position.x + castle.transform.position.x)));
            }
            else
                tmpRocket.transform.eulerAngles = new Vector3(0, 0, 10 + 180f / (2 * Mathf.PI) * Mathf.Atan((-tmpRocket.transform.position.y + castle.transform.position.y) / (-tmpRocket.transform.position.x + castle.transform.position.x)));
            tmpRocket.GetComponent<SpriteRenderer>().flipX = true;

        }
        else
        {

            if (tmpRocket.transform.position.y < castle.transform.position.y)
            {
                tmpRocket.transform.eulerAngles = new Vector3(0, 0, +15 + 180f / (2 * Mathf.PI) * Mathf.Atan((-tmpRocket.transform.position.y + castle.transform.position.y) / (-tmpRocket.transform.position.x + castle.transform.position.x)));
            }
            else
                tmpRocket.transform.eulerAngles = new Vector3(0, 0, -10 + 180f / (2 * Mathf.PI) * Mathf.Atan((-tmpRocket.transform.position.y + castle.transform.position.y) / (-tmpRocket.transform.position.x + castle.transform.position.x)));

        }

    }

    void SpawnRocketHard()
    {
        int ran = Random.Range(0, listCheckSameHard.Count);
        Rocket_ArmoredCarMinigame2 tmpRocket = Instantiate(rocketPrefab, listPosEasy[0].transform.position, Quaternion.identity);
        tmpRocket.pathCreator = listPath[listCheckSameHard[ran]];
        if (listCheckSameHard[ran] == 3 || listCheckSameHard[ran] == 4 || listCheckSameHard[ran] == 5)
        {
            tmpRocket.GetComponent<SpriteRenderer>().flipX = true;
        }
        listCheckSameHard.RemoveAt(ran);



    }

    public IEnumerator SetUpMap(int stageIndex)
    {
        while (stageIndex == 1)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                SpawnRocketEasy();
            }
            ResetListCheckSameEasy();
            yield return new WaitForSeconds(3);

        }
        while (stageIndex == 2)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                SpawnRocketHard();
            }
            ResetListCheckSameHard();
            yield return new WaitForSeconds(3);
        }
        while (stageIndex == 3)
        {
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                SpawnRocketHard();
            }
            ResetListCheckSameHard();
            yield return new WaitForSeconds(3);
        }
        while (stageIndex == 4)
        {
            for (int i = 0; i < 8; i++)
            {
                SpawnRocketHard();
            }
            ResetListCheckSameHard();
            yield return new WaitForSeconds(3);
        }

    }

    IEnumerator CountingTime()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            txtTime.text = time.ToString();
            if (time == 45)
            {
                stage = 2;
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = StartCoroutine(SetUpMap(stage));
            }
            if (time == 20)
            {
                stage = 3;
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = StartCoroutine(SetUpMap(stage));
            }
            if (time == 4)
            {
                stage = 4;
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = StartCoroutine(SetUpMap(stage));
            }
            if (time == 0 && !isLose)
            {
                isWin = true;
                Debug.Log("Win");
                StopAllCoroutines();
            }
        }
    }

    void ResetListCheckSameEasy()
    {
        listCheckSameEasy.Clear();
        for (int j = 0; j < listPosEasy.Count; j++)
        {
            listCheckSameEasy.Add(j);
        }
    }
    void ResetListCheckSameHard()
    {
        listCheckSameHard.Clear();
        for (int j = 0; j < listPath.Count; j++)
        {
            listCheckSameHard.Add(j);
        }
    }

    void ShowTutorial()
    {
        isBegin = true;
        if (rockTutorial != null)
        {
            tutorial.transform.position = arrayVector3Tutorial[0];
            tutorial.SetActive(true);
            float distance = 0;
            for (int i = 1; i < arrayVector3Tutorial.Length; i++)
            {
                distance += (arrayVector3Tutorial[i - 1] - arrayVector3Tutorial[i]).magnitude;
            }
            tutorial.transform.DOPath(arrayVector3Tutorial, distance / 7, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1);
            rockTutorial.transform.DOPause();
        }
    }

    public void BeginGame()
    {
        spawnCoroutine = StartCoroutine(SetUpMap(stage));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBegin)
        {
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.RaycastAll(mouseCurrentPos, Vector2.zero);
            if (hit.Length != 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.gameObject.CompareTag("Player"))
                    {
                        isHoldCar = true;
                        tmpPos_ArmoredCar = mouseCurrentPos - (Vector2)armoredCar.transform.position;
                        if (isTutorial)
                        {
                            isTutorial = false;
                        }
                        if (tutorial.activeSelf)
                        {
                            Time.timeScale = 1;
                            tutorial.SetActive(false);
                            tutorial.transform.DOKill();
                            StartCoroutine(CountingTime());
                            stage = 1;
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHoldCar = false;
        }

        if (isHoldCar)
        {
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (armoredCar.transform.position.y < 1.77f && armoredCar.transform.position.y > -1.89f)
            {
                if (armoredCar.transform.position.x < castle.transform.position.x)
                {
                    mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -mainCamera.orthographicSize * f2 + 1.3f + tmpPos_ArmoredCar.x, castle.transform.position.x - 1.37f - 1.3f + tmpPos_ArmoredCar.x), Mathf.Clamp(mouseCurrentPos.y, -mainCamera.orthographicSize + 0.5f + tmpPos_ArmoredCar.y, mainCamera.orthographicSize - 1.1f + tmpPos_ArmoredCar.y));
                }
                if (armoredCar.transform.position.x > castle.transform.position.x)
                {
                    mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -castle.transform.position.x + 1.37f + 1.3f + tmpPos_ArmoredCar.x, mainCamera.orthographicSize * f2 - 1.3f + tmpPos_ArmoredCar.x), Mathf.Clamp(mouseCurrentPos.y, -mainCamera.orthographicSize + 0.5f + tmpPos_ArmoredCar.y, mainCamera.orthographicSize - 1.1f + tmpPos_ArmoredCar.y));
                }
            }
            else if (armoredCar.transform.position.x < 2.42f && armoredCar.transform.position.x > -2.48f)
            {
                if (armoredCar.transform.position.y < castle.transform.position.y)
                {
                    mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -mainCamera.orthographicSize * f2 + 1.3f + tmpPos_ArmoredCar.x, mainCamera.orthographicSize * f2 - 1.3f + tmpPos_ArmoredCar.x), Mathf.Clamp(mouseCurrentPos.y, -mainCamera.orthographicSize + 0.5f + tmpPos_ArmoredCar.y, castle.transform.position.y - 1.3f - 1.1f + tmpPos_ArmoredCar.y));
                }
                if (armoredCar.transform.position.y > castle.transform.position.y)
                {
                    mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -mainCamera.orthographicSize * f2 + 1.3f + tmpPos_ArmoredCar.x, mainCamera.orthographicSize * f2 - 1.3f + tmpPos_ArmoredCar.x), Mathf.Clamp(mouseCurrentPos.y, -castle.transform.position.y + 1.37f + 0.5f + tmpPos_ArmoredCar.y, mainCamera.orthographicSize - 1.1f + tmpPos_ArmoredCar.y));
                }

            }
            else
            {
                mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -mainCamera.orthographicSize * f2 + 1.3f + tmpPos_ArmoredCar.x, mainCamera.orthographicSize * f2 - 1.3f + tmpPos_ArmoredCar.x), Mathf.Clamp(mouseCurrentPos.y, -mainCamera.orthographicSize + 0.5f + tmpPos_ArmoredCar.y, mainCamera.orthographicSize - 1.1f + tmpPos_ArmoredCar.y));
                armoredCar.transform.position = new Vector2(mouseCurrentPos.x - tmpPos_ArmoredCar.x, mouseCurrentPos.y - tmpPos_ArmoredCar.y);
            }

            //armoredCar.transform.position = new Vector2(mouseCurrentPos.x - tmpPos_ArmoredCar.x, mouseCurrentPos.y - tmpPos_ArmoredCar.y);
            armoredCar.transform.DOMove(new Vector2(mouseCurrentPos.x - tmpPos_ArmoredCar.x, mouseCurrentPos.y - tmpPos_ArmoredCar.y), 0.05f);
        }
    }
}
