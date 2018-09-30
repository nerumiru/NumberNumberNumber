using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(ResizerByRect))]
public class ResizerByRectInspector : Editor
{
    ResizerByRect m_script;
    bool sizeFolder = true;
    bool positionFolder = true;

    private void OnEnable()
    {
        m_script = (ResizerByRect)target;
    }
    public override void OnInspectorGUI()
    {
        m_script.activeDepth = EditorGUILayout.IntField("Active Depth", m_script.activeDepth, GUILayout.Width(250));

        sizeFolder = EditorGUILayout.Foldout(sizeFolder, "크기 조절");
        m_script.isReSize = EditorGUILayout.Toggle("Use ReSizer", m_script.isReSize);
        EditorGUILayout.Space();
        if (sizeFolder)
        {
            if (m_script.isReSize)
            {
                m_script.isSizeTargetMode = EditorGUILayout.Toggle("Use Target Mode", m_script.isSizeTargetMode);
                if (m_script.isSizeTargetMode)
                {
                    m_script.sizeTarget = (GameObject)EditorGUILayout.ObjectField("Target Size Object", m_script.sizeTarget, typeof(GameObject), true);
                    
                }
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(m_script.isHightSquare == true);
                m_script.isSquare = EditorGUILayout.Toggle("To Square by Width", m_script.isSquare);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.isSquare == true);
                m_script.isHightSquare = EditorGUILayout.Toggle("To Square by Height", m_script.isHightSquare);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(m_script.isHightSquare == true);
                m_script.staticWidth = EditorGUILayout.Toggle("Use Object Width", m_script.staticWidth);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.isSquare == true);
                m_script.staticHeight = EditorGUILayout.Toggle("Use Object Height", m_script.staticHeight);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUI.BeginDisabledGroup(m_script.isHightSquare == true || m_script.staticWidth);
                m_script.widthRatio = EditorGUILayout.FloatField("Width Ratio", m_script.widthRatio, GUILayout.Width(250));
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.isSquare == true || m_script.staticHeight);
                m_script.heightRatio = EditorGUILayout.FloatField("Height Ratio", m_script.heightRatio, GUILayout.Width(250));
                EditorGUI.EndDisabledGroup();
            }
        }

        positionFolder = EditorGUILayout.Foldout(positionFolder, "위치 조절");
        m_script.isRePosition = EditorGUILayout.Toggle("Use RePosition", m_script.isRePosition);
        EditorGUILayout.Space();
        if (positionFolder)
        {
            if (m_script.isRePosition)
            {
                m_script.isPositionTargetMode = EditorGUILayout.Toggle("Use Target Mode", m_script.isPositionTargetMode);
                if (m_script.isPositionTargetMode)
                {
                    m_script.positionTarget = (GameObject)EditorGUILayout.ObjectField("Target Position Object", m_script.positionTarget, typeof(GameObject), true);
                }
                EditorGUILayout.Space();
                m_script.endOfParent = EditorGUILayout.Toggle("Start Over Parent", m_script.endOfParent);
                if (m_script.endOfParent)
                {
                    m_script.finalAncestor = (RectTransform)EditorGUILayout.ObjectField("Final Ancestor", m_script.finalAncestor, typeof(RectTransform), true);
                }
                EditorGUILayout.Space();
                m_script.selfSizeWidth = EditorGUILayout.Toggle("Use Self Size Width", m_script.selfSizeWidth);
                m_script.selfSizeHeight = EditorGUILayout.Toggle("Use Self Size Height", m_script.selfSizeHeight);
                EditorGUILayout.Space();
                EditorGUI.BeginDisabledGroup(m_script.stickRight == true);
                m_script.stickLeft = EditorGUILayout.Toggle("Stick Left", m_script.stickLeft);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.stickLeft == true);
                m_script.stickRight = EditorGUILayout.Toggle("Stick Right", m_script.stickRight);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.stickBottom == true);
                m_script.stickTop = EditorGUILayout.Toggle("Stick Top", m_script.stickTop);
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_script.stickTop == true);
                m_script.stickBottom = EditorGUILayout.Toggle("Stick Bottom", m_script.stickBottom);
                EditorGUI.EndDisabledGroup();
                m_script.positionXRatio = EditorGUILayout.FloatField("X-Pos Ratio", m_script.positionXRatio, GUILayout.Width(250));
                m_script.positionYRatio = EditorGUILayout.FloatField("Y-Pos Ratio", m_script.positionYRatio, GUILayout.Width(250));
            }
        }
    }
}
