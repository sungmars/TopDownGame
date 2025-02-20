using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f;//블럭 사이즈
    private const float MovingBoundsSize = 3f;//이동량
    private const float StackMovingSpeed = 5.0f;//각각의 스피드
    private const float BlockMovingSpeed = 3.5f;//
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    Transform lastBlock = null;
    float blockTransition = 0f;
    float secondaryPosition = 0f;

    int stackCount = -1;//시작하면서 +1을 하면서 사용할것이기 때문에 -1로 시작
    public int Score { get { return stackCount; } }
    int comboCount = 0;
    public int Combo { get { return comboCount; } }
    private int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }
    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";

    private bool isGameOver = true;
    void Start()
    {
        if (originBlock == null)
        {
            Debug.Log("OriginBlock is Null");
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestScoreKey, 0);

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;


        Spawn_Block(); Spawn_Block();

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                //게임오버~
                Debug.Log("Game Over");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                UIManager.Instance.SetScoreUI();
            }
        }
            MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);//디자이어포지션을 한번에 이동시키면 카메라의 끊어짐이 있는것처럼 움직여짐
                                                                                                                  //러프라는 값은 선형보관을 의미함 일정한 값을 선형으로 두고 거기서 퍼센테이지(t(0~1))로 가져감
                                                                                                                  //Lerp(a,b,t)=a+(b-a)*t 여기서 a라는 값이 0이고 b라는 값이 10이라면 t가 0.5라면 5가 반환됨
                                                                                                                  //위의 러프에선 현재 위치와 목적지의 위치를 주게 되고 일정한 퍼센테이지를 주게됨. 매 프레임마다 desiredPosition으로 이동할 수 있게 해줌.
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);//복제 생성

        if (newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);
        newTrans = newBlock.transform;
        newTrans.parent = this.transform;//새롭게 생성된 오브젝트의 부모를 나의 트랜스폼으로 가져다준다. 이 스크립트를 가진 오브젝트를 부모로 만드는 코드
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity;//회전없는상태
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount;//디자이어 포지션 의미: 스택카운트가 증가하는 만큼 더 스택이라고 하는 아이를 바닥으로 내림
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;
        UIManager.Instance.UpdateScore();
        return true;
    }
    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);//0부터 10까지의 값들이 순환을 돌게 되고 거기서 10으로 나눠줌.그렇게 되면 0부터 1까지의 값이 나오게 됨. 난 이게 뭔 소린지 모르겠다.

        Renderer rn = go.GetComponent<Renderer>();//랜더러라는것은 무언가를 그려낸다는것. 블럭이 가지고 있는 랜더러는 매쉬 랜더러다. 매쉬랜더러의 부모 클래스가 랜더러가 된다.<<이것도 뭔소리지.

        if (rn == null)
        {
            Debug.Log("Renderer is NULL");
            return;
        }
        rn.material.color = applyColor;//랜더러의 material의 color를 바꿔준다.
        Camera.main.backgroundColor = applyColor-new Color(0.1f,0.1f,0.1f);//Camera.main은 가장 처음에 메인카메라라는 태그를 달고켜진 카메라를 미리 저장해둔것.

        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }
    void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blockTransition, BoundSize)-BoundSize/2;//PingPong은 0부터 우리가 지정해준 값까지를 순환하는 메서드.
        
        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        else
        {
            lastBlock.localPosition = new Vector3(
                secondaryPosition, stackCount, -movePosition * MovingBoundsSize);
        }

    }

    bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;

        if (isMovingX)
        {
            float deltaX = prevBlockPosition.x - lastPosition.x;//deltaX는 잘려나가는 크기
            bool isNegativeNum = (deltaX < 0) ? true : false;
            deltaX = Mathf.Abs(deltaX);
            if (deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;
                if (stackBounds.x <= 0)
                {
                    return false;
                }
                float middle = (prevBlockPosition.x + lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(
                    isNegativeNum 
                    ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale 
                    : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale, 
                    lastPosition.y, 
                    lastPosition.z
                    ),
                    new Vector3(deltaX,1,stackBounds.y)
                    );
                comboCount = 0;
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true : false;
            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > ErrorMargin)
            {
                stackBounds.y -= deltaZ;
                if (stackBounds.y <= 0)
                {
                    return false;
                }
                float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(new Vector3(
                    lastPosition.x,
                    lastPosition.y,
                    isNegativeNum
                    ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                    : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale),
                    new Vector3(stackBounds.x, 1, deltaZ)
                    );
                comboCount = 0;
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;
        return true;
    }

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);//블럭 복제
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;//포지션과 스케일 결정
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    void ComboCheck()
    {
        comboCount++;
        if (comboCount > maxCombo)
            maxCombo = comboCount;
        if((comboCount%5)==0)
                {
            Debug.Log("5 sCombo Success!");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }
    void GameOverEffect()
    {
        int childCount = this.transform.childCount;

        for(int i = 1; i < 20; i++)
        {
            if (childCount < i) break;
            GameObject go = transform.GetChild(childCount - i).gameObject;
            if (go.name.Equals("Rubble")) continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();
            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
                );
        }
    }
    public void Restart()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        isGameOver = false;
        lastBlock = null;
        desiredPosition = Vector3.zero;
        stackBounds = new Vector3(BoundSize, BoundSize);

        stackCount = -1;
        isMovingX = true;
        blockTransition = 0f;
        secondaryPosition = 0f;

        comboCount = 0;
        maxCombo = 0;
        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        Spawn_Block();
        Spawn_Block();
    }
}
