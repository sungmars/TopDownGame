using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public GameObject ScorePanel; // 점수 정보를 표시할 패널
    public TextMeshProUGUI scoreText;   // 점수를 표시할 UI 텍스트

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Tab 키를 누르면 패널 활성화
        {
            ShowPanel();
        }

        if (Input.GetKeyUp(KeyCode.Tab)) // Tab 키를 떼면 패널 비활성화
        {
            HidePanel();
        }
    }

    void ShowPanel()
    {
        int savedScore = PlayerPrefs.GetInt("BestScore", 0); // PlayerPrefs에서 점수 가져오기
        if (savedScore == -1)
        {
            scoreText.text = "저장된 기록이 없습니다.";
            ScorePanel.SetActive(true);
        }
        else
        {
            scoreText.text = "점수: " + savedScore.ToString(); // UI 업데이트
            ScorePanel.SetActive(true); 
        }
    }

    void HidePanel()
    {
        ScorePanel.SetActive(false); // 패널 비활성화
    }
}
