using System.Collections.Generic;
using System.Linq;
/*   정렬기능만 제공할것이기때문에static 으로 만들어서 상속은 할수없다.    
     enum 은 상수이기때문에 인터페이스에서 상속할수없고 제네릭으로 가져오기힘들다.
     그렇다고 제네릭을 클래스로받으면 상속때문에 문제가발생한다.
     ChangeObj(ref listData[min],ref listData[i]); CS0206 오류발생 프로퍼티나 배열이나 리스트의 인덱스는 ref 또는 out 을 사용할수가없다고한다.
     배열, 리스트 인덱스 또한 함수로 되있는거같다.
*/
/// <summary>
///     
///     클래스 내부 값(맴버변수 or 프로퍼티)을 기준으로 전체 정렬하고싶을때 사용할 클래스이다
///     사용방법 정렬할 컴퍼넌트를 ISortBase 를 상속받아서 내부프로퍼티를 작성한다.
///     T는 컴퍼넌트 고 매개변수로는 T의 배열을 넘긴다 
///     SortProccessType 은 정렬방식이라 입력안해도 상관없다 기본은 삽입정렬로 진행.
///     SortType 은 오름차순이 기본으로되있다 
///     SortComponent<T>.SorttingData(T[] 또는 List<T>,SortComponent<T>.SortType,SortComponent<T>.SortProccessType); 
///     
///     enum SortType 에 추가한값들은 테스트완료
/// </summary>
/// <typeparam name="T">ISortBase 인터페이스를 상속받은 클래스</typeparam>

public static class SortComponent<T> where T : ITurnBaseData 
{
    /// <summary>
    /// 정렬 알고리즘의 종류
    /// 구현이 더필요하다.
    /// </summary>
    public enum SortProccessType
    {
        InsertSort = 0, //삽입 정렬하기 
        SelectionSort,  //선택 정렬하기
        BubbleSort,     //버블 정렬하기
    }


   
    /// <summary>
    /// IEnumerable 은 쓰기가안되서 ICollection 을 사용해보앗다.
    /// ICollection는 인터페이스 자체가 두종류가 존재하여 Array 와 List 의 공통된 인터페이스인 IList 를 이용하였다. 덕분에 오버로딩 없이 사용할수있다.
    /// 정렬 알고리즘의 종류별로 연결해주는 함수
    /// 알고리즘 출처 : https://hyo-ue4study.tistory.com/68
    ///<param name="data">정렬할 데이터</param>
    ///<param name="proccessType">정렬할 알고리즘타입</param>
    ///<param name="type">정렬방법</param>
    /// </summary>
    public static void SorttingData(IList<T>  data,                                            //리스트든 배열이든 받을수있는 인터페이스를 인자값으로 받아서 처리해보았다. 
                                    SortProccessType proccessType = SortProccessType.InsertSort,    //정렬알고리즘이 여러개라 구분하기위해 넣어놨다.
                                    SortType type = SortType.Ascending                             //오름차순 내림차순 구분하기위한 값이다.
                                    )
    {
        int length = data.Count(); //의미없음 미리뽑은것뿐

        switch (proccessType) // 정렬방법선택
        {
            case SortProccessType.InsertSort: //삽입정렬
                InsertionSort(ref data , length, type); 
                break;
            case SortProccessType.SelectionSort: //선택정렬
                SelectionSort(ref data , length, type);
                break;
            case SortProccessType.BubbleSort: //버블정렬
                BubbleSort(ref data , length, type);
                break;
        }

    }

