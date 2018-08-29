using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonActorController : Photon.MonoBehaviour {

    [SerializeField] protected Actor m_actor = null;
    [SerializeField] protected PhotonView m_photonView = null;

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_actor.Movement);
            stream.SendNext(m_actor.MotionCurve);
            stream.SendNext(m_actor.transform.rotation);
            stream.SendNext(m_actor.isSyncAttacking);
        }
        else if (stream.isReading)
        {
            Vector3 _direction = (Vector3)stream.ReceiveNext();
            float _motion = (float)stream.ReceiveNext();
            m_actor.SetMotion(_direction, _motion);
            m_actor.transform.rotation = (Quaternion)stream.ReceiveNext();
            m_actor.isSyncAttacking = (bool)stream.ReceiveNext();
        }
    }
}
