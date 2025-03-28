using System.Collections.Generic;
using UnityEngine;

public class MenuDatabase : MonoBehaviour
{
    public static Dictionary<int , MenuData> ObjectData => _menuData;
    // int : key -> MenuData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , MenuData> _menuData = new Dictionary<int, MenuData>();

    private void Awake()
    {
        AddMenuData(201, "Menu/Menu1"); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddMenuData(202, "Menu/Menu2"); 
        AddMenuData(203, "Menu/Menu3"); 
        AddMenuData(204, "Menu/Menu4"); 
        AddMenuData(205, "Menu/Menu5");
        AddMenuData(206, "Menu/Menu6");
        AddMenuData(207, "Menu/Menu7");
    }

    public static void AddMenuData(int id, string path)
    {
        if (!_menuData.ContainsKey(id))
        {
            _menuData.Add(id, (MenuData)Resources.Load(path));
        }
    }

}
