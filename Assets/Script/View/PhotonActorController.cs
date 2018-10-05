using UnityEngine;

namespace PracticeLotsThings.View.Controller
{
    public class PhotonActorController : Photon.MonoBehaviour
    {
        [SerializeField] protected Actor.Actor m_actor = null;
        [SerializeField] protected PhotonView m_photonView = null;

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(m_actor.transform.rotation);
                stream.SendNext(m_actor.isSyncAttacking);
                stream.SendNext(m_actor.transform.position);
                stream.SendNext(m_actor.Movement);
                stream.SendNext(m_actor.MotionCurve);
            }
            else if (stream.isReading)
            {
                m_actor.transform.rotation = (Quaternion)stream.ReceiveNext();
                m_actor.isSyncAttacking = (bool)stream.ReceiveNext();
                m_actor.networkPosition = (Vector3)stream.ReceiveNext();
                Vector3 _movement = (Vector3)stream.ReceiveNext();
                float _motion = (float)stream.ReceiveNext();
                m_actor.SyncMotion(_movement.x, _movement.z, _motion);
            }
        }
    }
}
