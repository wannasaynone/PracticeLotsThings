using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonActorController : Photon.MonoBehaviour {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected PhotonView m_photonView = null;
    [SerializeField] protected float m_fixedPositionRange = 0.2f;

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_actor.Movement);
            stream.SendNext(m_actor.MotionCurve);
            stream.SendNext(m_actor.transform.rotation);
            stream.SendNext(m_actor.isSyncAttacking);
            stream.SendNext(m_actor.transform.position);
        }
        else if (stream.isReading)
        {
            Vector3 _direction = (Vector3)stream.ReceiveNext();
            float _motion = (float)stream.ReceiveNext();
            m_actor.SetMotion(_direction, _motion);
            m_actor.transform.rotation = (Quaternion)stream.ReceiveNext();
            m_actor.isSyncAttacking = (bool)stream.ReceiveNext();
            Vector3 _syncPosition = (Vector3)stream.ReceiveNext();
            if (Vector3.Distance(_syncPosition, m_actor.transform.position) > m_fixedPositionRange)
            {
                m_actor.transform.position = _syncPosition;
            }
        }
    }
}
