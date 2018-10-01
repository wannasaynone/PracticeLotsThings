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
            Engine.ActorManager.CreateActor(GameManager.GameSetting.NormalActorPrefabID, null, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Normal With Controller"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.NormalActorPrefabID, delegate(Actor actor) { actor.EnableAI(false); CameraController.MainCameraController.Track(actor.gameObject); }, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Normal AI"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.NormalActorPrefabID, delegate (Actor actor) { actor.EnableAI(true); }, Engine.GetRamdomPosition());
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Zombie"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ZombieActorPrefabID, null, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Zombie With Controller"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ZombieActorPrefabID, delegate (Actor actor) { actor.EnableAI(false); CameraController.MainCameraController.Track(actor.gameObject); }, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Zombie AI"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ZombieActorPrefabID, delegate (Actor actor) { actor.EnableAI(true); }, Engine.GetRamdomPosition());
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Shooter"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ShooterActorPrefabID, null, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Shooter With Controller"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ShooterActorPrefabID, delegate (Actor actor) { actor.EnableAI(false); CameraController.MainCameraController.Track(actor.gameObject); }, Engine.GetRamdomPosition());
        }
        if (GUILayout.Button("Shooter AI"))
        {
            Engine.ActorManager.CreateActor(GameManager.GameSetting.ShooterActorPrefabID, delegate (Actor actor) { actor.EnableAI(true); }, Engine.GetRamdomPosition());
        }
        EditorGUILayout.EndHorizontal();
    }

}
