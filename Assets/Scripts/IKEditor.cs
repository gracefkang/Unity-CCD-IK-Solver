using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Allows ik solver to run in editor. Creates gizmo for moving the handles.
/// </summary>
[CustomEditor(typeof(CCDSolver))]
public class IKEditor : Editor
{
    static GUIStyle errorBox;

    [DrawGizmo(GizmoType.Selected)]
    static void OnDrawGizmosSelected(CCDSolver ccd, GizmoType gizmoType)
    {
        Handles.color = Color.blue;
        Handles.Label(ccd.target, "Target");
        var distanceToTarget = Vector3.Distance(ccd.target, ccd.tip.position);
        var midPoint = Vector3.Lerp(ccd.target, ccd.tip.position, 0.5f);
        Handles.Label(midPoint, string.Format("Distance to Target: {0:0.00}", distanceToTarget));
    }

    void OnEnable()
    {
        errorBox = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).box);
        errorBox.normal.textColor = Color.red;
    }

    public void OnSceneGUI()
    {
        var ccd = target as CCDSolver;
        ccd.target = Handles.PositionHandle(ccd.target, Quaternion.identity);
        ccd.Update();
    }

    public override void OnInspectorGUI()
    {
        var s = target as CCDSolver;
        if ( s.tip == null)
            EditorGUILayout.HelpBox("Please assign effector tip transform.", MessageType.Error);
        base.OnInspectorGUI();
    }
}
