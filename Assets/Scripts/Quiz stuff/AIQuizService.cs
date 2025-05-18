using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

[Serializable]
public class Message
{
    public string content;
}


public class AIQuizService : MonoBehaviour
{
    [Header("Quiz Instructions")]
[TextArea(4, 10)]
public string quizInstructions = @"Create a 4-question multiple-choice quiz (MSQ) related to sustainability, specifically:
- The difference between renewable and non-renewable energy sources
- How they work (e.g., wind turbines, solar panels, coal-burning factories)
- The pollution or environmental impact they cause

Make sure the questions are concise, and the 4 multiple choices are short too (like three word max for each word).";
 private const string jsonFormatInstructions = "Respond in JSON format ONLY, matching the QuizData structure: {\"questions\":[{\"questionText\":\"...\", \"answers\":[\"A\",\"B\",\"C\",\"D\"], \"correctAnswerIndex\":0}]}";


    public static AIQuizService Instance;

    private string openAIKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string [] difficultyList= {"Easy", "Medium", "Hard"};
    public int difficultyIndex=0;

    
    private void Awake()
    {
        Instance = this;
        LoadApiKey();
    }

    private void LoadApiKey()
    {
        TextAsset keyFile = Resources.Load<TextAsset>("openai_key");
        if (keyFile != null)
        {
            openAIKey = keyFile.text.Trim();
            Debug.Log("OpenAI API key loaded successfully.");
        }
        else
        {
            Debug.LogError("openai_key.txt not found in Resources folder!");
        }
    }

    public IEnumerator GenerateQuiz(string prompt, Action<QuizData> onComplete)
    {
        if (string.IsNullOrEmpty(openAIKey))
        {
            Debug.LogError("OpenAI API key is missing!");
            yield break;
        }

        // Use the public instructions string
        string json = @"
        {
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [
                { ""role"": ""system"", ""content"": """ + EscapeJson(quizInstructions) + @"""},
                { ""role"": ""user"", ""content"": """ + EscapeJson(prompt) + @""" }
            ],
            ""temperature"": 0.7
        }";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + openAIKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Quiz API Error: " + request.error);
            Debug.LogError("Raw Response: " + request.downloadHandler.text);
            yield break;
        }

        string responseText = request.downloadHandler.text;
        string quizJson = ExtractQuizJson(responseText);

        Debug.Log("Raw GPT response: " + responseText);
        Debug.Log("Extracted Quiz JSON: " + quizJson);

        QuizData quizData = null;
        try
        {
            quizData = JsonUtility.FromJson<QuizData>(quizJson);
        }
        catch (Exception e)
        {
            Debug.LogError("QuizData deserialization failed: " + e.Message);
        }

        if (quizData == null)
        {
            Debug.LogError("Quiz data is null or empty. JSON: " + quizJson);
        }

        onComplete?.Invoke(quizData);
    }

    private string ExtractQuizJson(string responseText)
    {
        try
        {
            OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(responseText);
            if (response != null && response.choices != null && response.choices.Length > 0)
            {
                return response.choices[0].message.content.Trim();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse OpenAI response: " + e.Message);
        }
        Debug.LogError("Failed to extract quiz JSON from GPT response.");
        return "{}";
    }

    private string EscapeJson(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }
    public string BuildPrompt()
{
    string difficulty = difficultyList[difficultyIndex % difficultyList.Length];
    return $"{quizInstructions}\nDifficulty: {difficulty}.\n{jsonFormatInstructions}";
}
}
