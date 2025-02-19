using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject monsterPrefab; // 생성할 몬스터 프리팹
    public Transform spawnPoint; // 몬스터 스폰 위치

    public void SpawnMonster()
    {
        if (monsterPrefab != null && spawnPoint != null)
        {
            Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("몬스터가 스폰되었습니다!");
        }
        else
        {
            Debug.LogWarning("MonsterSpawn: 몬스터 프리팹 또는 스폰 위치가 설정되지 않았습니다.");
        }
    }
}
