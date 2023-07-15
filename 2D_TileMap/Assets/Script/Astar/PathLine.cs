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
    /// 경로를 그리는 함수 
    /// </summary>
    /// <param name="map">월드좌표를 구하는데 필요한 맵</param>
    /// <param name="path">맵의 그리그좌표로 구해진 경로 (A* 결과)</param>
    public void DrawPath(GridMap map, List<Vector2Int> path)
    {
        if (map != null && path != null && gameObject.activeSelf)// activeSelf 맵과 경로가 있어야 그린다
        {
            lineRenderer.positionCount = path.Count;// 경로 갯수만큼 라인렌더러 위치갯수 설정

            int index = 0;//위치 인덱스 
            foreach(Vector2Int node in path)//모든 경로를 돌면서 위치 세팅
            {
                Vector2 worldPos = map.GridToWorld(node);// 그리드좌표를 월드좌표로 변경
                // Vector3 localPos = (Vector3)worldPos - transform.position;//라이렌더러에서 월드좌표사용 체크를 안했을 경우 
                lineRenderer.SetPosition(index, worldPos);//라인랜더러의 위치설정
                index++;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;//맵이나 경로가 비활성상태이면 갯수를 0으로 만들어 안보이게 한다
        }
    }


    public void ClearPath()//그려놓은 경로를 초기화 하는 함수 
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        gameObject.SetActive(false);
    }
}
