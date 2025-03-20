using UnityEngine;

public class forhp : MonoBehaviour
{
    public RectTransform targetUIElement; // 需要调整宽度的UI元素
    public GameObject person;

    void Update()
    {
        // 动态设置宽度
        buff bu = person.GetComponent<buff>();
        targetUIElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bu.hp);
    }
}