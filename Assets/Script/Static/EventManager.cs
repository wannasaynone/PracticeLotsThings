using System;
using UnityEngine;
using PracticeLotsThings.Manager;

namespace PracticeLotsThings
{
    public static class EventManager
    {
        public static Action<HitInfo> OnHit;

        public struct HitInfo
        {
            public ActorFilter.ActorType actorType;
            public Collider HitCollider;
            public Vector3 HitPosition;
            public int Damage;
        }
    }
}
