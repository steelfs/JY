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
        // �ɼ��� �����ϰ� ���´�
        for (int i = 0; i < options.Length; i++)
        {
            int j = UnityEngine.Random.Range(i, options.Length);
            string temp = options[i];
            options[i] = options[j];
            options[j] = temp;
        }

        // ���ο� ������ ��ġ�� ã�´�
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
        quizData = new Dictionary<int, Quiz>//������ 1���� ��ġ
        {
            {1, new Quiz("'a' �� ������ ���� �˸��� ����?", new string[] { "1) �ϳ���", "2) �ΰ��� ", "3) �ι���", "4) ~�� �̻���"})},
            {2, new Quiz("'about' �� ������ ���� �˸��� ����?", new string[] { "1) ~�� ���Ͽ�", "2) ~�� ���� ", "3) ~ó��", "4) ~ ������"})},
            {3, new Quiz("'above' �� ������ ���� �˸��� ����?", new string[] { "1) ~����", "2) ~ó�� ", "3) ~����", "4) ��¼��"})},
            {4, new Quiz("'academy' �� ������ ���� �˸��� ����?", new string[] { "1) �м���, �б�", "2) ���� ", "3) ü����", "4) ��"})},
            {5, new Quiz("'accent' �� ������ ���� �˸��� ����?", new string[] { "1) ���, �׼�Ʈ ", "2) �＼��, �Ⱑ ��", "3) ���, ���", "4) ����"})},
            {6, new Quiz("'accident' �� ������ ���� �˸��� ����?", new string[] { "1) ���, �쿬", "2)  ~ó��", "3) �ʿ�������", "4) �Ҽ�����"})},
            {7, new Quiz("'across' �� ������ ���� �˸��� ����?", new string[] { "1) �ǳ���, �������� ", "2) �˸��� ", "3) Ʋ����", "4) ����"})},
            {8, new Quiz("'act' �� ������ ���� �˸��� ����?", new string[] { "1)  �ൿ�ϴ�, �����ϴ�", "2) �뷡�ϴ�", "3) �����", "4) ���ߴ�, �����ϴ�"})},
            {9, new Quiz("'add' �� ������ ���� �˸��� ����?", new string[] { "1) �߰��ϴ�", "2) ���� ", "3) ������", "4) ���ϴ�"})},
            {10, new Quiz("'address' �� ������ ���� �˸��� ����?", new string[] { "1) �ּ�", "2) ���б� ", "3) ����", "4) ȸ��"})},

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
