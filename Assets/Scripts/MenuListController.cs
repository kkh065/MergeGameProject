using UnityEngine;

public class MenuListController : MonoBehaviour
{
    [SerializeField] GameObject MenuList;

    public void OpenMenu()
    {
        MenuList.SetActive(true);
        //레벨에 맞춰서 오픈된 컨텐츠 버튼만 노출되게끔 코딩
    }

    public void CloseMenu()
    {
        MenuList.SetActive(false);
    }
}
