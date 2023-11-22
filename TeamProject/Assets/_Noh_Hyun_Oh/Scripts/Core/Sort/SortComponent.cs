using System.Collections.Generic;
using System.Linq;
/*   ���ı�ɸ� �����Ұ��̱⶧����static ���� ���� ����� �Ҽ�����.    
     enum �� ����̱⶧���� �������̽����� ����Ҽ����� ���׸����� �������������.
     �׷��ٰ� ���׸��� Ŭ�����ι����� ��Ӷ����� �������߻��Ѵ�.
     ChangeObj(ref listData[min],ref listData[i]); CS0206 �����߻� ������Ƽ�� �迭�̳� ����Ʈ�� �ε����� ref �Ǵ� out �� ����Ҽ������ٰ��Ѵ�.
     �迭, ����Ʈ �ε��� ���� �Լ��� ���ִ°Ű���.
*/
/// <summary>
///     
///     Ŭ���� ���� ��(�ɹ����� or ������Ƽ)�� �������� ��ü �����ϰ������ ����� Ŭ�����̴�
///     ����� ������ ���۳�Ʈ�� ISortBase �� ��ӹ޾Ƽ� ����������Ƽ�� �ۼ��Ѵ�.
///     T�� ���۳�Ʈ �� �Ű������δ� T�� �迭�� �ѱ�� 
///     SortProccessType �� ���Ĺ���̶� �Է¾��ص� ������� �⺻�� �������ķ� ����.
///     SortType �� ���������� �⺻���ε��ִ� 
///     SortComponent<T>.SorttingData(T[] �Ǵ� List<T>,SortComponent<T>.SortType,SortComponent<T>.SortProccessType); 
///     
///     enum SortType �� �߰��Ѱ����� �׽�Ʈ�Ϸ�
/// </summary>
/// <typeparam name="T">ISortBase �������̽��� ��ӹ��� Ŭ����</typeparam>

public static class SortComponent<T> where T : ITurnBaseData 
{
    /// <summary>
    /// ���� �˰����� ����
    /// ������ ���ʿ��ϴ�.
    /// </summary>
    public enum SortProccessType
    {
        InsertSort = 0, //���� �����ϱ� 
        SelectionSort,  //���� �����ϱ�
        BubbleSort,     //���� �����ϱ�
    }


   
    /// <summary>
    /// IEnumerable �� ���Ⱑ�ȵǼ� ICollection �� ����غ��Ѵ�.
    /// ICollection�� �������̽� ��ü�� �������� �����Ͽ� Array �� List �� ����� �������̽��� IList �� �̿��Ͽ���. ���п� �����ε� ���� ����Ҽ��ִ�.
    /// ���� �˰����� �������� �������ִ� �Լ�
    /// �˰��� ��ó : https://hyo-ue4study.tistory.com/68
    ///<param name="data">������ ������</param>
    ///<param name="proccessType">������ �˰���Ÿ��</param>
    ///<param name="type">���Ĺ��</param>
    /// </summary>
    public static void SorttingData(IList<T>  data,                                            //����Ʈ�� �迭�̵� �������ִ� �������̽��� ���ڰ����� �޾Ƽ� ó���غ��Ҵ�. 
                                    SortProccessType proccessType = SortProccessType.InsertSort,    //���ľ˰����� �������� �����ϱ����� �־����.
                                    SortType type = SortType.Ascending                             //�������� �������� �����ϱ����� ���̴�.
                                    )
    {
        int length = data.Count(); //�ǹ̾��� �̸������ͻ�

        switch (proccessType) // ���Ĺ������
        {
            case SortProccessType.InsertSort: //��������
                InsertionSort(ref data , length, type); 
                break;
            case SortProccessType.SelectionSort: //��������
                SelectionSort(ref data , length, type);
                break;
            case SortProccessType.BubbleSort: //��������
                BubbleSort(ref data , length, type);
                break;
        }

    }

