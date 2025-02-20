using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PIG_Talk : NPC_Talk
{
    protected override void EndDialogue()
    {
        base.EndDialogue();
      
        SceneManager.LoadScene("StackGame"); // "StackGame" 씬으로 이동
    }
}
