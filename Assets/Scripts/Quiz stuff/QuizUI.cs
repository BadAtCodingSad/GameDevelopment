using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuizUI : MonoBehaviour
{
    public static QuizUI Instance;
    private void Awake() => Instance = this;
   

    [Header("UI References")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public List<Button> answerButtons;

    private QuizData quizData;
    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private System.Action<int> onQuizComplete;

    private Color defaultColor = Color.white;
    private Color correctColor = Color.green;
    private Color wrongColor = Color.red;

     void Start(){
         quizPanel.SetActive(false);
    }

    public void StartQuiz(QuizData data, System.Action<int> onComplete)
    {
        quizData = data;
        currentQuestionIndex = 0;
        correctCount = 0;
        onQuizComplete = onComplete;

        quizPanel.SetActive(true);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        ResetButtonColors();

        QuizQuestion q = quizData.questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Count; i++)
        {
            int choiceIndex = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(choiceIndex));
            answerButtons[i].interactable = true;
        }
    }

    void OnAnswerSelected(int selectedIndex)
    {
        QuizQuestion q = quizData.questions[currentQuestionIndex];

        foreach (var btn in answerButtons) btn.interactable = false;

        if (selectedIndex == q.correctAnswerIndex)
        {
            correctCount++;
            answerButtons[selectedIndex].GetComponent<Image>().color = correctColor;
        }
        else
        {
            answerButtons[selectedIndex].GetComponent<Image>().color = wrongColor;
            answerButtons[q.correctAnswerIndex].GetComponent<Image>().color = correctColor;
        }

        StartCoroutine(NextQuestionAfterDelay());
    }

    IEnumerator NextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        currentQuestionIndex++;

        if (currentQuestionIndex < quizData.questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            quizPanel.SetActive(false);
            onQuizComplete?.Invoke(correctCount);
        }
    }

    void ResetButtonColors()
    {
        foreach (var btn in answerButtons)
        {
            btn.GetComponent<Image>().color = defaultColor;
        }
    }
}
