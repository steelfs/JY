using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;

    public BackTrackingCell(int x, int y) : base(x, y) // base���� ���� ��  ���� 
    {
        visited = false;
    }
  
}
public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
       
    
        //Recursive BackTracking
        //�������� ����, ����
        //�ش� ����  �湮 ǥ��
        //�����ϰ� �� ���� �� �� ������ ����.(�湮�� ���� ���� ����)
        // �� ���� ��� �湮�ߴٸ� ���� ���� ���ư���.
        // ���������� ������������ ���ư��� �˰��� ����
        //
    }

  
}
