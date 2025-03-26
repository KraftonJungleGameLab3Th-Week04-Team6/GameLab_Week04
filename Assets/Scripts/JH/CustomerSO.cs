using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerSO", menuName = "Scriptable Objects/CustomerSO")]
public class CustomerSO : ScriptableObject
{
    public string Name;
    public string Dialog;
    public int ChoiceNum;
    public string[] Choices;
}
