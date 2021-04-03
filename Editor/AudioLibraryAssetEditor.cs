using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace AudioLibrary
{
    [CustomEditor(typeof(AudioLibraryAsset))]
    public class AudioLibraryAssetEditor : Editor
    {
        private ReorderableListWrapper _bgmList;
        private ReorderableListWrapper _sfxList;
        private void OnEnable()
        {
            _bgmList = new ReorderableListWrapper(serializedObject.FindProperty("AudioBGM") ,serializedObject , "BGM");
            _sfxList = new ReorderableListWrapper(serializedObject.FindProperty("AudioSFX"), serializedObject, "SFX");

            _bgmList.SetupOrderList();
            _sfxList.SetupOrderList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck())
                _bgmList.SetupOrderList();
                _sfxList.SetupOrderList();

            _bgmList.DoLayourList();
            _sfxList.DoLayourList();

            serializedObject.ApplyModifiedProperties();
        }
    }

    internal class ReorderableListWrapper
    {
        private SerializedProperty _audioProp;
        private ReorderableList _list;
        private string _headerName = "";

        private readonly float kElementHeight = EditorGUIUtility.singleLineHeight * 3.5f;
        public int lineSpacing = 3;
        public ReorderableListWrapper(SerializedProperty audioProp, SerializedObject serializedObject, string headerName)
        {
            _headerName = headerName;
            _audioProp = audioProp.FindPropertyRelative("_audioCategoryList");
            _list = new ReorderableList(serializedObject, _audioProp, true, false, true, true);
        }

        public void SetupOrderList()
        {
            _list.drawElementCallback = DrawElement;
            _list.elementHeight = kElementHeight;
            _list.elementHeightCallback = GetElementHeight;
            _list.onAddCallback = OnAddCallback;
            _list.drawHeaderCallback = DrawHeader;
        }

        public void DoLayourList()
        {
            _list.DoLayoutList();
        }

        float GetElementHeight(int index)
        {
            SerializedProperty property = _audioProp.GetArrayElementAtIndex(index);
            SerializedProperty spriteListProp = property.FindPropertyRelative("_audioLabelList");
            if (spriteListProp.isExpanded)
                return (spriteListProp.arraySize + 1) * (EditorGUIUtility.singleLineHeight + lineSpacing) + kElementHeight;

            return kElementHeight;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _audioProp.GetArrayElementAtIndex(index);
            Rect catRect = new Rect(rect.x, rect.y, rect.width - (kElementHeight * 2), EditorGUIUtility.singleLineHeight);
            Rect vaRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width - kElementHeight, EditorGUIUtility.singleLineHeight);

            SerializedProperty categoryProp = element.FindPropertyRelative("_categoryName");
            SerializedProperty audioLabelListProp = element.FindPropertyRelative("_audioLabelList");

            EditorGUI.BeginChangeCheck();
            string newCatName = EditorGUI.DelayedTextField(catRect, categoryProp.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                categoryProp.stringValue = newCatName;
            }

            EditorGUI.PropertyField(vaRect, audioLabelListProp, new GUIContent("Label"));
            if (audioLabelListProp.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect indentedRect = EditorGUI.IndentedRect(vaRect);
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 40 + indentedRect.x - vaRect.x;
                indentedRect.y += EditorGUIUtility.singleLineHeight + lineSpacing;
                Rect sizeRect = indentedRect;
                int size = EditorGUI.IntField(sizeRect, "Size", audioLabelListProp.arraySize);
                if (size != audioLabelListProp.arraySize && size >= 0)
                    audioLabelListProp.arraySize = size;
                indentedRect.y += EditorGUIUtility.singleLineHeight + lineSpacing;
                DrawLabelProperties(indentedRect, audioLabelListProp);
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.indentLevel--;
            }

        }

        private void OnAddCallback(ReorderableList list)
        {
            int oldSize = _audioProp.arraySize;
            _audioProp.arraySize += 1;

            const string kNewCatName = "New Category";
            string newCatName = kNewCatName;

            SerializedProperty sp = _audioProp.GetArrayElementAtIndex(oldSize);
            sp.FindPropertyRelative("_categoryName").stringValue = newCatName;
        }

        private void DrawLabelProperties(Rect rect, SerializedProperty property)
        {
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                EditorGUI.BeginChangeCheck();
                string oldName = element.FindPropertyRelative("_label").stringValue;

                Rect nameRect = new Rect(rect.x - rect.width / 10 + 5, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight);

                string newName = EditorGUI.DelayedTextField(nameRect, oldName);
                if (EditorGUI.EndChangeCheck())
                {
                    newName = newName.Trim();
                    element.FindPropertyRelative("_label").stringValue = newName;
                }
                EditorGUI.PropertyField(new Rect(rect.x + rect.width / 2.5f, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("_audioClip"), new GUIContent("Audio"));
                rect.y += EditorGUIUtility.singleLineHeight + lineSpacing;
            }
        }

        private void DrawHeader(Rect rect)
        {
            string name = _headerName;
            EditorGUI.LabelField(rect, name);
        }
    }
}
