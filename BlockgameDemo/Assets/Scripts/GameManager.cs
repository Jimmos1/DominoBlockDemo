using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject cells;
    public GameObject skipButton;

    public GameObject winText;
    public GameObject loseText;
    
    public List<GameObject> dominoPool;

    public List<GameObject> COMHandList;
    public List<GameObject> COMHandActual;
    public List<GameObject> PCHandList;
    public List<GameObject> PCHandActual;

    public GameObject comHandTransform;
    public GameObject playerHandTransform;


    public int startHandSize = 7;
    private int currentHandSizeCOM = 0;
    [HideInInspector]
    public int currentHandSizePC = 0;

    public bool COMTurn = false;
    public bool PCTurn = false;
    public bool firstMove = true;

    public bool skipTurn = false;
    public bool turnFinished = false;

    private void Awake()
    {
        //COMHandActual = new GameObject[7]; 
        //PCHandActual = new GameObject[7]; 

        DecideTurn();
        FillHand();
    }

    private void Update()
    {
        if(currentHandSizeCOM == 0)
        {
            //Lose
            skipButton.SetActive(false);
            loseText.SetActive(true);
            //Debug.Log("You Lost");
        }
        if(currentHandSizePC == 0)
        {
            skipButton.SetActive(false);
            winText.SetActive(true);
            //Debug.Log("You Won");
            //Win
        }

        if (COMTurn)
        {
            MoveCOM();
        }

        if (PCTurn)
        {
            MovePC();
        }

        if (skipTurn)
        {
            skipTurn = false;
            COMTurn = !COMTurn;
            PCTurn = !PCTurn;
        }
    }

    private void MoveCOM()
    {
        turnFinished = false;
        if (firstMove)
        {
            firstMove = false;

            int randNum = Random.Range(0, COMHandActual.Count);
            GameObject dominoTemp = Instantiate(COMHandActual[randNum], cells.transform);
            dominoTemp.transform.SetParent(cells.transform);
            Destroy(COMHandActual[randNum]);
            COMHandActual.RemoveAt(randNum);
            COMHandActual.TrimExcess();
            currentHandSizeCOM--;

            COMTurn = false;
            PCTurn = true;

            turnFinished = true;

        }

        if(!turnFinished)
        {
            turnFinished = true;


            int randNum = Random.Range(0, COMHandActual.Count);
            GameObject dominoTemp = Instantiate(COMHandActual[randNum], cells.transform);
            dominoTemp.transform.SetParent(cells.transform);
            Destroy(COMHandActual[randNum]);
            COMHandActual.RemoveAt(randNum);
            COMHandActual.TrimExcess();
            currentHandSizeCOM--;

            COMTurn = false;
            PCTurn = true;

            //logic not implemented should not be random, instead use domino values




        }


    }

    private void MovePC()
    {
        turnFinished = false;

        if (firstMove)
        {
            firstMove = false;

            //special case

            if (turnFinished || skipTurn)
            {
                turnFinished = false;
                COMTurn = true;
                PCTurn = false;
            }
        }
        else if(turnFinished)
        {
            turnFinished = false;
            COMTurn = true;
            PCTurn = false;
        }

    }

    private void DecideTurn()
    {
        int randNum = Random.Range(0, 2);

        if (randNum == 1)
        {
            COMTurn = true;
            PCTurn = false;
        }
        else
        {
            COMTurn = false;
            PCTurn = true;
        }
    }

    private void FillHand()
    {
        int tempRand;

        for (int i = 0; i < startHandSize; i++)
        {
            tempRand = Random.Range(0, dominoPool.Count);

            COMHandList.Add(dominoPool[tempRand]);
            dominoPool.RemoveAt(tempRand);
            currentHandSizeCOM++;

            tempRand = Random.Range(0, dominoPool.Count);

            PCHandList.Add(dominoPool[tempRand]);
            dominoPool.RemoveAt(tempRand);
            currentHandSizePC++;
        }

        foreach (GameObject domino in COMHandList)
        {
            GameObject dominoTemp = Instantiate(domino, comHandTransform.transform);
            COMHandActual.Add(dominoTemp);
            dominoTemp.GetComponent<CanvasGroup>().interactable = false;
            dominoTemp.name = domino.name;
        }

        foreach (GameObject domino in PCHandList)
        {
            GameObject dominoTemp = Instantiate(domino, playerHandTransform.transform);
            PCHandActual.Add(dominoTemp);
            dominoTemp.name = domino.name;
        }

    }

    public void ResetScene(int sceneIntex)
    {
        SceneManager.LoadScene(sceneIntex);
    }

    public void SkipTurn()
    {
        skipTurn = true;
    }
}
