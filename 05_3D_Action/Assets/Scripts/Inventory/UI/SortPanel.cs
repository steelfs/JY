using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SortPanel : MonoBehaviour
{
    TMP_Dropdown dropdown;
    Button runButton;

    //ItemSortBy sortBy = ItemSortBy.Code;

    public Action<ItemSortBy> onSort;
    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        runButton = GetComponentInChildren<Button>();

       // dropdown.onValueChanged.AddListener((index) => sortBy = (ItemSortBy)index);
        runButton.onClick.AddListener(() => onSort?.Invoke((ItemSortBy)dropdown.value));
    }
}
