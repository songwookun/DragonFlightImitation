using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject bullet; //�극�� �������� �޾ƿ� ����

    //
    
    void Start()
    {
        InvokeRepeating("Shoot", 0.5f, 0.5f);
    }

    void Shoot()
    { 
        //�̻��� ������, ��ó������, ���Ⱚ ����
        Instantiate(bullet, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
       
  
    }
}
