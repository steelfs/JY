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
        {1, new Quiz("What does 'dog' mean? ('dog'의 뜻은 무엇인가요?)\n\n1) 고양이 (Cat)\n2) 개 (Dog)\n3) 새 (Bird)\n4) 말 (Horse)", 2)},
        {2, new Quiz("Choose the translation for 'apple'. ('apple'의 번역을 고르세요.)\n\n1) 사과 (Apple)\n2) 바나나 (Banana)\n3) 포도 (Grape)\n4) 딸기 (Strawberry)", 1)},
        {3, new Quiz("What is the Korean word for 'water'? ('water'의 한국어는 무엇인가요?)\n\n1) 물 (Water)\n2) 바람 (Wind)\n3) 불 (Fire)\n4) 흙 (Earth)", 1)},
        {4, new Quiz("Translate 'sun' into Korean. ('sun'을 한국어로 번역하세요.)\n\n1) 달 (Moon)\n2) 별 (Star)\n3) 태양 (Sun)\n4) 구름 (Cloud)", 3)},
        {5, new Quiz("How do you say 'thank you' in English? ('thank you'를 영어로 어떻게 말하나요?)\n\n1) Please\n2) Sorry\n3) Hello\n4) Thank you", 4)},
        {6, new Quiz("Which word means 'school'? ('school'의 뜻은 무엇인가요?)\n\n1) 학교 (School)\n2) 병원 (Hospital)\n3) 은행 (Bank)\n4) 슈퍼마켓 (Supermarket)", 1)},
        {7, new Quiz("What is the English for '물'? ('물'의 영어는 무엇인가요?)\n\n1) Air\n2) Water\n3) Fire\n4) Earth", 2)},
        {8, new Quiz("What does 'happy' mean in Korean? ('happy'의 한국어 뜻은 무엇인가요?)\n\n1) 슬픈 (Sad)\n2) 기쁜 (Happy)\n3) 화난 (Angry)\n4) 지루한 (Bored)", 2)},
        {9, new Quiz("Which word means 'car'? ('car'의 뜻은 무엇인가요?)\n\n1) 버스 (Bus)\n2) 자전거 (Bicycle)\n3) 차 (Car)\n4) 기차 (Train)", 3)},
        {10, new Quiz("How do you say 'book' in Korean? ('book'을 한국어로 어떻게 말하나요?)\n\n1) 책 (Book)\n2) 펜 (Pen)\n3) 가방 (Bag)\n4) 지우개 (Eraser)", 1)}
    };
}
