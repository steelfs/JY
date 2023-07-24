using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    public Dropdown dropdown; // Assign this in the inspector

    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate
        {
            ChangeDropdownListSize();
        });
    }

    void ChangeDropdownListSize()
    {
        // Get the dropdown template and resize it
        RectTransform template = dropdown.template;
        template.sizeDelta = new Vector2(template.sizeDelta.x, 500); // Change 500 to whatever height you want
    }
}
