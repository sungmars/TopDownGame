using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    private float offsetX;
    private float offsetY;

    void Start()
    {
        if (target == null)
            return;

        // 씬이 시작될 때 카메라 위치를 타겟 위치로 초기화 (x, y만 적용)
        Vector3 startPosition = transform.position;
        startPosition.x = target.position.x;
        startPosition.y = target.position.y;
        transform.position = startPosition;

        // 카메라가 따라갈 오프셋 저장
        offsetX = transform.position.x - target.position.x;
        offsetY = transform.position.y - target.position.y;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;
        pos.y = target.position.y + offsetY;
        transform.position = pos;
    }
}
