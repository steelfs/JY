using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Quiz
{
    public string Question { get; private set; }
    public int CorrectAnswer_Index { get; private set; }
    string correctAnswer;
    public string[] Options { get; private set; }
    public int Index { get; set; } // CellVisualizer 를 찾아올때 사용할 인덱스

    public Quiz(string question, string[] options)
    {
        this.Question = question;
        this.Options = options; 
        this.correctAnswer = Options[0];
        ShuffleOptions();
    }

    public void ShuffleOptions()
    {
        // 옵션을 랜덤하게 섞는다
        for (int i = 0; i < Options.Length; i++)
        {
            int j = UnityEngine.Random.Range(i, Options.Length);
            string temp = Options[i];
            Options[i] = Options[j];
            Options[j] = temp;
        }

        // 새로운 정답의 위치를 찾는다
        this.CorrectAnswer_Index = Array.IndexOf(Options, correctAnswer) + 1;//correctAnswer와 같은 값이 들어있는 인덱스에 1더해서 정답으로 설정
    }
}
public class QuizData_English 
{
    Dictionary<int, Quiz> quizzes;
    public Dictionary<int, Quiz> Quizzes => quizzes;
    Dictionary<int, Quiz> quizData;
    public void InitQuizData()
    {
        quizzes = quizzes ?? new Dictionary<int, Quiz>();
        quizzes.Clear();
        
        List<int> shuffledKeys = new List<int>(quizData.Keys);
        Util.ShuffleList(shuffledKeys);
        int index = 0;
        for (int i = 0; i < GameManager.Visualizer.CellVisualizers.Length ; i++)
        {
            index = i % 10;
            Quiz quiz = quizData[shuffledKeys[index]];
            quizzes[i] = quiz;
            quiz.Index = i;
        }
    }
    public QuizData_English()
    {
        quizData = new Dictionary<int, Quiz>//정답을 1번에 배치
        {
            {0, new Quiz("'a' 의 뜻으로 가장 알맞은 것은?", new string[] { "하나의", "두개의 ", "두배의", "~개 이상의"})},
            {1, new Quiz("'about' 의 뜻으로 가장 알맞은 것은?", new string[] { "~에 대하여", "~와 같이 ", "~처럼", "~ 때문에"})},
            {2, new Quiz("'above' 의 뜻으로 가장 알맞은 것은?", new string[] { "~위에", "~처럼 ", "~같이", "어쩌면"})},
            {3, new Quiz("'academy' 의 뜻으로 가장 알맞은 것은?", new string[] { "학술원, 학교", "병원 ", "체육관", "집"})},
            {4, new Quiz("'accent' 의 뜻으로 가장 알맞은 것은?", new string[] { "억양, 액센트 ", "억세다, 기가 센", "사건, 사고", "과거"})},
            {5, new Quiz("'accident' 의 뜻으로 가장 알맞은 것은?", new string[] { "사고, 우연", "~처럼", "필연적으로", "할수없이"})},
            {6, new Quiz("'across' 의 뜻으로 가장 알맞은 것은?", new string[] { "건너편에, 가로질러 ", "알맞은 ", "틀리다", "늙은"})},
            {7, new Quiz("'act' 의 뜻으로 가장 알맞은 것은?", new string[] { "행동하다, 연기하다", "노래하다", "가까운", "멈추다, 정지하다"})},
            {8, new Quiz("'add' 의 뜻으로 가장 알맞은 것은?", new string[] { "추가하다", "빼다 ", "나누다", "곱하다"})},
            {9, new Quiz("'address' 의 뜻으로 가장 알맞은 것은?", new string[] { "주소", "대학교 ", "병원", "회사"})},

        };
    }
    //public readonly string[] QuizArr =
    //{
    //    "'a' 의 뜻으로 가장 알맞은 것은?\n1) 하나의 \n2) 두개의\n3) 두배의 \n4) ~개 이상의\"",
    //    "'about' 의 뜻으로 가장 알맞은 것은?\n1) ~에 대하여 \n2) ~와 같이\n3) ~처럼\n4) ~ 때문에",
    //    "'above' 의 뜻으로 가장 알맞은 것은?\n1) ~위에 \n2) ~처럼\n3) ~같이\n4) 어쩌면",
    //    "'academy' 의 뜻으로 가장 알맞은 것은?\n1) 학술원, 학교 \n2) 병원\n3) 체육관\n4) 집",
    //    "'accent' 의 뜻으로 가장 알맞은 것은?\n1) 억양, 액센트 \n2) 억세다, 기가 센\n3) 사건, 사고\n4) 과거",
    //    "'accident' 의 뜻으로 가장 알맞은 것은?\n1) 사고, 우연\n2) ~처럼\n3) 필연적으로\n4) 할수없이",
    //    "'across' 의 뜻으로 가장 알맞은 것은?\n1) 건너편에, 가로질러 \n2) 알맞은\n3) 틀리다\n4) 늙은",
    //    "'act' 의 뜻으로 가장 알맞은 것은?\n1) 행동하다, 연기하다 \n2) 노래하다\n3) 가까운\n4) 멈추다, 정지하다",
    //    "'add' 의 뜻으로 가장 알맞은 것은?\n1) 추가하다 \n2) 빼다\n3) 나누다\n4) 곱하다",
    //    "'address' 의 뜻으로 가장 알맞은 것은?\n1) 주소\n2) 대학교\n3) 병원\n4) 회사",

    //};
}
