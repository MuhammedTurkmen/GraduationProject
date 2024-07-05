using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaycasterUI : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public RectTransform canvasRectTransform;

    private bool clicked, selected;

    private Image clickedImage;
    private Vector2 mousePosition;

    private ProcessTypeIconUI selectedProcessItem;
    private InputItemUI selectedConnector;

    public GameObject slicerIconPrefab, mergerIconPrefab, alterationIconPrefab;

    public Transform machineUI;

    List<RaycastResult> newResults;

    public GameObject linePrefab, line;

    void Update()
    {
        #region Left Click

        if (Input.GetMouseButtonDown(0))
        {
            newResults = CheckUIRaycast();

            foreach (RaycastResult result in newResults)
            {
                //print($"left clicked obj: {result.gameObject.name}");
                if (result.gameObject.CompareTag("ProcessIconButton"))
                {
                    clicked = true;
                    GameObject icon = null;
                    Cursor.visible = false;

                    if (result.gameObject.GetComponent<ProcessItemCreateUI>().processType == ProcessType.Alteration)
                    {
                        icon = Instantiate(alterationIconPrefab, machineUI);
                    }
                    else if (result.gameObject.GetComponent<ProcessItemCreateUI>().processType == ProcessType.Slice)
                    {
                        icon = Instantiate(slicerIconPrefab, machineUI);
                    }
                    else if (result.gameObject.GetComponent<ProcessItemCreateUI>().processType == ProcessType.Merger)
                    {
                        icon = Instantiate(mergerIconPrefab, machineUI);
                    }

                    if (icon != null)
                        clickedImage = icon.GetComponent<Image>();
                }
                else if (result.gameObject.TryGetComponent(out ProcessTypeIconUI item))
                {
                    clicked = true;
                    GameObject icon = null;
                    Cursor.visible = false;

                    if (item != null)
                    {
                        icon = item.gameObject;
                    }

                    if (icon != null)
                        clickedImage = icon.GetComponent<Image>();
                }
            }
        }

        if (clicked)
        {
            MoveImage();
        }

        if (Input.GetMouseButtonUp(0))
        {
            clicked = false;
            clickedImage = null;
            Cursor.visible = true;
            selectedConnector = null;
            selectedProcessItem = null;
        }

        #endregion

        #region Right Click

        if (Input.GetMouseButtonDown(1))
        {
            newResults = CheckUIRaycast();

            foreach (RaycastResult result in newResults)
            {
                //print($"right clicked obj: {result.gameObject.name}");
                if (result.gameObject.TryGetComponent(out InputItemUI input))
                {
                    selected = true;
                    //Cursor.visible = false;

                    selectedConnector = input;
                    selectedConnector.connectorIndex = 0;
                }
                else if (result.gameObject.TryGetComponent(out ProcessTypeIconUI item))
                {
                    selected = true;
                    //Cursor.visible = false;

                    selectedProcessItem = item;
                }

            }
        }

        if (Input.GetMouseButton(1))
        {
            if (selected)
            {
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            newResults = CheckUIRaycast();

            foreach (RaycastResult result in newResults)
            {
                if (result.gameObject.TryGetComponent(out ProcessTypeIconUI item))
                {
                    selected = false;
                    //Cursor.visible = true;
                    item.connected = true;

                    if (selectedProcessItem != null)
                    {
                        selectedProcessItem.connectedProcess = item;
                        selectedProcessItem.connected = true;
                        item.connectorIndex = selectedProcessItem.connectorIndex + 1;
                    }

                    if (selectedConnector != null)
                    {
                        selectedConnector.connected = true;
                        selectedConnector.connectedProcess = item;
                        item.connectorIndex = selectedConnector.connectorIndex + 1;
                    }
                }
                else if (result.gameObject.TryGetComponent(out InputItemUI output))
                {
                    if (selectedProcessItem != null)
                    {
                        selectedProcessItem.inputItemUI = output;
                        selectedProcessItem.connected = true;
                        output.connected = true;
                        output.connectorIndex = selectedProcessItem.connectorIndex + 1;
                    }
                }
            }

            selectedConnector = null;
            selectedProcessItem = null;
        }

        #endregion
    }

    public void MoveImage()
    {
        if (clickedImage != null)
        {
            Vector2 anchoredPosition;
            mousePosition = Input.mousePosition;
            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out anchoredPosition);

            clickedImage.rectTransform.anchoredPosition = anchoredPosition;
        }
    }

    public List<RaycastResult> CheckUIRaycast()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        return results;
    }
}
