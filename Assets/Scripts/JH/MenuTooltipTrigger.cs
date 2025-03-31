using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTooltipTrigger : TooltipTrigger
{
    private string _menuName;
    private string _price;
    private string _ingredients;


    public void Init(int menuIndex)
    {
        print("MenuTooltipTrigger Init");
        //int index = -1;
        ////_currentOrderData
        //for (int i = 0; i < 7; i++)
        //{
        //    if (MenuDatabase.ObjectData[i].menuID == menuIndex)
        //    {
        //        index = i;
        //        break;
        //    }
        //}
        
        //if (index == -1)
        //{
        //    message = "";
        //    return;
        //}

        //MenuData _menuData = MenuDatabase.ObjectData[index];
        MenuData _menuData = MenuDatabase.ObjectData[menuIndex];
        _menuName = _menuData.menuName;
        _price = _menuData.menuPrice.ToString();
        _ingredients = "";
        for (int i = 0; i < _menuData.menuIngredients.Count; i++)
        {
            int _IngredientsIndex = _menuData.menuIngredients[i];
            _ingredients += IngredientsDatabase.ObjectData[_IngredientsIndex].IngredientsName;
            if (i != _menuData.menuIngredients.Count - 1) { _ingredients += ", "; }
        }
        message = $"{_menuName}, {_price}원\n[{_ingredients}]";
    }
}