    /// <summary>
    /// �������� �˰����� �����ͼ� ����ߴ�\
    /// ���������� ���� ���� �������� ����(�ε����� �������� ����)���� ���Ľ�Ű�°��̴�.
    /// ���ʰ� �񱳸��ϸ鼭 �ϳ��� �����ϱ⶧���� j������ Ƚ���� �ڷΰ����� Ŀ����.
    /// ���Ҵ���̾������ break; �� ���������⶧���� ���ʿ��� �񱳴� �ִ������� ���δ�..
    /// </summary>
    /// <param name="listData">�迭���</param>
    /// <param name="length">�迭�� ũ��</param>
    /// <param name="type">��������</param>
    private static void InsertionSort(ref IList<T> listData, int length, SortType type)
    {

        T key; //������ ��ü
        for (int i = 1; i < length; i++) //1��°���� �迭�� ũ�⸸ŭ ������ ���� 
        {
            int j = 0; // j ���� i �κп��� ����ؾ������� ���⿡ ����
            key = listData[i];// 0���̾ƴ϶� 1������ ���� ��ü�� ��� 
            for (j = i - 1; j >= 0; j--) //���� ���� -1��°���� �񱳸� �����Ͽ� 
            {
                if (SortAscDesCheck(key, listData[j], type)) //���������϶�  key ������ ū���� �����Ұ��  ���������϶� key������ �������� �����Ұ��
                {
                    listData[j + 1] = listData[j];  // ��ĭ�� ��(+)�� �̵���Ų��.
                }
                else //�̹����ĵȰ��߿� ���� ��ü���� ������ ��ġ�� ���̻� �̵����ʿ䰡���⶧���� j������ ����������.
                {
                    break;
                }
            }
            listData[j + 1] = key; //  j������ ��ġ���� �����ͼ� ���������Ѱ��� �ٷεڿ� ���Ұ�ü�� �����͸� ��Ƽ� i������ �ϳ���������������. 
        }

    }
    /// <summary>
    /// �������� �� ���������� ������ üũ���ִ� �Լ�
    /// </summary>
    /// <param name="min">�۾ƾߵɰ�</param>
    /// <param name="max">Ŀ�ߵ� ��</param>
    /// <param name="sortType">�������� �����������ð�</param>
    /// <returns></returns>
    public static bool SortAscDesCheck(ITurnBaseData min, ITurnBaseData max, SortType sortType)
    {
        bool resultValue = false;
        switch (sortType)
        {
            case SortType.Ascending:
                resultValue = min.TurnActionValue < max.TurnActionValue; // �ø������ϰ�� -100 ~ 100
                break;
            case SortType.Descending:
                resultValue = min.TurnActionValue > max.TurnActionValue; // ���������ϰ�� 100 ~ -100
                break;
        }
        return resultValue;
    }


    /// <summary>
    /// �������� �������İ��� �ݴ�� +�������� �񱳸��ϱ⶧���� 
    /// ó�� ���Ҷ� j������ Ƚ���� ������.
    /// </summary>
    /// <param name="arrayData">�迭���</param>
    /// <param name="length">�迭�� ũ��</param>
    private static void SelectionSort(ref IList<T> listData, int length, SortType type)
    {
        for (int i = 0; i < length - 1; i++) // �迭ũ�⸸ŭ ���������� ������
        {
            int min = i; //�ּҰ��� ã������ ��ġ���� �����Ѵ�
            for (int j = i + 1; j < length; j++) //-������ �ּҰ������̳������̴� +������ Ž���� �����Ѵ�.
            {
                if (SortAscDesCheck(listData[j], listData[min], type))// min���� ���ؼ� min���� �������� �ִ°�� 
                {
                    min = j;//�ּҰ���ġ�� �����Ѵ�.
                }//�ݺ��Ͽ� ������ü�� �ּҰ���ġ�� ã�´�.
            }
            if (i != min)
            {
                //�ּҰ��� ���� ���� ������������ ��ü�Ѵ�.
                //�Լ��ξȻ������� �Լ��λ��¼��� T[i] ���� �Ű������� �ѱ涧 ����Ÿ���� �ƴ϶� ��Ÿ������ ġȯ�� �Ǽ� �Ѿ�⶧���̴�.�����¸𸣰ٴ�.
                //������ C# ������ ������Ƽ�� �迭�̳� ����Ʈ�� �ε����� �Ű��γѱ涧 ��Ÿ������ �ڵ�ġȯ�� �ȴٰ��Ѵ�. 
                // ref �� out �� ���������°� ������Ƽ�� �ε����� ����ȵȴ�. ������ �ƴ϶� �Լ���.
                (listData[min], listData[i]) = (listData[i], listData[min]);//Ʃ���̶�°� �ΰ��� ��ȯ���ش� ���ٽĺ���ѰͰ���.
            }
        }
    }
    /// <summary>
    /// �������� ������ �ΰ�ü�� ���Ѵ�.
    /// </summary>
    /// <param name="arrayData">�迭 ���</param>
    /// <param name="length">�迭 ũ��</param>
    private static void BubbleSort(ref IList<T> arrayData, int length, SortType type)
    {
        int i = 0;
        int j = 0;

        for (i = 0; i < length - 1; i++) //ù��°���� ���������ͱ��� �� 
        {
            for (j = 0; j < length - 1 - i; j++) // �ڷΰ����� ���� ��Ƚ���� �ٿ�����.
            {
                if (SortAscDesCheck(arrayData[j + 1], arrayData[j], type))//�ڿ� ���� ���簪�� ���ؼ� �ڿ����� ������ 
                {
                    //���簪�� �ڿ����� ��ü�Ѵ�. 
                    (arrayData[j + 1], arrayData[j]) = (arrayData[j], arrayData[j + 1]);//Ʃ���̶�°� �ΰ��� ��ȯ���ش� ���ٽĺ���ѰͰ���.
                }
            }
        }
    }
    //==============================================  �Ʒ��� ��ũ�� ����Ʈ ���� ��� ==========================================

