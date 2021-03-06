﻿using UnityEngine;
using PracticeLotsThings.Input;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.View.Controller
{
    public class ActorController : View
    {
        public bool IsLocked { get { return m_lockDetect; } }

        [SerializeField] protected Actor.Actor m_actor = null;
        [SerializeField] protected InputDetecter m_inputDetecter = null;

        private bool m_lockDetect = false;

        public void Lock()
        {
            m_lockDetect = true;
        }

        public void Unlock()
        {
            m_lockDetect = false;
        }

        protected virtual void Update()
        {
            m_inputDetecter.Update();

            if (m_lockDetect)
            {
                return;
            }

            // for testing
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                CameraController.MainCameraController.Track(gameObject);
            }

            if (CameraController.MainCameraActor != null)
            {
                m_actor.SetMotion
                    (
                    m_inputDetecter.Horizontal * CameraController.MainCameraActor.transform.right + m_inputDetecter.Vertical * CameraController.MainCameraActor.transform.forward,
                    m_inputDetecter.Horizontal != 0f || m_inputDetecter.Vertical != 0f ? 1f : 0f
                    );
            }
            else
            {
                m_actor.SetMotion
                    (
                    m_inputDetecter.Horizontal,
                    m_inputDetecter.Vertical,
                    m_inputDetecter.Horizontal != 0f || m_inputDetecter.Vertical != 0f ? 1f : 0f
                    );
            }

            if (CameraController.MainCameraController != null)
            {
                if (CameraController.MainCameraController.TrackingGameObjectInstanceID == gameObject.GetInstanceID())
                {
                    if (m_inputDetecter.IsRotateingCameraRight)
                    {
                        CameraController.MainCameraController.Rotate(true);
                    }

                    if (m_inputDetecter.IsRotateingCameraLeft)
                    {
                        CameraController.MainCameraController.Rotate(false);
                    }
                }
            }

            if (m_inputDetecter.IsStartingInteract)
            {
                m_actor.StartInteracting();
            }

            if (!m_inputDetecter.IsInteracting)
            {
                m_actor.StopInteracting();
            }

            m_actor.FaceTo(InputDetecter.MousePositionOnStage);
            m_actor.SetCharacterStateUIActive(m_inputDetecter.IsProcessingSpecialInput);
        }
    }
}
