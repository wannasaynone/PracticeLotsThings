using System;
using UnityEngine;

public static class EventManager {

    public static Action<HitInfo> OnHit;

    public struct HitInfo
    {
        public ActorFilter.ActorType actorType;
        public Collider HitCollider;
        public Vector3 HitPosition;
        public int Damage;
    }

}
