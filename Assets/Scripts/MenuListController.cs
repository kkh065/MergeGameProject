using UnityEngine;

public class MenuListController : MonoBehaviour
{
    [SerializeField] GameObject MenuList;

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
