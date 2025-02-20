using UnityEngine;
using TMPro;
using System.Collections;

public class NPC_Talk : MonoBehaviour
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

    private void Start()
    {
        // 시작할 때 대화창 비활성화
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    public void Talk()
    {
        if (isTyping) return; // 타이핑 중이면 무시

        if (!isTalking)
        {
            // 처음 대화를 시작하는 경우
            isTalking = true;
            dialogueUI.SetActive(true);
            nameText.text = npcName; // 이름 즉시 출력
            currentLineIndex = 0;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            // 다음 대화로 진행
            NextDialogue();
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
        }

        isTyping = false;
    }

    private void NextDialogue()
    {
        if (isTyping) return; // 타이핑 중이면 무시

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

    protected virtual void EndDialogue()
    {
        dialogueUI.SetActive(false); // 대화창 닫기
        isTalking = false;
        currentLineIndex = 0; // 대화 인덱스 초기화
    }
}