    /// <summary>
    /// ��ũ�帮��Ʈ ����. �˰��� : ��������
    /// ���������� ������ �ΰ�ü�� ���ϸ鼭 ��ü�ϱ⶧�� ��ũ�� ����Ʈ�� Ư������ �߸´´� 
    /// �׽�Ʈ �Ϸ� 
    /// </summary>
    /// <param name="turnObjectList">�����ҵ����� ISortBase ��ũ�帮��Ʈ</param>
    /// <param name="type">��������Asc ��������Des</param>
    public static void BubbleSortLinkedList(LinkedList<ITurnBaseData> turnObjectList, SortType type = SortType.Ascending)
    {
        LinkedListNode<ITurnBaseData> key;
        LinkedListNode<ITurnBaseData> temp;
        int length = turnObjectList.Count;
        for (int i = 0; i < length - 1; i++)
        {
            key = turnObjectList.First;//������ ó������ �˻�����
            for (int j = 0; j < length - 1 - i; j++)//��ó������ ������ ���鼭 ���� �ѹ����������� ����Ƚ���� ���� �پ���
            {
                if (SortAscDesCheck(key.Next.Value, key.Value, type)) //���� ���Ͽ� ��ü�ʿ��ϸ� 
                {   //��ü
                    temp = key.Next; //�ڿ��� �ӽð�ü�� ��Ƶΰ� 
                    turnObjectList.Remove(key);//�տ��ִ���ü ��ũ�帮��Ʈ���� ���� 
                    turnObjectList.AddAfter(temp, key);//�׷��� �ڿ��ִ� �ӽð�ü�� ���� �ڿ��ٰ� ��� �߰��Ͽ� �ڿ��ִ��Ͱ� �տ��ִ����� ��ġ�� �ٲ۴�.
                }
                else
                {
                    key = key.Next; //��ü�Ұ��̾����� ���� ��ü�� �񱳴���� �ű��.
                }

            }

        }

    }


}
/*


�Ʒ������� �ð����� �߰��ҿ��� 

4. ���� ����(Merge Sort)

���� : �� �̻��� �κ��������� ������, �� �κ������� ������ ���� �κ����յ��� �ٽ� ���ĵ� ���·� ��ġ�� ���. �������� �����Ѵ�.

- ���� ������ ���(Divide-And-Conquer)

���� : �ذ��ϰ��� �ϴ� ������ ���� ũ���� ������ ������� �����Ѵ�.
���� : ������ ���� ������ ��ȯ������ �ذ��Ѵ�.
�պ� : ���� ������ �ظ� ���Ͽ�(merge) ���� ������ ���� �ظ� ���Ѵ�.







#include<iostream>
using namespace std;
#define ARRNUM 5
int N = ARRNUM;
int arr[] = { 8,5,3,1,6 };
int tempArr[ARRNUM];

void Merge(int left, int right)
{
    //����¥�� arr�� tempArr�������Ѵ�.
    for (int i = left; i <= right; i++)
    {
        tempArr[i] = arr[i];
    }

    int mid = (left + right) / 2;

    int tempLeft = left;
    int tempRight = mid+1;
    int curIndex = left;

    //temparr�迭 ��Ⱥ��. ���� ���ݰ� ������ ���� ���ؼ�
    //�� ���� ���� ���� �迭�� ����
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
    //���� ���ݿ� ���� ���ҵ��� ���� �迭�� ����
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





5. �� ����

���� : Ʈ�� ������� �ִ� �� Ʈ��or �ּ� �� Ʈ���� ������ ������ �ϴ� ���. ������ ���� X

�������� ������ ���ؼ��� �ִ� ���� �����ϰ� �������� ������ ���ؼ��� �ּ� ���� �����ϸ� �ȴ�. 





�ڵ����





6. �� ����(Quick Sort)(��������)

��� :  ������ ���ճ��� ������ ����(pivot)���� ���ϰ� �ش� �ǹ����� ������ �������� �ΰ��� �κ� �������� ������.

  ���� �κп��� �ǹ������� �������鸸, �ٸ� ������ ū���鸸 �ִ´�. ������ ���� X

�� �̻� �ɰ� �κ� ������ ���� ������ ������ �κ� ���տ� ���� �ǹ�/�ɰ��� ��������� ����.






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
    int pivot = arr[right]; //�� �������� �Ǻ� ����
    int i = (left - 1);

    for (int j = left; j <= right-1; j++)
    {
        if (arr[j] <= pivot) //�迭 ��ȸ�ϸ� �Ǻ��̶� ���ų� ���� �� Ž��
        {					
            i++;    //i �ε��� ��ġ�� ��ü 
            Swap(arr[i], arr[j]);
        }
    }

    //�� ã�� �ǿ����ʿ� �ִ� �Ǻ����� ��ü
    Swap(arr[i + 1], arr[right]);

    return (i + 1); // ���ϰ� �������� ������ �����ε������� �۰� �������� ū����

}

void Quick(int L, int R)
{
    if (L < R)
    {
        int p = Partition(L, R); //�ѹ� �Ǻ����� ������ �� �������� 

        Quick(L, p - 1); //�Ǻ� ���� ���� �ٽ� ����
        Quick(p + 1, R); //�Ǻ� ���� ������ �ٽ� ����
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
    int pivot = arr[(left+right)/2]; //�Ǻ� �߽� ����
    int startIndex = left; 
    int endIndex = right;

    while (startIndex <= endIndex) //startIndex�� endIndex���� ������������ while
    {
        while (arr[startIndex] < pivot) //�ǹ����� ���ʿ��� �ǹ����� ū�� ã��
        {
            ++startIndex;
        }
        while (arr[endIndex] > pivot) //�ǹ����� �����ʿ��� �ǹ����� ������ ã��
        {
            --endIndex;
        }

        if (startIndex <= endIndex) //�׷��� ã���� ���� ������ ���� ���� swap
        {
            Swap(arr[startIndex], arr[endIndex]);
            ++startIndex;
            --endIndex;
        }
    }

    if (left < endIndex) //�ǹ����� ���� smaller�� ����
    {
        QuickSort(left, endIndex);
    } 
    if (startIndex < right)//�ǹ����� ������ bigger�� ����
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




7. ��� ����(Radix Sort)

��� : ���� �ڸ������� ���ذ��� �����Ѵ�. �񱳿����� ���� �ʾ� ��������, �� �ٸ� �޸� ������ �ʿ��ϴٴ°� ����.��������� ���� �ڸ������� ���Ͽ� ������ ���ٴ� ���� �⺻ �������� �ϴ� ���� �˰����Դϴ�. ��������� �� ������ ���� ������ ���� �ӵ��� �������� ������ ��ü ũ�⿡ ��� ���̺��� ũ�⸸�� �޸𸮰� �� �ʿ��մϴ�.


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

- ��� ǥ��(�ð����⵵)


d�� �ڸ���



 */

