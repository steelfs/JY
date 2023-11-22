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
        if (itemData != null)                // ������ �����Ͱ� �־�� �ϰ�
        {
            if (itemData.itemIcon != null) // ������ �����Ϳ� ������ �̹����� �־�� �Ѵ�.
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview");            // ���� ����
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);  // �ؽ��� ��������(������ �����Ϳ� �ִ� ������ �̹��� ���)
                if (texture != null)                                               // �ؽ��İ� ������
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // �ؽ��İ� �׷��� ���� ����
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // �տ��� ������ ������ �ؽ��� �׸���
                }
            }
        }

        base.OnInspectorGUI();  // �⺻������ �׸��� ���� �״�� �׸���
    } 
}
#endif
