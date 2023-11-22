using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOPopupWindow : MonoBehaviour
{
    private void OnEnable()
    {
        WindowList.Instance.ActivePopup = EnumList.PopupList.SAVE_LOAD_POPUP;
    }
    private void OnDisable()
    {
        WindowList.Instance.ActivePopup = EnumList.PopupList.NONE;
        WindowList.Instance.IOPopupWindow.NewIndex = -1; //값 초기화
        WindowList.Instance.IOPopupWindow.OldIndex = -1; //카피 값 초기화
        WindowList.Instance.IOPopupWindow.CopyCheck = false; //카피 값 초기화

    }
}
