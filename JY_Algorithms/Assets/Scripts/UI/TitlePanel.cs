using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Animator titleTextAnim;
    public Button newGameButton;
    public Button loadButton;
    public ChooseCharactor_Panel Charactor_Panel;
    public void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        titleTextAnim.SetTrigger("TitleTextPopup");
    }
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
