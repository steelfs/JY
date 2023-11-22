using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Potal_SceneChange_UI : MonoBehaviour, IPopupSortWindow, IPointerClickHandler
{
    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

    //public Button prefabObj;

    //private void Awake()
    //{
        //Transform content = transform.GetComponentInChildren<VerticalLayoutGroup>().transform;

    //}




    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupSorting(this);
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
        PopupSorting(this);
    }
}
