using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject bullet; //브레스 프리팹을 받아올 변수

    //
    
    void Start()
    {
        InvokeRepeating("Shoot", 0.5f, 0.5f);
    }

    void Shoot()
    { 
        //미사일 프리팹, 런처포지션, 방향값 안줌
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
       
  
    }
}
