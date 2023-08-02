using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

//ItemData용 컴스텀 에디터. 두번째 파라미터가 true면 ItemData를 상속받은 자식클래스도 같이 적용받는다.
[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    ItemData itemData;
    private void OnEnable()
    {
        itemData = target as ItemData; // target은 
    }

    //인스펙터창의 내부를 그리는 함수 
    public override void OnInspectorGUI()
    {
        if (itemData != null)// 아이템 데이터가 있어야하고 
        {
            if (itemData.itemIcon != null)//데이터의 이미지가 있으면 
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview"); //제목 
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon); // 텍스쳐 가져오기 (아이템데이터의 아이템 아이콘 기반 )
                if (texture != null)
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                }
            }
        }
        base.OnInspectorGUI();
    }
}
#endif