    /// <summary>
    /// 삽입정렬 알고리즘을 가져와서 사용했다\
    /// 삽입정렬은 비교할 값중 작은값을 왼쪽(인덱스의 작은값의 방향)으로 정렬시키는것이다.
    /// 왼쪽과 비교를하면서 하나씩 정렬하기때문에 j포문의 횟수는 뒤로갈수록 커진다.
    /// 비교할대상이없을경우 break; 로 빠져나오기때문에 불필요한 비교는 최대한으로 줄인다..
    /// </summary>
    /// <param name="listData">배열목록</param>
    /// <param name="length">배열의 크기</param>
    /// <param name="type">정렬종류</param>
    private static void InsertionSort(ref IList<T> listData, int length, SortType type)
    {

        T key; //정렬할 객체
        for (int i = 1; i < length; i++) //1번째부터 배열의 크기만큼 정렬을 시작 
        {
            int j = 0; // j 값을 i 부분에서 사용해야함으로 여기에 선언
            key = listData[i];// 0번이아니라 1번부터 비교할 객체을 담고 
            for (j = i - 1; j >= 0; j--) //담은 값의 -1번째부터 비교를 시작하여 
            {
                if (SortAscDesCheck(key, listData[j], type)) //오름차순일땐  key 값보다 큰값이 존재할경우  내림차순일땐 key값보다 작은값이 존재할경우
                {
                    listData[j + 1] = listData[j];  // 한칸씩 뒤(+)로 이동시킨다.
                }
                else //이미정렬된값중에 비교할 객체보다 작으면 위치를 더이상 이동할필요가없기때문에 j포문을 빠져나간다.
                {
                    break;
                }
            }
            listData[j + 1] = key; //  j포문의 위치값을 가져와서 마지막비교한값의 바로뒤에 비교할객체의 데이터를 담아서 i포문의 하나의정렬을끝낸다. 
        }

    }
    /// <summary>
    /// 오름차순 과 내림차순을 나눠서 체크해주는 함수
    /// </summary>
    /// <param name="min">작아야될값</param>
    /// <param name="max">커야될 값</param>
    /// <param name="sortType">오름차순 내림차순선택값</param>
    /// <returns></returns>
    public static bool SortAscDesCheck(ITurnBaseData min, ITurnBaseData max, SortType sortType)
    {
        bool resultValue = false;
        switch (sortType)
        {
            case SortType.Ascending:
                resultValue = min.TurnActionValue < max.TurnActionValue; // 올림차순일경우 -100 ~ 100
                break;
            case SortType.Descending:
                resultValue = min.TurnActionValue > max.TurnActionValue; // 내림차순일경우 100 ~ -100
                break;
        }
        return resultValue;
    }


    /// <summary>
    /// 선택정렬 삽입정렬과는 반대로 +방향으로 비교를하기때문에 
    /// 처음 비교할때 j포문의 횟수가 가장길다.
    /// </summary>
    /// <param name="arrayData">배열목록</param>
    /// <param name="length">배열의 크기</param>
    private static void SelectionSort(ref IList<T> listData, int length, SortType type)
    {
        for (int i = 0; i < length - 1; i++) // 배열크기만큼 순차적으로 돌리고
        {
            int min = i; //최소값을 찾기위한 위치값을 저장한다
            for (int j = i + 1; j < length; j++) //-방향은 최소값정렬이끝난곳이니 +값으로 탐색을 시작한다.
            {
                if (SortAscDesCheck(listData[j], listData[min], type))// min값을 비교해서 min보다 작은값이 있는경우 
                {
                    min = j;//최소값위치를 수정한다.
                }//반복하여 포문전체의 최소값위치를 찾는다.
            }
            if (i != min)
            {
                //최소값이 현재 값과 같지않을때만 교체한다.
                //함수로안뺀이유는 함수로빼는순간 T[i] 값을 매개변수로 넘길때 참조타입이 아니라 값타입으로 치환이 되서 넘어가기때문이다.이유는모르겟다.
                //이유는 C# 구조가 프로퍼티나 배열이나 리스트의 인덱스는 매개로넘길때 값타입으로 자동치환이 된다고한다. 
                // ref 나 out 를 쓸수도없는게 프로퍼티나 인덱스는 적용안된다. 변수가 아니라 함수라서.
                (listData[min], listData[i]) = (listData[i], listData[min]);//튜플이라는것 두값을 교환해준다 람다식비슷한것같다.
            }
        }
    }
    /// <summary>
    /// 버블정렬 인접한 두객체를 비교한다.
    /// </summary>
    /// <param name="arrayData">배열 목록</param>
    /// <param name="length">배열 크기</param>
    private static void BubbleSort(ref IList<T> arrayData, int length, SortType type)
    {
        int i = 0;
        int j = 0;

        for (i = 0; i < length - 1; i++) //첫번째부터 마지막전것까지 비교 
        {
            for (j = 0; j < length - 1 - i; j++) // 뒤로갈수록 점점 비교횟수를 줄여간다.
            {
                if (SortAscDesCheck(arrayData[j + 1], arrayData[j], type))//뒤에 값과 현재값을 비교해서 뒤에값이 작으면 
                {
                    //현재값과 뒤에값을 교체한다. 
                    (arrayData[j + 1], arrayData[j]) = (arrayData[j], arrayData[j + 1]);//튜플이라는것 두값을 교환해준다 람다식비슷한것같다.
                }
            }
        }
    }
    //==============================================  아래는 링크드 리스트 정렬 기능 ==========================================

