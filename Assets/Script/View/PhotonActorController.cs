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
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(m_actor.isSyncAttacking);
        }
        else if (stream.isReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            m_actor.isSyncAttacking = (bool)stream.ReceiveNext();
        }
    }
}
