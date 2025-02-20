using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Vector3 savedPlayerPosition;
    private bool hasSavedPosition = false;
    public bool IsClear { get; private set; } = false; // 50점 이상이면 true

    private const string BestScoreKey = "BestScore";

    private void Awake()
    {
        // 싱글톤 패턴 적용 (씬이 변경되어도 유지됨)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 씬 로드 완료 후 자동으로 위치 복원
        SceneManager.sceneLoaded += OnSceneLoaded;

        // BestScore 확인하여 IsClear 설정
        int bestScore = PlayerPrefs.GetInt(BestScoreKey, -1); // -1이면 저장된 값 없음
        if (bestScore == -1)
        {
            Debug.Log(" PlayerPrefs에 저장된 스코어가 없습니다.");
        }
        else
        {
            IsClear = bestScore > 50;
            Debug.Log($"BestScore: {bestScore}, IsClear: {IsClear}");
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        savedPlayerPosition = position;
        hasSavedPosition = true;
        Debug.Log($"플레이어 위치 저장됨: {savedPlayerPosition}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasSavedPosition)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = savedPlayerPosition;
                
            }

        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
