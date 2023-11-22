using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    public RectTransform contentsRect;
    public int overCount = 0;
    public float imageSize = 60.0f;
 
    public void On_ValueChanged()
    {
        if (contentsRect.anchoredPosition.y < 0)
        {
            contentsRect.anchoredPosition = Vector2.zero;
        }
        else if (contentsRect.anchoredPosition.y > (imageSize * overCount))
        {
            contentsRect.anchoredPosition = new Vector2(0, imageSize * overCount);
        }
    }
}
