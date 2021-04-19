#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class FOVVisualiser : Editor
{
    private void OnSceneGUI()
    {
        EnemyManager sight = (EnemyManager)target;
        Handles.color = new Color(1, 0, 0, 0.25f);

        Vector3 positionOnGround = new Vector3(sight.transform.position.x, 0, sight.transform.position.z);

        float leftAngle = sight.minDetectAngle / 2;
        leftAngle += sight.transform.eulerAngles.y;
        Vector3 fromVector = new Vector3(Mathf.Sin(leftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(leftAngle * Mathf.Deg2Rad));
        
        Handles.DrawSolidArc(positionOnGround, Vector3.up, fromVector, sight.maxDetectAngle, sight.detectRadius);
    }
}
#endif
