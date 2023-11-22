using UnityEngine;
using NUnit.Framework.Interfaces;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    ItemData itemData;
    private void OnEnable()
    {
        itemData = (ItemData)target;
    }
    public override void OnInspectorGUI()
    {
        if (itemData != null)                // 아이템 데이터가 있어야 하고
        {
            if (itemData.itemIcon != null) // 아이템 데이터에 아이콘 이미지가 있어야 한다.
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview");            // 제목 적기
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);  // 텍스쳐 가져오기(아이템 데이터에 있는 아이콘 이미지 기반)
                if (texture != null)                                               // 텍스쳐가 있으면
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 텍스쳐가 그려질 영역 설정
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 앞에서 설정한 영역에 텍스쳐 그리기
                }
            }
        }

        base.OnInspectorGUI();  // 기본적으로 그리던 것은 그대로 그리기
    } 
}
#endif
