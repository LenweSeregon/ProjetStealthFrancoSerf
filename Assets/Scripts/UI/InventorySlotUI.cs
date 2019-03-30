using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector]
    public int index;
    [HideInInspector]
    public InventorySlot slotAssociated;
    [HideInInspector]
    public InventoryBuilder builderReference;
    public Slider progressBar;
    public GameObject progressBarFillArea;

    private Vector3 gapImageProgress;
    private Vector3 gapImageQuantity;
    private Transform originalParent;
    private Vector3 originalPositionImage;
    private Vector3 originalPositionQuantity;
    private Vector3 originalPositionProgressBar;
    private Transform canvas;

    private Image image;
    private TextMeshProUGUI quantity;


    public void Build()
    {
        progressBar.value = 0;
        image = transform.Find("Image").GetComponent<Image>();
        quantity = transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

        if (slotAssociated == null)
        {
            image.enabled = false;
            quantity.enabled = false;
            progressBar.gameObject.SetActive(false);
        }
        else
        {
            progressBarFillArea.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(false);
            image.enabled = true;
            quantity.enabled = true;
            image.sprite = slotAssociated.Item.GetIcon();
            quantity.text = slotAssociated.Quantity.ToString();

            if(slotAssociated.Item.GetMaxDurability() > -1)
            {
                progressBar.gameObject.SetActive(true);

                int actualDurability = slotAssociated.Item.GetCurrentDurability();
                int maxDurability = slotAssociated.Item.GetMaxDurability();
                float percentage = actualDurability / (float)maxDurability;
                if(percentage == 0)
                {
                    progressBarFillArea.gameObject.SetActive(false);
                }
                else
                {
                    progressBar.value = percentage;
                }
            }
        }
    }

    public void CallBuilderClicked()
    {
        if(slotAssociated != null)
        {
            builderReference.InventorySlotClicked(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(slotAssociated != null)
        {
            image.raycastTarget = false;
            builderReference.isDragging = true;
            builderReference.currentlyDragging = this;
            gapImageProgress = progressBar.transform.position - image.transform.position;
            gapImageQuantity = quantity.transform.position - image.transform.position;
            originalPositionImage = image.transform.position;
            originalPositionQuantity = quantity.transform.position;
            originalPositionProgressBar = progressBar.transform.position;

            originalParent = transform;
            canvas = GameObject.FindGameObjectWithTag("UICanvas").transform;

            image.transform.SetParent(canvas, false);
            quantity.transform.SetParent(canvas, false);
            progressBar.transform.SetParent(canvas, false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        image.transform.position = Input.mousePosition;
        quantity.transform.position = image.transform.position + gapImageQuantity;
        progressBar.transform.position = image.transform.position + gapImageProgress;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        image.transform.position = originalPositionImage;
        quantity.transform.position = originalPositionQuantity;
        progressBar.transform.position = originalPositionProgressBar;

        image.transform.SetParent(originalParent, true);
        quantity.transform.SetParent(originalParent, true);
        progressBar.transform.SetParent(originalParent, true);

        builderReference.EndDraggingOperation();
        builderReference.isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (builderReference.isDragging)
        {
            builderReference.endDragging = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (builderReference.isDragging)
        {
            builderReference.endDragging = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && slotAssociated != null)
        {
            builderReference.InventorySlotActionClicked(this);
        }
        else
        {
            builderReference.HideActionPanel();
        }
    }
}
