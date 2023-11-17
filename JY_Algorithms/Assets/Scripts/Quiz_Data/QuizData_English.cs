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
    Dictionary<int, Quiz> quizzes_;
    public Dictionary<int, Quiz> Quizzes => quizzes_;
    HashSet<int> usedIndexes = new HashSet<int>();
    public void InitQuizData()
    {
        quizzes_.Clear();
        while(quizzes.Count > quizzes_.Count)
        {
            int random = Random.Range(1, quizzes.Count + 1);
            if (usedIndexes.Add(random))
            {
                quizzes_.Add(quizzes_.Count + 1, quizzes[random]);
            }
        }
    }

    Dictionary<int, Quiz> quizzes = new Dictionary<int, Quiz>
    {
        {1, new Quiz("What does 'dog' mean? ('dog'�� ���� �����ΰ���?)\n\n1) ����� (Cat)\n2) �� (Dog)\n3) �� (Bird)\n4) �� (Horse)", 2)},
        {2, new Quiz("Choose the translation for 'apple'. ('apple'�� ������ ������.)\n\n1) ��� (Apple)\n2) �ٳ��� (Banana)\n3) ���� (Grape)\n4) ���� (Strawberry)", 1)},
        {3, new Quiz("What is the Korean word for 'water'? ('water'�� �ѱ���� �����ΰ���?)\n\n1) �� (Water)\n2) �ٶ� (Wind)\n3) �� (Fire)\n4) �� (Earth)", 1)},
        {4, new Quiz("Translate 'sun' into Korean. ('sun'�� �ѱ���� �����ϼ���.)\n\n1) �� (Moon)\n2) �� (Star)\n3) �¾� (Sun)\n4) ���� (Cloud)", 3)},
        {5, new Quiz("How do you say 'thank you' in English? ('thank you'�� ����� ��� ���ϳ���?)\n\n1) Please\n2) Sorry\n3) Hello\n4) Thank you", 4)},
        {6, new Quiz("Which word means 'school'? ('school'�� ���� �����ΰ���?)\n\n1) �б� (School)\n2) ���� (Hospital)\n3) ���� (Bank)\n4) ���۸��� (Supermarket)", 1)},
        {7, new Quiz("What is the English for '��'? ('��'�� ����� �����ΰ���?)\n\n1) Air\n2) Water\n3) Fire\n4) Earth", 2)},
        {8, new Quiz("What does 'happy' mean in Korean? ('happy'�� �ѱ��� ���� �����ΰ���?)\n\n1) ���� (Sad)\n2) ��� (Happy)\n3) ȭ�� (Angry)\n4) ������ (Bored)", 2)},
        {9, new Quiz("Which word means 'car'? ('car'�� ���� �����ΰ���?)\n\n1) ���� (Bus)\n2) ������ (Bicycle)\n3) �� (Car)\n4) ���� (Train)", 3)},
        {10, new Quiz("How do you say 'book' in Korean? ('book'�� �ѱ���� ��� ���ϳ���?)\n\n1) å (Book)\n2) �� (Pen)\n3) ���� (Bag)\n4) ���찳 (Eraser)", 1)}
    };
}
