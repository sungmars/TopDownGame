using UnityEngine;

public class Succle : MonoBehaviour
{
    public Transform targetTransform; // 순간이동할 목표 위치

    public void Teleport()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기

        if (player != null && targetTransform != null)
        {
            player.transform.position = targetTransform.position; // 플레이어 위치 변경
        }
        else
        {
            Debug.LogWarning("버그다.");
        }
    }
}
