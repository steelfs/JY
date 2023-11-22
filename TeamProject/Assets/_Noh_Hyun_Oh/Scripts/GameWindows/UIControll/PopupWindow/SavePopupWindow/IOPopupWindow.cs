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
        WindowList.Instance.IOPopupWindow.NewIndex = -1; //�� �ʱ�ȭ
        WindowList.Instance.IOPopupWindow.OldIndex = -1; //ī�� �� �ʱ�ȭ
        WindowList.Instance.IOPopupWindow.CopyCheck = false; //ī�� �� �ʱ�ȭ

    }
}
