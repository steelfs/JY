using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsButton : MonoBehaviour
{
 
    public void OnClickOptions()
    {
        WindowList.Instance.MainWindow.gameObject.SetActive(true);

    }
}
