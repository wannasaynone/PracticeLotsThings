using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestActorCreator : EditorWindow {

	[MenuItem("Tools/Test Actor Creator")]
    private static void Init()
    {
        GetWindow<TestActorCreator>();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Normal"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Normal With Controller"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition()).EnableAI(false);
        }
        if (GUILayout.Button("Normal AI"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.NormalActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Zombie"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Zombie With Controller"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(false);
        }
        if (GUILayout.Button("Zombie AI"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Shooter"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Shooter With Controller"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(false);
        }
        if (GUILayout.Button("Shooter AI"))
        {
            Engine.ActorManager.CreateActor(Engine.GameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
        }
        EditorGUILayout.EndHorizontal();
    }

}
