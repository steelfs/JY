using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// NPC ��ȭ ����� ������ Ŭ����
/// 
/// �α� ��ɵ� �߰��� �ʿ��ϴ� 
/// ��ȭ������ �����ؼ� �ش� ��ȭ�����ȿ� ���븸 ���������ֵ��� ���� �������ʿ� 
/// UI ������ ���� ��ư�� ������ ��ȭ�鿡 ��ȭ�������� ������ȭ�� �Ҽ��ְ� �����ϰ� 
/// ��ģ ��ȭ�������������� �α� UI �� �����Ͽ� �����ֵ����Ѵ�.
/// 
/// </summary>
public class TalkData_Gyu : MonoBehaviour
{
    /// <summary>
    /// ����Ʈ ��ȭ����
    /// ���ٿ� ���� �迭 ����
    /// ���� �ۼ����������� ��
    /// </summary>
    int talkQuestArrayLength = 3;

    string filePath = string.Empty;
    /// <summary>
    /// ��ȭ ���� ����� �����̸�
    /// </summary>
    const string fileFirstName = "NPC_";
    const string fileType = ".txt";
    /// <summary>
    /// ������ ������ ���� ���ϴܾ� 
    /// ���Ͽ��� ���������� ���Ƿ� �����ϱ����� �ʿ��ϴ�.
    /// </summary>
    string lineCheckChar = "//";


    /// <summary>
    /// ���Ͽ��� �о�� ������ �����ص� ���߹迭
    /// �ؽ����̺��ٴ� ���߹迭�� ��ȿ�����ϰŰ��Ƽ� ����ߴ�. 
    /// </summary>
    string[][] talkData;

    private void Awake()
    {
        //StreamingAssets���� ����ϴ¹������ ���� 
        filePath = Path.Combine(Application.streamingAssetsPath, "TextFile/NPC_TalkData/"); // ��� ��� ����
        
        talkData = new string[Enum.GetValues(typeof(TalkType)).Length][]; //�̳�ũ�⸸ŭ �迭 ��Ƶΰ� 
        FileRead_And_SplitFileData(); //�ܺ� �����о ��ȭ��� �ҷ����� 
    }

    /// <summary>
    /// �ܺ� �ؽ�Ʈ ���� �о ��Ʈ�� �����ͷ� ����� 
    /// </summary>
    /// <param name="typeName">�о�� �����̸�(Ȯ��������)</param>
    /// <returns>�о�� ����</returns>
    private string File_IO_TalkData_Read(string typeName)
    {
        string fullFilePath = $"{filePath}{fileFirstName}{typeName}{fileType}";
        //string fullFilePath = $"{filePath}{fileFirstName}{typeName}{fileType}";
        //Debug.Log(fullFilePath);
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                Debug.Log("��������");
            }
            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath);
                Debug.Log("���� ����");

            }
            string result = File.ReadAllText(fullFilePath);
            return result;
            //Debug.Log($"���� �־� �о��� ������  = {readFile}");
        }
        catch (Exception e)
        {
           Debug.LogWarning($"TalkType �� {typeName} ���� �ش��ϴ� ��ȭTxt������ �����ϴ� \r\n������ �ʿ�����ġ :{fullFilePath} , {e.Message}");
           return null;
        }
    }



    /// <summary>
    /// ���ڿ� Ư�� �������� �߶� �����ϱ� 
    /// </summary>
    /// <param name="fileReadData">�����о�� ���ڿ�</param>
    /// <returns></returns>
    private void FileRead_And_SplitFileData()
    {
        Array enumArray = Enum.GetValues(typeof(TalkType));
        string temp;
        foreach (var enumValue in enumArray)
        {
            //�̳��߰��Ǹ� ���� ���� ������ ������Ѵ�.
            //__CommonAssets/TextFile/NPC_TalkData/NPC_�̳��̸�.txt ������ ���ܾ� 
            temp = File_IO_TalkData_Read(enumValue.ToString()); //�����о�ͼ� ��Ƶΰ� 
            if (temp != null) //�о�� ���� �����Ұ�� �����Ѵ�. 
            {
                talkData[(int)enumValue] = temp.Replace("\n", "").Split(lineCheckChar);
            } 
        }
    }

    /// <summary>
    /// ����� ��ȭ��� ��������
    /// </summary>
    /// <param name="type">��ȭ ����</param>
    /// <param name="talkIndex">��ȭ ����</param>
    /// <returns>��ȭ ������ ���� ��ȭ��� ��ȯ</returns>
    public string[] GetTalk(TalkType type, int talkIndex)
    {
        string[] result = new string[talkQuestArrayLength];      // ��ȯ�� ��ȭ��� ũ�����ϰ�
        int startIndex = talkQuestArrayLength * talkIndex;       // talkIndex �������� ���۹迭 ��ġ���
        int endIndex = startIndex + talkQuestArrayLength;        // ������ġ�� ���ϰ� 
        int typeIndex = (int)type;
        endIndex = endIndex < talkData[typeIndex].Length ? endIndex : talkData[typeIndex].Length;
        int textIndex = 0;                                       // �迭 ���������δ������ �ӽú��������ϰ� 
        for (int i = startIndex; i < endIndex; i++)              // �������鼭 
        {
            result[textIndex] = talkData[typeIndex][i];          // �ӽú����� ��ȯ�� ��Ͽ� ���������� �迭��ġ ��ŭ �����´�
            textIndex++;                                         // ���������� �ֱ����� ����
        }
        if (textIndex > 0)
        {
            return result;
        }
        return null;
    }
    /// <summary>
    /// 0������ ���� ��ȭ �ε������� ��� ���ڿ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="type">��ȭ ����</param>
    /// <param name="talkIndex">��ȭ ����</param>
    /// <returns>��ȭ ������ ���� ��ȭ��� ��ȯ</returns>
    public string[] GetLog(TalkType type, int talkIndex)
    {
        int endIndex = talkQuestArrayLength * talkIndex + talkQuestArrayLength;        // ������ġ�� ���ϰ� 
        int typeIndex = (int)type;
        endIndex = endIndex  < talkData[typeIndex].Length ? endIndex : talkData[typeIndex].Length;    
        string[] result = new string[endIndex];      // ��ȯ�� ��ȭ��� ũ�����ϰ�
        int textIndex = 0;                                       // �迭 ���������δ������ �ӽú��������ϰ� 
        for (int i = 0; i < endIndex; i++)              // �������鼭 
        {
            result[textIndex] = talkData[typeIndex][i];          // �ӽú����� ��ȯ�� ��Ͽ� ���������� �迭��ġ ��ŭ �����´�
            textIndex++;                                         // ���������� �ֱ����� ����
        }
        if (textIndex > 0) 
        {
            return result;  
        }
        return null;
    }
}
