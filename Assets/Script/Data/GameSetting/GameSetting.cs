﻿namespace PracticeLotsThings.Data
{
    public class GameSetting : IGameData
    {
        public int ID { get; private set; }
        public int NormalActorPrefabID { get; private set; }
        public int ShooterActorPrefabID { get; private set; }
        public int ZombieActorPrefabID { get; private set; }
        public int EmptyActorPrefabID { get; private set; }
        public int TowerActorPrefabID { get; private set; }
        public float Edge_MaxX { get; private set; }
        public float Edge_MinX { get; private set; }
        public float Edge_MaxZ { get; private set; }
        public float Edge_MinZ { get; private set; }
        public int SceneObjectNumber { get; private set; }
    }
}
