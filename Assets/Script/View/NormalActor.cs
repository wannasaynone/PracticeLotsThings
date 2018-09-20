
public class NormalActor : Actor {

    private int m_createdZombiePhotonViewID = 0;

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
                        if (!NetworkManager.IsOffline)
                        {
                            if (!ActorManager.IsMyActor(this))
                            {
                                return;
                            }
                            PhotonEventSender.OnActorCreated += OnZombieCreated;
                            m_createdZombiePhotonViewID = PhotonNetwork.AllocateViewID();
                            PhotonEventSender.CreateActor(GameManager.GameSetting.ZombieActorPrefabID, transform.position, transform.rotation.eulerAngles, m_createdZombiePhotonViewID);
                        }
                        else
                        {
                            ReplacePlayerWithEmpty();
                            Engine.ActorManager.CreateActor(GameManager.GameSetting.ZombieActorPrefabID,
                            delegate (Actor actor)
                            {
                                ZombieActor _zombie = actor as ZombieActor;
                                _zombie.SetIsTransformedFromOthers();
                                ActorManager.DestroyActor(this);
                            },
                            transform.position, transform.rotation.eulerAngles);
      
                        }
                    });
                }
            }
        }
    }

    protected void OnZombieCreated(Actor actor)
    {
        if(Engine.ActorManager.GetPhotonView(actor).viewID == m_createdZombiePhotonViewID)
        {
            PhotonEventSender.OnActorCreated -= OnZombieCreated;
            ZombieActor _zombie = actor as ZombieActor;
            _zombie.SetIsTransformedFromOthers();
            PhotonEventSender.ShowTransformedFromOthers(_zombie);
            ReplacePlayerWithEmpty();
            ActorManager.DestroyActor(this);
        }
    }
}
