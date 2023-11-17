using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dictionary_DataManager : Singleton<Dictionary_DataManager>
{
    ReadOnlyDictionary<string, string> Dictionary_Eng_Kor;
    Dictionary<string, string> dictionary_Eng_Kor;
    public Dictionary_DataManager()
    {
        Init_Dictionary();
    }
    void Init_Dictionary()
    {
        dictionary_Eng_Kor = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Apple", "���" },
            { "Human", "�ΰ�" },
            { "Baby", "�Ʊ�"},
            { "Bread", "��"},
            { "Tear", "����"},
            { "Hobby", "���"},
            { "rice",  "��" },
            { "finger", "�հ���"},
            { "Aalborg", "����ũ �Ϻ��� ����" },
            { "Aalesund", "�븣������ ����" },
            { "aalii", "�Ͽ����� ����" },
            { "aaliis", "aalii�� ������" },
            { "aals", "���� ����" },
            { "Aalst", "���⿡�� ����" },
            { "Aalto", "�˹ٸ� �ƾ���, �ɶ����� ���డ" },
            { "AAM", "����� �̻���" },
            { "AAMSI", "���� ����" },
            { "Aandahl", "��(��)" },
            { "Aani", "���� ����" },
            { "Aaqbiye", "���� ����" },
            { "Aar", "�������� ��" },
            { "Aara", "��(��) �Ǵ� �̸�" },
            { "Aarau", "�������� ����" },
            { "AARC", "���� ����" },
            { "aardvark", "������" },
            { "aardvarks", "aardvark�� ������" },
            { "aardwolf", "������ī�� ������" },
            { "aardwolves", "aardwolf�� ������" },
            { "Aaren", "�̸�" },
            { "Aargau", "�������� ��(�)" },
            { "aargh", "��ź��" },
            { "Aarhus", "����ũ�� ����" },
            { "Aarika", "�̸�" },
            { "Aaron", "�̸�" },
            { "Aaronic", "�Ʒа� ���õ�" },
            { "Aaronical", "Aaronic�� �ٸ� ����" },
            { "Aaronite", "�Ʒ��� ������" },
            { "Aaronitic", "Aaronite�� �ٸ� ����" },
            { "Aaron's-beard", "�Ĺ��� ����" },
            { "Aaronsburg", "�̱� ��Ǻ��Ͼ��� ����" },
            { "Aaronson", "��(��)" },
            { "AARP", "�̱��� �ôϾ� ��ü" },
            { "aarrgh", "��ź��" },
            { "aarrghh", "aarrgh�� ������" },
            { "Aaru", "��� ����Ʈ ��ȭ�� ����" },
            { "AAS", "���� ��� �б���" },
            { "A'asia", "�̸�" },
            { "aasvogel", "������ī�� ������" },
            { "aasvogels", "aasvogel�� ������" },
            { "A-axes", "������ ���" },
            { "A-axis", "A-axes�� �ܼ���" },
            { "AB", "������ �׷� �� �ϳ�" },
            { "ab-", "���λ� ('������', '�����ϴ�')" },
            { "ABA", "�̱� ��ȣ�� ��ȸ" },
            { "Ababa", "�Ƶ𽺾ƹٹ�, ��Ƽ���Ǿ��� ����" },
            { "Ababdeh", "����Ʈ�� �������" },
            { "Ababua", "���� ����" },
            { "abac", "��� ����" }
        };
        Dictionary_Eng_Kor = new ReadOnlyDictionary<string, string>(dictionary_Eng_Kor);
    }
    public bool ContainsWord(string word, out string value)
    {
        if (Dictionary_Eng_Kor.ContainsKey(word))
        {
            value = Dictionary_Eng_Kor[word];
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }


}
