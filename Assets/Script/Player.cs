using UnityEngine;

public class Player : MonoBehaviour
{
    //움직이는 속도 정의
    public float moveSpeed = 5.0f;

    // ↓ 카메라 및 플레이어 절반 폭 변수 추가
    private Camera mainCam;
    private float halfWidth;

    void Start()
    {
        //메인 카메라 가져오기
        mainCam = Camera.main;

        //SpriteRenderer의 절반 폭 계산 (화면 벗어나지 않게 하기 위해)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
        }
    }

    void Update()
    {
        //지정한 Axis를 통해 키의 방향을 판단하고 속도와 프레임 판정을 곱해 이동량 정함
        float distaceX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        //이동량만큼 실제로 이동을 해주는 함수
        transform.Translate(distaceX, 0, 0);

        //이동 후 화면 경계 밖으로 나가지 않도록 위치 제한
        ClampToScreen();
    }

    //화면 경계를 벗어나지 않도록 위치를 제한하는 함수
    void ClampToScreen()
    {
        Vector3 pos = transform.position;

        //카메라 기준 화면 좌하단(0,0) ~ 우상단(1,1)의 실제 위치를 가져옴
        float zDist = transform.position.z - mainCam.transform.position.z;
        Vector3 min = mainCam.ViewportToWorldPoint(new Vector3(0, 0, zDist));
        Vector3 max = mainCam.ViewportToWorldPoint(new Vector3(1, 1, zDist));

        //x 위치가 화면 밖으로 나가지 않도록 제한
        pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);

        //제한된 위치를 실제 위치에 적용
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") //적 충돌 판정
        {
            //적 파괴
            Destroy(collision.gameObject);

            //자기 자신 파괴
            Destroy(gameObject);
        }
    }
}
