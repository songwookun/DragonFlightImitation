using UnityEngine;

public class BackgroundRepeat : MonoBehaviour
{
    //��ũ�� �ӵ� ����
    public float scrollSpeed = 1.2f;
    //������ ��Ƽ���� �޾ƿ� ��ü ����
    private Material thisMaterial;
   
    void Start()
    {
        //���� ��ü�� component���� ������ Renderer��� ������Ʈ Material���� �޾ƿ�
        thisMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //���Ӱ� �������� offset ��ü ����
        Vector2 newoffset = thisMaterial.mainTextureOffset;   
        //y�κп� ���� y���� �ӵ� ������ ������ ����
        newoffset.Set(0, newoffset.y + (scrollSpeed * Time.deltaTime)); 
        //���� offset���� ����
        thisMaterial.mainTextureOffset = newoffset;
    }
}
