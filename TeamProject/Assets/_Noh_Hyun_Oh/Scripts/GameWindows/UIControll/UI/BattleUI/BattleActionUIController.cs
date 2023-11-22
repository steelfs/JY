using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static BattleActionUI;

/// <summary>
/// �׼� UI Button���� �����̴� ���
/// </summary>
public class BattleActionUIController : MonoBehaviour
{
    /// <summary>
    /// ������ ��ư �迭 ���ϱ� 
    /// </summary>
    [SerializeField]
    RectTransform[] buttonRects;

    /// <summary>
    /// ��ư���� ������ ũ������ �ϱ����� �ʿ��� ����
    /// </summary>
    RectTransform controllerUI;

    /// <summary>
    /// ���콺 �÷����� ��ư�� �������� �ӵ� 
    /// </summary>
    [SerializeField]
    private float viewSpeed = 1000.0f;

    /// <summary>
    /// ��ư���� ���� ��ġ�� ���� 
    /// </summary>
    float[] moveValues;

    /// <summary>
    /// ��ư���� �⺻ ���̰� ���ϱ� 
    /// </summary>
    private float viewHeight = 0.0f;

    /// <summary>
    /// ���콺 �÷����� �ε巴�� ���������� ���� �ڷ�ƾ
    /// </summary>
    IEnumerator OnUpView;

    /// <summary>
    /// ���콺 �÷����� �ε巴�� ������ ���� ���� �ڷ�ƾ
    /// </summary>
    IEnumerator OnDownView;

    /// <summary>
    /// �׼� ��ư�� �ƹ��͵��������� ��ư�� Ȱ��ȭ�� �� �̰� ��Ȱ��ȭ ��Ű�¿����� �����´�.
    /// </summary>
    GameObject actionButton;

    /// <summary>
    /// ��ġ ����� �ӽù��� �ΰ�.
    /// </summary>
    Vector2 tempMin = Vector2.zero;
    Vector2 tempMax = Vector2.zero;

    /// <summary>
    /// �θ� Ȱ��ȭ �������� Ȯ���ϴ� ������Ƽ
    /// </summary>
    bool IsParentVisible 
    {
        get 
        {
            bool isCheck = true;
            Transform parent = transform.parent;
            do
            {
                isCheck = parent.gameObject.activeSelf;
                parent = parent.parent;
            } while (parent != null && isCheck);

            return isCheck;
        }
    }
    private void Awake()
    {
       
        ///��Ʈ�ѷ��� �ڽĹؿ� �־�Ѵ�.
        Transform parent = transform.parent; //�׷��� �θ� ã��

        buttonRects = new RectTransform[parent.childCount - 2]; //Action��ư �� ���콺 �ڵ鷯 �̺�Ʈ ������Ʈ �ΰ� �������� ��������
        moveValues = new float[parent.childCount - 2]; //���ప ���� �迭 �����ϰ�
        for (int i = 0; i < buttonRects.Length; i++) //�������鼭 
        {
            buttonRects[i] = parent.GetChild(i).GetComponent<RectTransform>(); // ã�ƿ���
            moveValues[i] = 0.0f; //���ʱ�ȭ
        }
        viewHeight = buttonRects[0].rect.height; //��ư�� ���̵��� ���� �����ϴٴ� ���� �Ͽ� ��ư���� ��������  ��ư���� �ٴٸ���� �迭�� ��ü�ʿ���.
        actionButton = parent.GetChild(parent.childCount-2).gameObject; // ã�ƿ���
        controllerUI = GetComponent<RectTransform>(); // �̰͵� ã��
    }

    private void OnDisable()
    {
        ResetButtons();
    }

