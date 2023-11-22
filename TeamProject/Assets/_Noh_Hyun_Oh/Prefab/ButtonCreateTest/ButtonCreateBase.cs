using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BattleActionUI;

public static class ButtonCreateBase<T> where T : Enum
{

    public static float width = 100.0f;
    public static float height = 50.0f;
    public static float fontSize = 24.0f;

    /// <summary>
    /// ��ư ���ӿ�����Ʈ ����� ��ư���ʿ��� �̹��� ������Ʈ �߰��ؼ� ���� ������.
    /// ��Ŀ�� �ǹ� ��ġ��� âũ�������� maskable ������ ��ư Ŭ���̺�Ʈ �ߵ��ϱ����� raycastTarget ���� 
    /// �׸��� �������� ���ڷι��� �׼��� �����ʿ� �����Ѵ�.
    /// </summary>
    /// <param name="objectName">��ư�̹������� ǥ������ �̸�</param>
    /// <param name="parentPosition">��ư�� ���Ե� �θ�Ʈ������</param>
    /// <param name="index">��ư�� �������϶� �Ʒ��� ��ġ������� ���� �ϳ��� �Է¾��ص��ȴ�.</param>
    /// <returns></returns>
    private static void CreateButtonObject(string objectName, Transform parentPosition, Action buttonListener, int index = 1)
    {
        GameObject tempObj = new GameObject(objectName); // ���� ������Ʈ����� 
        tempObj.transform.SetParent(parentPosition);     // Ʈ������ �Ű������� �ڽ����� ����ְ�
        tempObj.AddComponent<RectTransform>();           // RectTransform ���� �ٲ��ְ� 
        tempObj.AddComponent<Button>();                  // Button �߰� 

        Image tempImg = tempObj.AddComponent<Image>();   // Button �� ����� Image �����ϰ� 

        //�θ��� 
        GameObject childObj = new GameObject(objectName); // TextMeshPro �־��� ������Ʈ���� 

        //�ؽ�Ʈ ����
        TextMeshProUGUI tempText = childObj.AddComponent<TextMeshProUGUI>(); //TextMeshPro �־��ְ�
        childObj.transform.SetParent(tempObj.transform); // Button �� �θ�� ����
        tempText.text = objectName;                      // EnumList �� ���� �����ϰų� �⺻���� �����ϰ�
        tempText.fontSize = fontSize;                    // ��Ʈ���������� 
        tempText.alignment = TextAlignmentOptions.Center;// ���Ĺ���� �����


        RectTransform temprt = tempObj.GetComponent<RectTransform>(); //��ư RectTransform �� ã�ƿͼ� 
        //��ư ũ��� ��ġ���
        temprt.anchorMin =  Vector2.up;                                 //��Ŀ 0,1 ���� �������� ����
        temprt.anchorMax =  Vector2.up;                                 //��Ŀ 0,1 ���� �������� ����
        temprt.pivot =      Vector2.up;                                 //�ǹ� 0,1 ���� �������� ����
        temprt.anchoredPosition = new Vector2(0, -(height * index));    //���� ���ĵȰ��������� ����ؼ� �׹����� ����ְ�
        temprt.sizeDelta = new Vector2(width, height);                  //����� �ٽø����.
        //��Ÿ����
        tempImg.maskable = true;                                        //����ũ��������Ѽ� ��ư�� �θ���ġ���� ����� ���ߵ��ϼ���
        tempImg.raycastTarget = true;                                   //��ư�� Ŭ���̺�Ʈ�� ���� RayCast �� �������ְԼ���
        tempImg.color = Color.black;                                   //�� 

        tempObj.GetComponent<Button>().onClick.AddListener(() => 
                        { 
                            buttonListener?.Invoke(); 
                        });


    }

    /// <summary>
    /// �Ʒ��� �����Ǵ� ��ư�� �ڵ������ؼ� ���ڷ� ����� �Լ��� �������ش�.
    /// </summary>
    /// <param name="buttonPosition">��ư�� ���Ե� �θ� ������Ʈ��ġ</param>
    /// <param name="_">��ư�̿������϶� Enum �� �ƹ����̳� �ѱ�� Ÿ�Ը��ʿ���</param>
    /// <param name="function">��ư�� ����� ��������Ʈ Enum ������ ������Ѵ�</param>
    static public void SettingNoneScriptButton(Transform buttonPosition, T _, Action[] function)
    {
        Array battleButtonEnumList = Enum.GetValues(typeof(T)); // ��ư���� ��ũ��Ʈ����� �����Ƽ� �ڵ����� ó���ϱ����� enum ����Ʈ.

        RectTransform rt = buttonPosition.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height * battleButtonEnumList.Length);
        for (int i = 0; i < battleButtonEnumList.Length; i++) //�̳� ũ�⸸ŭ�� ������
        {
            CreateButtonObject(battleButtonEnumList.GetValue(i).ToString(), buttonPosition, function[i], i);//�и��� �Լ�����ȴ�.
        }
    }

    /// <summary>
    /// �Ʒ��� �����Ǵ� ��ư�� �ڵ������ؼ� ���ڷ� ����� �Լ��� �������ش�.
    /// </summary>
    /// <param name="buttonPosition">��ư�� ���Ե� �θ� ������Ʈ��ġ</param>
    /// <param name="function">��ư�� ����� ��������Ʈ </param>
    /// <param name="objectName">��ư�� ǥ�õ� �̸�</param>
    static public void SettingNoneScriptButton(Transform buttonPosition, Action function , string objectName = "Empty")
    {
        CreateButtonObject(objectName, buttonPosition, function);
    }
}
