using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class QuizQuestion
{
    public string questionText;
    public List<string> answers;
    public int correctAnswerIndex;
}

[System.Serializable]
public class QuizData
{
    public List<QuizQuestion> questions;
}

