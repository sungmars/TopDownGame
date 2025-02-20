using UnityEngine;
using UnityEngine.SceneManagement;

public class PIG_Talk : NPC_Talk
{
    protected override void EndDialogue()
    {
        base.EndDialogue();
        SavePlayerPositionAndChangeScene("StackGame"); // 위치 저장 후 씬 이동
    }

    private void SavePlayerPositionAndChangeScene(string sceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameManager.Instance.SavePlayerPosition(player.transform.position);
        }
        else
        {
            Debug.LogWarning("⚠ 플레이어 오브젝트를 찾을 수 없습니다!");
        }

        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        SetDialogueBasedOnScore();
    }

    private void SetDialogueBasedOnScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0); // 저장된 최고 점수 가져오기

        if (bestScore == 0)
        {
            
        }
        else if (!GameManager.Instance.IsClear)
        {
            // 최고 점수가 50 이하일 경우
            dialogueLines = new string[]
            {
                "어때? 쉽지 않지?",
                "재도전은 언제든 환영이라구.",
                "내 최고기록 50점을 넘어봐"
            };
        }
        else
        {
            // 최고 점수가 50 초과일 경우
            dialogueLines = new string[]
            {
                "이미 네가 이겼는데 한판 더 하게?",
                "인상적인걸..."
            };
        }

        Debug.Log($"현재 대사 설정됨 (BestScore: {bestScore}, IsClear: {GameManager.Instance.IsClear})");
    }
}