    /// <summary>
    /// ���콺 �ö󰡸� ��Ʋ �׼ǹ�ư�� Ȱ��ȭ 
    /// </summary>
    public void ViewButtons() 
    {
        actionButton.SetActive(false); //Ȱ��ȭ���� �׼ǹ�ư������

        StopAllCoroutines(); // Ȱ��ȭ�� ��Ȱ��ȭ ���� ���� ���߰� 
        if (gameObject.activeSelf && IsParentVisible) //Ȱ��ȭ ���¸� �ڷ�ƾ�� �����Ҽ��ִ�.
        {
            for (int i = 0; i < buttonRects.Length; i++)
            {
                OnUpView = SetUpView(i);    //������ ������� �������� ������ �Ѵ�. 
                StartCoroutine(OnUpView);   // Ȱ��ȭ ���� 
            }
        }
        else
        { 
            foreach (RectTransform rt in buttonRects)
            {
                SetTopBottomValue(rt, 0.0f); //��ġ �ʱ�ȭ
            }
        }

    }
    /// <summary>
    /// ���콺 ����� ��Ʋ �׼ǹ�ư�� ��Ȱ��ȭ
    /// </summary>
    public void ResetButtons() 
    {
        StopAllCoroutines(); //���������� �ڷ�ƾ �ٸ��߰�
        if (gameObject.activeSelf && IsParentVisible) //Ȱ��ȭ ���¸� �ڷ�ƾ�� �����Ҽ��ִ�.
        {
            for (int i = 0; i < buttonRects.Length; i++)
            {
                OnDownView = SetDownView(i); //������ ������� �������� ������ �Ѵ�. 
                StartCoroutine(OnDownView);  //������ġ�� ���� ���� ����
            }
        }
        else 
        {
            foreach (RectTransform rt in buttonRects) 
            {
                SetTopBottomValue(rt, 0.0f); //��ġ �ʱ�ȭ
            }
        }
        actionButton.SetActive(true);  //����ġ��� �׼ǹ�ư Ȱ��ȭ 
    
    }
    /// <summary>
    /// rectTransform �� top �� bottom �� �̿��Ͽ� ��ġ ���� ��Ű�� ����
    /// </summary>
    /// <param name="index">��ư ����</param>
    /// <returns></returns>
    private IEnumerator SetUpView(int index) 
    {
        if (buttonRects.Length > 0) 
        {
            
            //while�� ���������� 
            float checkValue  = viewHeight * index; // �ִ���� ��ŭ ����ش�
            while (moveValues[index] <= checkValue)
            {
                moveValues[index] += Time.deltaTime * viewSpeed; //�ε巴�� ������Ű������ ��ŸŸ�� ������Ű��
                moveValues[index] = checkValue < moveValues[index] ? checkValue : moveValues[index]; //���ǵ带 �ʹ����������� ��ġ������°�찡�������� ����
                SetTopBottomValue(buttonRects[index], moveValues[index]);
                yield return null;
            }
        }
    }
    /// <summary>
    /// rectTransform �� top bottom �� �̿��Ͽ� ��ġ ���� ��Ű�� ���� 
    /// </summary>
    /// <param name="index">��ư����</param>
    /// <returns></returns>
    private IEnumerator SetDownView(int index) 
    {
        if (buttonRects.Length > 0)
        {

           
            while (moveValues[index] >= 0) //�̰��ϰ� 
            {
                moveValues[index] -= Time.deltaTime * viewSpeed; //�̰��� �ٸ��� ������ ����
                moveValues[index] = 0 > moveValues[index] ? 0 : moveValues[index]; //���ǵ带 �ʹ����������� ��ġ������°�찡�������� ����
                SetTopBottomValue(buttonRects[index], moveValues[index]);
                yield return null;
            }
            
        }
    }
    /// <summary>
    ///  rectTransform �� top�� bottom ���� 
    ///  �������ڰ����� �����Ѵ�  top �� -���ڰ�  bottom �� ���ڰ� �״�� 
    ///<param name="value">������ ��</param>
    ///<param name="rt">������ ��ƮƮ������</param>
    /// </summary>
    private void SetTopBottomValue(RectTransform rt , float value)
    {
        tempMin = rt.offsetMin; //bottom
        tempMax = rt.offsetMax; //top
        tempMin.y = value; //bottom ���� ����� �״�� ����
        tempMax.y = value; //top ���� ����� ������ ���������� ��ȯ�Ǿ� ����.
        rt.offsetMin = tempMin; //bottom
        rt.offsetMax = tempMax; //top
    }
    
}
