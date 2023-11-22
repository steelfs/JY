using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpstionsButton : MonoBehaviour
{
   
   public  void CloseWindow() {
        WindowList.Instance.MainWindow.gameObject.SetActive(false);
    }
}
