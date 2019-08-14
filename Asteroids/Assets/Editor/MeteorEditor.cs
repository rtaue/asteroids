using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Meteor))]
public class MeteorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Meteor myMeteor = (Meteor)target;

        myMeteor.minMaxForce = EditorGUILayout.FloatField("Force", myMeteor.minMaxForce);
        myMeteor.minMaxTorque = EditorGUILayout.FloatField("Torque", myMeteor.minMaxTorque);
        myMeteor.health = EditorGUILayout.IntField("Health", myMeteor.health);
        myMeteor.damage = EditorGUILayout.IntField("Damage", myMeteor.damage);
        myMeteor.maxScore = EditorGUILayout.IntField("Score", myMeteor.maxScore);
        myMeteor.spawnMeteor = EditorGUILayout.Toggle("Spawn Meteor", myMeteor.spawnMeteor);
        if (myMeteor.spawnMeteor)
        {
            myMeteor.meteorTag = EditorGUILayout.TextField("Meteor Tag", myMeteor.meteorTag);
            myMeteor.randomSpawn = EditorGUILayout.Toggle("Random Quantity", myMeteor.randomSpawn);
            if (myMeteor.randomSpawn)
            {
                myMeteor.maxMeteorQuantity = EditorGUILayout.IntField("Max Quantity", myMeteor.maxMeteorQuantity);
            }
            else
            {
                myMeteor.meteorQuantity = EditorGUILayout.IntField("Quantity", myMeteor.meteorQuantity);
            }
        }

    }
}
