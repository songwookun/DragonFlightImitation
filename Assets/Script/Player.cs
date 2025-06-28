using UnityEngine;

public class Player : MonoBehaviour
{
    //�����̴� �ӵ� ����
    public float moveSpeed = 5.0f;

    // �� ī�޶� �� �÷��̾� ���� �� ���� �߰�
    private Camera mainCam;
    private float halfWidth;

    void Start()
    {
        //���� ī�޶� ��������
        mainCam = Camera.main;

        //SpriteRenderer�� ���� �� ��� (ȭ�� ����� �ʰ� �ϱ� ����)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
        }
    }

    void Update()
    {
        //������ Axis�� ���� Ű�� ������ �Ǵ��ϰ� �ӵ��� ������ ������ ���� �̵��� ����
        float distaceX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        //�̵�����ŭ ������ �̵��� ���ִ� �Լ�
        transform.Translate(distaceX, 0, 0);

        //�̵� �� ȭ�� ��� ������ ������ �ʵ��� ��ġ ����
        ClampToScreen();
    }

    //ȭ�� ��踦 ����� �ʵ��� ��ġ�� �����ϴ� �Լ�
    void ClampToScreen()
    {
        Vector3 pos = transform.position;

        //ī�޶� ���� ȭ�� ���ϴ�(0,0) ~ ����(1,1)�� ���� ��ġ�� ������
        float zDist = transform.position.z - mainCam.transform.position.z;
        Vector3 min = mainCam.ViewportToWorldPoint(new Vector3(0, 0, zDist));
        Vector3 max = mainCam.ViewportToWorldPoint(new Vector3(1, 1, zDist));

        //x ��ġ�� ȭ�� ������ ������ �ʵ��� ����
        pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);

        //���ѵ� ��ġ�� ���� ��ġ�� ����
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") //�� �浹 ����
        {
            //�� �ı�
            Destroy(collision.gameObject);

            //�ڱ� �ڽ� �ı�
            Destroy(gameObject);
        }
    }
}
