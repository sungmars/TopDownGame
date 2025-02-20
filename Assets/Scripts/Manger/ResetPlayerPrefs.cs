using UnityEngine;
using UnityEngine.UI;

public class ResetPlayerPrefs : MonoBehaviour
{
    public Button resetButton; // 버튼 연결

    private void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(DeleteAllPlayerPrefs);
        }
        else
        {
            Debug.LogWarning("⚠ Reset 버튼이 연결되지 않았습니다!");
        }
    }

    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // 변경 사항 즉시 저장
        Debug.Log("✅ 모든 PlayerPrefs 데이터가 삭제되었습니다!");
    }
}
