using UnityEngine;

public class BackgroundRepeat : MonoBehaviour
{
    //스크롤 속도 지정
    public float scrollSpeed = 1.2f;
    //쿼드의 머티리얼 받아올 객체 선언
    private Material thisMaterial;
   
    void Start()
    {
        //현재 객체의 component들을 참조해 Renderer라는 컴포넌트 Material정보 받아옴
        thisMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //새롭게 지정해줄 offset 객체 선언
        Vector2 newoffset = thisMaterial.mainTextureOffset;   
        //y부분에 현재 y값에 속도 프레임 보정을 더함
        newoffset.Set(0, newoffset.y + (scrollSpeed * Time.deltaTime)); 
        //최종 offset값을 지정
        thisMaterial.mainTextureOffset = newoffset;
    }
}
