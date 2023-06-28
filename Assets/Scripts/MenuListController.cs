using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListController : MonoBehaviour
{
    [SerializeField] GameObject MenuList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenMenu()
    {
        MenuList.SetActive(true);
        //������ ���缭 ���µ� ������ ��ư�� ����ǰԲ� �ڵ�
    }

    public void CloseMenu()
    {
        MenuList.SetActive(false);
    }
}
