using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueUI; // 대화창 UI
    private bool isPlayerInRange = false;
    private bool dialogueFinished = false;

    private MonsterSpawn monsterSpawner; // 몬스터 스폰 스크립트 참조
    private GameObject[] portals; // Portal 태그를 가진 모든 오브젝트 저장

    private void Start()
    {
        // 시작할 때 대화창 비활성화
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }

        
        portals = GameObject.FindGameObjectsWithTag("Potal");
    }

    private void Update()
    {
        // 플레이어가 범위 안에 있고 스페이스바를 누르면 대화 시작
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space) && !dialogueFinished)
        {
            dialogueUI.SetActive(true);
            Debug.Log("대화 시작!");
            SetPortalActive(false); // 대화 중에는 포탈 비활성화
        }

        // 대화 종료 조건 (예: UI가 비활성화됨)
        if (dialogueUI.activeSelf == false && !dialogueFinished)
        {
            dialogueFinished = true; // 대화 종료 플래그 설정
            SpawnMonster(); // 몬스터 스폰 실행
            SetPortalActive(true); // 대화가 끝나면 포탈 다시 활성화
        }
    }

    private void SpawnMonster()
    {
        if (monsterSpawner != null)
        {
            monsterSpawner.SpawnMonster();
        }
        else
        {
            Debug.LogWarning("스폰스크립트랑 연결안됨");
        }
    }

    private void SetPortalActive(bool isActive)
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(isActive);
        }

        string state = isActive ? "활성화" : "비활성화";
        Debug.Log($"Portal 오브젝트가 {state}되었습니다.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("플레이어가 대화 범위 안에 들어왔습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("플레이어가 대화 범위를 벗어났습니다.");
        }
    }
}