    /// <summary>
    /// 링크드리스트 정렬. 알고리즘 : 버블정렬
    /// 버블정렬은 인접한 두객체를 비교하면서 교체하기때움에 링크드 리스트의 특성과도 잘맞는다 
    /// 테스트 완료 
    /// </summary>
    /// <param name="turnObjectList">정렬할데이터 ISortBase 링크드리스트</param>
    /// <param name="type">오름정렬Asc 내림정렬Des</param>
    public static void BubbleSortLinkedList(LinkedList<ITurnBaseData> turnObjectList, SortType type = SortType.Ascending)
    {
        LinkedListNode<ITurnBaseData> key;
        LinkedListNode<ITurnBaseData> temp;
        int length = turnObjectList.Count;
        for (int i = 0; i < length - 1; i++)
        {
            key = turnObjectList.First;//무조건 처음부터 검색시작
            for (int j = 0; j < length - 1 - i; j++)//맨처음부터 끝까지 돌면서 시작 한바퀴돌때마다 포문횟수는 점점 줄어든다
            {
                if (SortAscDesCheck(key.Next.Value, key.Value, type)) //값을 비교하여 교체필요하면 
                {   //교체
                    temp = key.Next; //뒤에거 임시객체에 담아두고 
                    turnObjectList.Remove(key);//앞에있던객체 링크드리스트에서 제거 
                    turnObjectList.AddAfter(temp, key);//그런후 뒤에있던 임시객체를 통해 뒤에다가 노드 추가하여 뒤에있던것과 앞에있던것의 위치를 바꾼다.
                }
                else
                {
                    key = key.Next; //교체할값이없으면 다음 객체로 비교대상을 옮긴다.
                }

            }

        }

    }


}
/*


아래내용은 시간나면 추가할예정 

4. 병합 정렬(Merge Sort)

설명 : 둘 이상의 부분집합으로 가르고, 각 부분집합을 정렬한 다음 부분집합들을 다시 정렬된 형태로 합치는 방식. 안정성은 보장한다.

- 분할 정복법 사용(Divide-And-Conquer)

분할 : 해결하고자 하는 문제를 작은 크기의 동일한 문제들로 분할한다.
정복 : 각각의 작은 문제를 순환적으로 해결한다.
합병 : 작은 문제의 해를 합하여(merge) 원래 문제에 대한 해를 구한다.







#include<iostream>
using namespace std;
#define ARRNUM 5
int N = ARRNUM;
int arr[] = { 8,5,3,1,6 };
int tempArr[ARRNUM];

void Merge(int left, int right)
{
    //절반짜리 arr을 tempArr에복사한다.
    for (int i = left; i <= right; i++)
    {
        tempArr[i] = arr[i];
    }

    int mid = (left + right) / 2;

    int tempLeft = left;
    int tempRight = mid+1;
    int curIndex = left;

    //temparr배열 수횐하. 왼쪽 절반과 오른쪽 절반 비교해서
    //더 작은 값을 원래 배열에 복사
    while (tempLeft <= mid && tempRight <= right)
    {
        if (tempArr[tempLeft] <= tempArr[tempRight])
        {
            arr[curIndex++] = tempArr[tempLeft++];			
        }
        else
        {
            arr[curIndex++] = tempArr[tempRight++];			
        }		
    }
    //왼쪽 절반에 남은 원소들을 원래 배열에 복사
    int remain = mid - tempLeft;
    for (int i = 0; i <= remain; i++)
    {
        arr[curIndex + i] = tempArr[tempLeft + i];
    }
}
void Partition( int left, int right)
{
    if (left < right)
    {
        int mid = (left + right) / 2;
        Partition(left, mid);
        Partition(mid + 1, right);
        Merge(left, right);
    }
}
int main() {



    Partition(0, N - 1);

    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}





5. 힙 정렬

설명 : 트리 기반으로 최대 힙 트리or 최소 힙 트리를 구성해 정렬을 하는 방법. 안정성 보장 X

내림차순 정렬을 위해서는 최대 힙을 구성하고 오름차순 정렬을 위해서는 최소 힙을 구성하면 된다. 





코드없음





6. 퀵 정렬(Quick Sort)(분할정복)

방법 :  데이터 집합내에 임의의 기준(pivot)값을 정하고 해당 피벗으로 집합을 기준으로 두개의 부분 집합으로 나눈다.

  한쪽 부분에는 피벗값보다 작은값들만, 다른 한쪽은 큰값들만 넣는다. 안정성 보장 X

더 이상 쪼갤 부분 집합이 없을 때까지 각각의 부분 집합에 대해 피벗/쪼개기 재귀적으로 적용.






#include<iostream>
using namespace std;
#define ARRNUM 8
int N = ARRNUM;
int arr[] = { 8,15,5,9,3,12,1,6};
void Swap(int& A, int& B)
{
    int Temp = A;
    A = B;
    B = Temp;
}


int Partition( int left, int right)
{
    int pivot = arr[right]; //맨 오른쪽을 피봇 선정
    int i = (left - 1);

    for (int j = left; j <= right-1; j++)
    {
        if (arr[j] <= pivot) //배열 순회하며 피봇이랑 같거나 작은 값 탐색
        {					
            i++;    //i 인덱스 위치와 교체 
            Swap(arr[i], arr[j]);
        }
    }

    //다 찾고 맨오른쪽에 있던 피봇값과 교체
    Swap(arr[i + 1], arr[right]);

    return (i + 1); // 리턴값 기준으로 왼쪽은 리턴인덱스보다 작고 오른쪽은 큰값들

}

void Quick(int L, int R)
{
    if (L < R)
    {
        int p = Partition(L, R); //한번 피봇으로 선정된 값 기준으로 

        Quick(L, p - 1); //피봇 기준 왼쪽 다시 정렬
        Quick(p + 1, R); //피봇 기준 오른쪽 다시 정렬
    }
}
int main() {

    Quick(0, N - 1);
    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}

6-2





#include<iostream>
using namespace std;
#define ARRNUM 8
int N = ARRNUM;
int arr[] = { 2,15,5,9,3,12,20,6 };
void Swap(int& A, int& B)
{
    int Temp = A;
    A = B;
    B = Temp;
}

void QuickSort(int left, int right)
{
    int pivot = arr[(left+right)/2]; //피봇 중심 선정
    int startIndex = left; 
    int endIndex = right;

    while (startIndex <= endIndex) //startIndex가 endIndex보다 높아질떄까지 while
    {
        while (arr[startIndex] < pivot) //피벗보다 왼쪽에서 피벗보다 큰값 찾기
        {
            ++startIndex;
        }
        while (arr[endIndex] > pivot) //피벗보다 오른쪽에서 피벗보다 작은값 찾기
        {
            --endIndex;
        }

        if (startIndex <= endIndex) //그렇게 찾아진 왼쪽 오른쪽 값을 서로 swap
        {
            Swap(arr[startIndex], arr[endIndex]);
            ++startIndex;
            --endIndex;
        }
    }

    if (left < endIndex) //피벗기준 왼쪽 smaller들 정렬
    {
        QuickSort(left, endIndex);
    } 
    if (startIndex < right)//피벗기준 오른쪽 bigger들 정렬
    {
        QuickSort(startIndex, right);
    }
}


int main() {

    QuickSort(0, N - 1);
    for (int i = 0; i < N; i++)
    {
        cout << arr[i] << endl;
    }

    return 0;
}




7. 기수 정렬(Radix Sort)

방법 : 낮은 자리수부터 비교해가며 정렬한다. 비교연산을 하지 않아 빠르지만, 또 다른 메모리 공간을 필요하다는게 단점.기수정렬은 낮은 자리수부터 비교하여 정렬해 간다는 것을 기본 개념으로 하는 정렬 알고리즘입니다. 기수정렬은 비교 연산을 하지 않으며 정렬 속도가 빠르지만 데이터 전체 크기에 기수 테이블의 크기만한 메모리가 더 필요합니다.


void Radix_Sort()
{
    int Radix = 1;
    while(true){
        if(Radix >= n)
        {
            break;
        }
        Radix = Radix *10;
    }

    for(int i = 1; i < Radix; i = i *10)
    {
        for(int j = 0; j < n; j++)
        {
            int k;
            if (arr[j] < i)
            {
                k = 0;
            }
            else
            {
                k = (arr[j] / i) % 10;
                Q[k].push(arr[j]);
                Q[j].pop();
                Idx++;
            }
        }
    }
}

- 빅오 표기(시간복잡도)


d는 자리수



 */

