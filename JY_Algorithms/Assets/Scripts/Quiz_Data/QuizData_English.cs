using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Quiz
{
    public string Question { get; set; }
    public int CorrectAnswer { get; set; }

    public Quiz(string question, int correctAnswer)
    {
        Question = question;
        CorrectAnswer = correctAnswer;
    }
}



public class QuizData_English 
{
    Dictionary<int, Quiz> quizzes;
    public Dictionary<int, Quiz> Quizzes => quizzes;
    public void InitQuizData()
    {
        quizzes = quizzes ?? new Dictionary<int, Quiz>();
        quizzes.Clear();
        
        List<int> shuffledKeys = new List<int>(quizData.Keys);
        Util.ShuffleList(shuffledKeys);
        for (int i = 1; i < GameManager.Visualizer.CellVisualizers.Length + 1; i++)
        {
            quizzes[i] = quizData[shuffledKeys[i]];
        }
    }

    Dictionary<int, Quiz> quizData = new Dictionary<int, Quiz>
    {
        {1, new Quiz("What does 'a' mean?\n1) 하나의 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {2, new Quiz("What does 'about' mean?\n1) 대하여 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {3, new Quiz("What does 'above' mean?\n1) 위에 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {4, new Quiz("What does 'academy' mean?\n1) 학술원, 학교 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {5, new Quiz("What does 'accent' mean?\n1) 억양, 액센트 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {6, new Quiz("What does 'accident' mean?\n1) 사고 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {7, new Quiz("What does 'across' mean?\n1) 건너편에, 가로질러 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {8, new Quiz("What does 'act' mean?\n1) 행동하다, 연기하다 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {9, new Quiz("What does 'add' mean?\n1) 추가하다 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
        {10, new Quiz("What does 'address' mean?\n1) 주소 \n2) Incorrect option 1 \n3) Incorrect option 2 \n4) Incorrect option 3", 1)},
    };
    public readonly string[] QuizArr =
    {
        "What does 'a' mean?\n1) 하나의 \n2) 두개의\n3) 두배의 \n4) ~개 이상의\"",
    };
 

}
