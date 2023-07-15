using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PathLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        
    }

    /// <summary>
    /// ��θ� �׸��� �Լ� 
    /// </summary>
    /// <param name="map">������ǥ�� ���ϴµ� �ʿ��� ��</param>
    /// <param name="path">���� �׸�����ǥ�� ������ ��� (A* ���)</param>
    public void DrawPath(GridMap map, List<Vector2Int> path)
    {
        if (map != null && path != null && gameObject.activeSelf)// activeSelf �ʰ� ��ΰ� �־�� �׸���
        {
            lineRenderer.positionCount = path.Count;// ��� ������ŭ ���η����� ��ġ���� ����

            int index = 0;//��ġ �ε��� 
            foreach(Vector2Int node in path)//��� ��θ� ���鼭 ��ġ ����
            {
                Vector2 worldPos = map.GridToWorld(node);// �׸�����ǥ�� ������ǥ�� ����
                // Vector3 localPos = (Vector3)worldPos - transform.position;//���̷��������� ������ǥ��� üũ�� ������ ��� 
                lineRenderer.SetPosition(index, worldPos);//���η������� ��ġ����
                index++;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;//���̳� ��ΰ� ��Ȱ�������̸� ������ 0���� ����� �Ⱥ��̰� �Ѵ�
        }
    }


    public void ClearPath()//�׷����� ��θ� �ʱ�ȭ �ϴ� �Լ� 
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        gameObject.SetActive(false);
    }
}
