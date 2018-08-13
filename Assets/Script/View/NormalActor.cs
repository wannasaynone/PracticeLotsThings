
public class NormalActor : Actor {

    protected override void OnGetHit(EventManager.HitInfo hitInfo)
    {
        base.OnGetHit(hitInfo);
        if (hitInfo.HitCollider == m_collider)
        {
            if (Engine.ActorManager != null)
            {
                if (hitInfo.actorType == ActorFilter.ActorType.Zombie && GetCharacterStatus().HP <= 0)
                {
                    TimerManager.Schedule(2.3f, delegate
                    {
                        if (this == null || Engine.ActorManager == null)
                        {
                            return;
                        }
                        ZombieActor _zombie = Engine.ActorManager.CreateActor(Engine.GameSetting.ZombieActorPrefabID, transform.position) as ZombieActor;
                        _zombie.SetIsTransformedFromOthers();
                        _zombie.transform.rotation = transform.rotation;
                        Destroy(gameObject);
                    });
                }
            }
        }
    }

}
