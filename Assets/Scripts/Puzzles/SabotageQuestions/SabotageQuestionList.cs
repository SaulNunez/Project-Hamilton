using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Puzzle assets/Sabotage question list")]
public class SabotageQuestionList : ScriptableObject
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public List<string> answers;
        public int correctAnswerIndex;
    }

    public List<Question> questions;
}
