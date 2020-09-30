using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Place on a Domino object
public class DragScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas = null;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject cells;


    public int leftValue;
    public int rightValue;

    private GameManager gameManager;

    private Vector2 posBeforeDrag;
    private Vector2 posOnDrop;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        canvas = gameManager.canvas;
        cells = gameManager.cells;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<CanvasGroup>().interactable)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        eventData.pointerDrag.GetComponent<RectTransform>().ForceUpdateRectTransforms();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name + " down");
        posBeforeDrag = rectTransform.anchoredPosition;

    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + " dropped on " + gameObject.name);

        if (EvaluateDrop(eventData))
        {
            eventData.pointerDrag.transform.SetParent(cells.transform, false);
            eventData.pointerDrag.GetComponent<CanvasGroup>().interactable = false;
            gameManager.currentHandSizePC--;
            gameManager.turnFinished = true;
        }
        else
        {
            //set back to picked up position
            //todo

        }
    }

    private bool EvaluateDrop(PointerEventData eventData)
    {
        DragScript dragObj = eventData.pointerDrag.GetComponent<DragScript>();
        int dragObjLV = dragObj.leftValue;
        int dragObjRV = dragObj.rightValue;

        DragScript groundObj = this;

        if (dragObjLV == leftValue || dragObjRV == rightValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
