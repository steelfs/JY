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
            { "Apple", "사과" },
            { "Human", "인간" },
            { "Baby", "아기"},
            { "Bread", "빵"},
            { "Tear", "눈물"},
            { "Hobby", "취미"},
            { "rice",  "쌀" },
            { "finger", "손가락"},
            { "Aalborg", "덴마크 북부의 도시" },
            { "Aalesund", "노르웨이의 도시" },
            { "aalii", "하와이의 관목" },
            { "aaliis", "aalii의 복수형" },
            { "aals", "정보 부족" },
            { "Aalst", "벨기에의 도시" },
            { "Aalto", "알바르 아알토, 핀란드의 건축가" },
            { "AAM", "공대공 미사일" },
            { "AAMSI", "정보 부족" },
            { "Aandahl", "성(姓)" },
            { "Aani", "정보 부족" },
            { "Aaqbiye", "정보 부족" },
            { "Aar", "스위스의 강" },
            { "Aara", "성(姓) 또는 이름" },
            { "Aarau", "스위스의 도시" },
            { "AARC", "정보 부족" },
            { "aardvark", "땅돼지" },
            { "aardvarks", "aardvark의 복수형" },
            { "aardwolf", "아프리카의 포유류" },
            { "aardwolves", "aardwolf의 복수형" },
            { "Aaren", "이름" },
            { "Aargau", "스위스의 주(州)" },
            { "aargh", "감탄사" },
            { "Aarhus", "덴마크의 도시" },
            { "Aarika", "이름" },
            { "Aaron", "이름" },
            { "Aaronic", "아론과 관련된" },
            { "Aaronical", "Aaronic의 다른 형태" },
            { "Aaronite", "아론의 추종자" },
            { "Aaronitic", "Aaronite의 다른 형태" },
            { "Aaron's-beard", "식물의 일종" },
            { "Aaronsburg", "미국 펜실베니아의 마을" },
            { "Aaronson", "성(姓)" },
            { "AARP", "미국의 시니어 단체" },
            { "aarrgh", "감탄사" },
            { "aarrghh", "aarrgh의 강조형" },
            { "Aaru", "고대 이집트 신화의 낙원" },
            { "AAS", "원자 흡수 분광법" },
            { "A'asia", "이름" },
            { "aasvogel", "아프리카의 독수리" },
            { "aasvogels", "aasvogel의 복수형" },
            { "A-axes", "결정학 용어" },
            { "A-axis", "A-axes의 단수형" },
            { "AB", "혈액형 그룹 중 하나" },
            { "ab-", "접두사 ('떨어진', '부재하는')" },
            { "ABA", "미국 변호사 협회" },
            { "Ababa", "아디스아바바, 에티오피아의 수도" },
            { "Ababdeh", "이집트의 유목민족" },
            { "Ababua", "정보 부족" },
            { "abac", "계산 도구" }
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
