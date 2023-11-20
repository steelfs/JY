using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Quiz
{
    public string Question { get; private set; }
    public int CorrectAnswer { get; private set; }
    public string[] Options { get; private set; }

    public Quiz(string question, string[] options)
    {
        this.Question = question;
        this.Options = ShuffleOptions(options, out int newCorrectIndex);
        this.CorrectAnswer = newCorrectIndex + 1;
    }

    private string[] ShuffleOptions(string[] options, out int newCorrectIndex)
    {
        string correctAnswer = options[0];
        // 옵션을 랜덤하게 섞는다
        for (int i = 0; i < options.Length; i++)
        {
            int j = UnityEngine.Random.Range(i, options.Length);
            string temp = options[i];
            options[i] = options[j];
            options[j] = temp;
        }

        // 새로운 정답의 위치를 찾는다
        newCorrectIndex = Array.IndexOf(options, correctAnswer);
        return options;
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
        for (int i = 1; i < GameManager.Visualizer.CellVisualizers.Length + 1; i++)
        {
            index = i % 10;
            quizzes[i] = quizData[shuffledKeys[index]];
        }
    }
    public QuizData_English()
    {
        quizData = new Dictionary<int, Quiz>//정답을 1번에 배치
        {
            {1, new Quiz("'a' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 하나의", "2) 두개의 ", "3) 두배의", "4) ~개 이상의"})},
            {2, new Quiz("'about' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) ~에 대하여", "2) ~와 같이 ", "3) ~처럼", "4) ~ 때문에"})},
            {3, new Quiz("'above' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) ~위에", "2) ~처럼 ", "3) ~같이", "4) 어쩌면"})},
            {4, new Quiz("'academy' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 학술원, 학교", "2) 병원 ", "3) 체육관", "4) 집"})},
            {5, new Quiz("'accent' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 억양, 액센트 ", "2) 억세다, 기가 센", "3) 사건, 사고", "4) 과거"})},
            {6, new Quiz("'accident' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 사고, 우연", "2)  ~처럼", "3) 필연적으로", "4) 할수없이"})},
            {7, new Quiz("'across' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 건너편에, 가로질러 ", "2) 알맞은 ", "3) 틀리다", "4) 늙은"})},
            {8, new Quiz("'act' 의 뜻으로 가장 알맞은 것은?", new string[] { "1)  행동하다, 연기하다", "2) 노래하다", "3) 가까운", "4) 멈추다, 정지하다"})},
            {9, new Quiz("'add' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 추가하다", "2) 빼다 ", "3) 나누다", "4) 곱하다"})},
            {10, new Quiz("'address' 의 뜻으로 가장 알맞은 것은?", new string[] { "1) 주소", "2) 대학교 ", "3) 병원", "4) 회사"})},

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
