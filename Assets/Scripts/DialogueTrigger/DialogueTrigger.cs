using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueUI; // 대화창 UI
    public TextMeshProUGUI nameText; // 이름 표시 텍스트
    public TextMeshProUGUI dialogueText; // 대화 텍스트 (타이핑 효과 적용)
    public string npcName; // NPC 이름
    public string[] dialogueLines; // 대화 내용 배열
    public float typingSpeed = 0.05f; // 타이핑 속도

    private int currentLineIndex = 0; // 현재 대화 줄 인덱스
    private bool isTyping = false; // 현재 타이핑 중인지
    private bool isTalking = false; // 대화창이 열려있는지
    private bool dialogueFinished = false; // 대화가 끝났는지 체크

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
        monsterSpawner = FindObjectOfType<MonsterSpawn>(); // 몬스터 스폰 스크립트 찾기
    }

    private void Update()
    {
        // 플레이어가 대화 중일 때 Space를 누르면 다음 대화 진행
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }

        // 대화 종료 후 몬스터 스폰 및 포탈 활성화
        if (dialogueFinished)
        {
            SpawnMonster();
            SetPortalActive(true);
            dialogueFinished = false; // 한 번만 실행되도록 설정
        }
    }

    public void Talk()
    {
        if (!isTalking)
        {
            // 처음 대화를 시작하는 경우
            isTalking = true;
            dialogueUI.SetActive(true);
            nameText.text = npcName; // NPC 이름 즉시 출력
            currentLineIndex = 0;
            StartCoroutine(TypeDialogue());
            SetPortalActive(false); // 대화 중에는 포탈 비활성화
        }
    }

    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = ""; // 기존 텍스트 초기화

        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도

            // ✅ 만약 Space를 누르면 즉시 대화 출력 완료
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.text = dialogueLines[currentLineIndex];
                break;
            }
        }

        isTyping = false; // ✅ isTyping을 false로 설정하여 다음 입력을 받도록 함
    }

    private void NextDialogue()
    {
        if (isTyping)
        {
            // ✅ 현재 타이핑 중이면 문장을 즉시 완성하고 종료
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
            return;
        }

        currentLineIndex++; // 다음 대화로 이동

        if (currentLineIndex >= dialogueLines.Length)
        {
            EndDialogue(); // 대화가 끝났으면 닫기
        }
        else
        {
            StartCoroutine(TypeDialogue()); // 다음 대화 출력
        }
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false); // 대화창 닫기
        isTalking = false;
        dialogueFinished = true; // 몬스터 스폰 및 포탈 활성화를 위해 플래그 설정
        currentLineIndex = 0; // 대화 인덱스 초기화
    }

    private void SpawnMonster()
    {
        if (monsterSpawner != null)
        {
            Debug.Log("몬스터 스폰 시작!");
            monsterSpawner.SpawnMonster();
            monsterSpawner.SpawnMonster();
            monsterSpawner.SpawnMonster();
            monsterSpawner.SpawnMonster();
            monsterSpawner.SpawnMonster();
        
        }
        else
        {
            Debug.LogWarning("스폰 스크립트가 연결되지 않음.");
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
            Debug.Log("플레이어가 대화 범위 안에 들어왔습니다.");
            Talk();
        }
    }
}
