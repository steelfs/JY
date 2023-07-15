using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
//static�� new ��� �Ұ��� 
public static class AStar //�ܼ� ���� Ŭ����
{

    /// <summary>
    /// // Astar �˰������� ���� Ž���ϴ� �Լ�//�⺻������ ��ã��˰����� ���̴�. ���� �׷����� ���� �����ϸ� �ش�׷��� �߽����� �������� �ѹ��� �����Ѵ�.
    /// </summary>
    /// <param name="map">���� ã�� ��</param>
    /// <param name="start">������ġ</param>
    /// <param name="end">������ġ</param>
    /// <returns> ��� -> �������� ���� ��� ��ã�� ���н� null</returns>
    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end)
    {
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.4f;//�밢������ �̵��� ��� �Ÿ�

       
        List<Vector2Int> path = null;

        if (map.IsValidPosition(start) && map.IsValidPosition(end))//�Ķ���ͷ� ���� ��ǥ�� ���� �ȿ� �ִٸ� 
        {
            map.ClearMapData();//������ �� �� ����� �ʱ�ȭ �ѹ� ����

            List<Node> open = new List<Node>();// ������ Ž���� �ĺ������ ����Ʈ
            List<Node> close = new List<Node>();//Ž���� �Ϸ�� ����Ʈ

            Node current = map.GetNode(start); //A* ���� // start�� �����ͼ�
            current.G = 0;
            current.H = GetHeuristic(current, end); //G, H �ʱ� ���� �� 
            open.Add(current);// openList�� �߰�

            while(open.Count > 0)//���¸���Ʈ(�ĺ�) �� ��尡 ����ִٸ� ��� �ݺ�, openList�� ��� ��ã�� ����
            {
                open.Sort();// //F�� ��������  ���� ��
                current = open[0];//F���� ���� ���� ��带 current�� ����
                open.RemoveAt(0); // ���� ��带 OpenList���� ����
                if (current != end) // ������ġ�� ������������ Ȯ��
                {
                    close.Add(current);//close����Ʈ�� �־ �� ���� Ž�������� ǥ��

                    for (int y = -1; y<2; y++) //current�� �ֺ� 8���� ��带 openList�� �ְų� G�� ����
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = map.GetNode(current.x + x, current.y + y);//  �ֺ���� �����ͼ�

                            //��ŵ�� ��� üũ
                            if (node == null) continue; //��尡 �� ���� ���̸� continue
                            if (node == current) continue; //�ڱ��ڽ�
                            if (node.nodeType == Node.NodeType.Wall) continue; //���� �� 
                            if (close.Exists((x) => x == node)) continue;//��尡 Ŭ�����Ʈ�� ���� �� 
                            bool isDiagonal = (x * y) != 0;   //�밢�� ����
                            //�밢���� ��� Ȯ���� �� �ִ°�  //isDiagonal = Mathf.Abs(x) != Mathf.Abs(y);

                            if (isDiagonal && (map.IsWall(current.x + x, current.y) || map.IsWall(current.x, current.y + y))) //�밢���� �� �� ���� ��� �� �ϳ��� �� �϶�  
                                continue;

                            float distance;//�Ÿ� ���� 
                            if (isDiagonal) distance = diagonalDistance; //�밢���̸� 1

                            else
                                distance = sideDistance;

                            if (node.G > current.G + distance) //����� g���� current�� ���ļ� ���� ���� G������ ũ�� ����
                            {
                                if (node.parent == null) //����� �θ� ������ ���� openList�� �ȵ� ������ �Ǵ�
                                {
                                    //openList�� ���� �ȵ� �ִٸ� 
                                    node.H = GetHeuristic(node, end); //�޸���ƽ ���
                                    open.Add(node);//open ����Ʈ�� �߰�
                                }
                                node.G = current.G + distance;//G�� ����
                                node.parent = current;//current�� ���ļ� �����ߴٰ� ǥ��
                            }
                            



                            //(-1, 1), (0,  1), (1,  1)
                            //(-1, 0), (0,  0), (1,  0)
                            //(-1,-1), (0, -1), (1, -1)
                        }
                    }
                }
                else//���������̶��
                {
                    break;
                }
            }
            //�������۾�
            if (current == end)//�������� �����ߴٸ� 
            {
                path = new List<Vector2Int>();//��� �����
                Node result = current;
                while(result != null) //������ġ�� �����Ҷ� ���� ��� �߰�
                {
                    path.Add(new Vector2Int(result.x, result.y));
                    result = result.parent;
                }
                path.Reverse(); //���� -> ��߷� ���� ����Ʈ�� ������ 
            }
        }
        return path;
    }

    /// <summary>
    /// �޸���ƽ���� ����ϴ� �Լ�  (������ġ���� ���������� ����Ÿ� ���ϱ�)
    /// </summary>
    /// <param name="current">������</param>
    /// <param name="end">��������</param>
    /// <returns>current ���� end���� ����Ÿ�</returns>
    private static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.x - end.x) + Mathf.Abs(current.y - end.y);//���밪 ���� ���� ����
    }
}
