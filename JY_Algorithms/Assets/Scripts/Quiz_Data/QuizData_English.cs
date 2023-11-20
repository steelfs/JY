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
    public int Index { get; set; } // CellVisualizer �� ã�ƿö� ����� �ε���

    public Quiz(string question, string[] options)
    {
        this.Question = question;
        this.Options = options; 
        this.correctAnswer = Options[0];
        ShuffleOptions();
    }

    public void ShuffleOptions()
    {
        // �ɼ��� �����ϰ� ���´�
        for (int i = 0; i < Options.Length; i++)
        {
            int j = UnityEngine.Random.Range(i, Options.Length);
            string temp = Options[i];
            Options[i] = Options[j];
            Options[j] = temp;
        }

        // ���ο� ������ ��ġ�� ã�´�
        this.CorrectAnswer_Index = Array.IndexOf(Options, correctAnswer) + 1;//correctAnswer�� ���� ���� ����ִ� �ε����� 1���ؼ� �������� ����
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
        quizData = new Dictionary<int, Quiz>//������ 1���� ��ġ
        {
            {0, new Quiz("'a' �� ������ ���� �˸��� ����?", new string[] { "�ϳ���", "�ΰ��� ", "�ι���", "~�� �̻���"})},
            {1, new Quiz("'about' �� ������ ���� �˸��� ����?", new string[] { "~�� ���Ͽ�", "~�� ���� ", "~ó��", "~ ������"})},
            {2, new Quiz("'above' �� ������ ���� �˸��� ����?", new string[] { "~����", "~ó�� ", "~����", "��¼��"})},
            {3, new Quiz("'academy' �� ������ ���� �˸��� ����?", new string[] { "�м���, �б�", "���� ", "ü����", "��"})},
            {4, new Quiz("'accent' �� ������ ���� �˸��� ����?", new string[] { "���, �׼�Ʈ ", "�＼��, �Ⱑ ��", "���, ���", "����"})},
            {5, new Quiz("'accident' �� ������ ���� �˸��� ����?", new string[] { "���, �쿬", "~ó��", "�ʿ�������", "�Ҽ�����"})},
            {6, new Quiz("'across' �� ������ ���� �˸��� ����?", new string[] { "�ǳ���, �������� ", "�˸��� ", "Ʋ����", "����"})},
            {7, new Quiz("'act' �� ������ ���� �˸��� ����?", new string[] { "�ൿ�ϴ�, �����ϴ�", "�뷡�ϴ�", "�����", "���ߴ�, �����ϴ�"})},
            {8, new Quiz("'add' �� ������ ���� �˸��� ����?", new string[] { "�߰��ϴ�", "���� ", "������", "���ϴ�"})},
            {9, new Quiz("'address' �� ������ ���� �˸��� ����?", new string[] { "�ּ�", "���б� ", "����", "ȸ��"})},

        };
    }
    //public readonly string[] QuizArr =
    //{
    //    "'a' �� ������ ���� �˸��� ����?\n1) �ϳ��� \n2) �ΰ���\n3) �ι��� \n4) ~�� �̻���\"",
    //    "'about' �� ������ ���� �˸��� ����?\n1) ~�� ���Ͽ� \n2) ~�� ����\n3) ~ó��\n4) ~ ������",
    //    "'above' �� ������ ���� �˸��� ����?\n1) ~���� \n2) ~ó��\n3) ~����\n4) ��¼��",
    //    "'academy' �� ������ ���� �˸��� ����?\n1) �м���, �б� \n2) ����\n3) ü����\n4) ��",
    //    "'accent' �� ������ ���� �˸��� ����?\n1) ���, �׼�Ʈ \n2) �＼��, �Ⱑ ��\n3) ���, ���\n4) ����",
    //    "'accident' �� ������ ���� �˸��� ����?\n1) ���, �쿬\n2) ~ó��\n3) �ʿ�������\n4) �Ҽ�����",
    //    "'across' �� ������ ���� �˸��� ����?\n1) �ǳ���, �������� \n2) �˸���\n3) Ʋ����\n4) ����",
    //    "'act' �� ������ ���� �˸��� ����?\n1) �ൿ�ϴ�, �����ϴ� \n2) �뷡�ϴ�\n3) �����\n4) ���ߴ�, �����ϴ�",
    //    "'add' �� ������ ���� �˸��� ����?\n1) �߰��ϴ� \n2) ����\n3) ������\n4) ���ϴ�",
    //    "'address' �� ������ ���� �˸��� ����?\n1) �ּ�\n2) ���б�\n3) ����\n4) ȸ��",

    //};
}
