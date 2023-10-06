using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_CrossHair : TestBase
{
     

    public AnimationCurve curve;
    public CrossHair crossHair_;

    public float expandAmount = 30.0f;

    [Range(0, 1)]
    public float testValue = 0;

    protected override void Test1(InputAction.CallbackContext context)
    {
        Debug.Log(curve.Evaluate(testValue));
    }
    protected override void TestClick(InputAction.CallbackContext context)
    {
        crossHair_.Expand(expandAmount);
        //Shoot();
    }

    RectTransform north;
    RectTransform south;
    RectTransform east;
    RectTransform west;

    Vector2 fixedPosNorth;
    Vector2 fixedPosSouth;
    Vector2 fixedPosEast;
    Vector2 fixedPosWest;

    Vector2 northOrigin;
    Vector2 southOrigin;
    Vector2 eastOrigin;
    Vector2 westOrigin;

    protected override void Awake()
    {
        base.Awake();
        north = crossHair_.transform.GetChild(0).GetComponent<RectTransform>();
        south = crossHair_.transform.GetChild(1).GetComponent<RectTransform>();
        east = crossHair_.transform.GetChild(2).GetComponent<RectTransform>();
        west = crossHair_.transform.GetChild(3).GetComponent<RectTransform>();

        northOrigin = north.anchoredPosition;
        southOrigin = south.anchoredPosition;
        eastOrigin = east.anchoredPosition;
        westOrigin = west.anchoredPosition;

    }

    public void Shoot()
    {
        fixedPosNorth = north.anchoredPosition;
        fixedPosNorth.y += 10;
        fixedPosSouth = south.anchoredPosition;
        fixedPosSouth.y -= 10;
        fixedPosEast = east.anchoredPosition;
        fixedPosEast.x += 10;
        fixedPosWest = west.anchoredPosition;
        fixedPosWest.x -= 10;
        north.anchoredPosition = fixedPosNorth;
        south.anchoredPosition = fixedPosSouth;
        east.anchoredPosition = fixedPosEast;
        west.anchoredPosition = fixedPosWest;


    }
    private void Update()
    {
        //north.anchoredPosition = Vector2.Lerp(north.anchoredPosition, northOrigin, testValue);
        //south.anchoredPosition = Vector2.Lerp(south.anchoredPosition, southOrigin, testValue);
        //east.anchoredPosition = Vector2.Lerp(east.anchoredPosition, eastOrigin, testValue);
        //west.anchoredPosition = Vector2.Lerp(west.anchoredPosition, westOrigin, testValue);


    }
   

}
