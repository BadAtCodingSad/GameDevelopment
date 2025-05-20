using UnityEngine;
using System.Collections;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;
    public GameObject objectivePannel;
    private GameManager gameManger;
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardText;
    public float pollutionDecreaseRate=1.5f;

    private void Awake()
    {
        Instance = this;
    }
    void Start(){
        rewardPanel.SetActive(false);
    }

    // Call this from your UI "Claim Reward" button
    public void OnClaimRewardButtonPressed()
    {
        // 🔹 You define the quiz topic right here
        objectivePannel.SetActive(false);
        
        string prompt = AIQuizService.Instance.BuildPrompt();
        StartCoroutine(AIQuizService.Instance.GenerateQuiz(prompt, OnQuizGenerated));
        ObjectiveManager.instance.objectiveButtons[AIQuizService.Instance.difficultyIndex%3].gameObject.SetActive(false);
        AIQuizService.Instance.difficultyIndex++;
        
    }

   
    private void OnQuizGenerated(QuizData quiz)
    {
        if (quiz == null || quiz.questions == null || quiz.questions.Count == 0)
        {
            Debug.LogError("Quiz data is null or empty.");
            return;
        }

        QuizUI.Instance.StartQuiz(quiz, OnQuizCompleted);
    }

    // Called after the player finishes the quiz
    private void OnQuizCompleted(int correctAnswers)
    {
        
        float decreaseAmount = pollutionDecreaseRate * correctAnswers;
        GameManager.instance.pollution -= decreaseAmount*100;
        Debug.Log($"pollution decreased  {decreaseAmount}");
        StartCoroutine(ShowRewardPanel(decreaseAmount));
        
    }
    private IEnumerator ShowRewardPanel(float amount)
    {
        rewardText.text = $"Pollution decreased by {amount}%";
        rewardPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        rewardPanel.SetActive(false);
    }
}
