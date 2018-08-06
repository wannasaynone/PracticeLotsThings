
public class NormalActor : Actor {

    protected override void OnGetHit(EventManager.HitInfo hitInfo)
    {
        if (hitInfo.HitCollider == m_collider)
        {
            if (Engine.ActorManager != null)
            {
                if (Engine.ActorManager.GetCharacterStatus(this).HP < 0)
                {
                    return;
                }

                Engine.ActorManager.GetCharacterStatus(this).AddHP(-hitInfo.Damage);

                if (Engine.ActorManager.GetCharacterStatus(this).HP <= 0)
                {
                    m_aiController.enabled = false;
                    m_actorController.enabled = false;
                    m_collider.enabled = false;
                    m_rigidBody.useGravity = false;
                    m_lockMovement = true;
                    m_isAttacking = false;

                    if (CameraController.MainCameraController.TrackingGameObjectInstanceID == gameObject.GetInstanceID())
                    {
                        CameraController.MainCameraController.StopTrack();
                    }

                    m_actorAniamtorController.SetDie();

                    if (hitInfo.actorType == ActorFilter.ActorType.Zombie)
                    {
                        TimerManager.Schedule(2.3f, delegate
                        {
                            ZombieActor _zombie = Engine.ActorManager.CreateActor(Engine.Instance.GameSetting.ZombieActorPrefabID, transform.position) as ZombieActor;
                            _zombie.SetIsTransformedFromOthers();
                            _zombie.transform.rotation = transform.rotation;
                            Destroy(gameObject);
                        });
                    }
                }
            }
        }
    }

}
