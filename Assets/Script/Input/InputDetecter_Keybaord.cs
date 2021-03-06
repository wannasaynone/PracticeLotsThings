﻿using UnityEngine;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.Input
{
    using Input = UnityEngine.Input;

    [CreateAssetMenu(menuName = "InputDeater_KeyBoard")]
    public class InputDetecter_Keybaord : InputDetecter
    {
        public override void Update()
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            IsMovePressed = Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal");
            IsMoving = Input.GetButton("Vertical") || Input.GetButton("Horizontal");
            if (Input.GetButtonDown("Vertical"))
            {
                LastDirectionButton = "Vertical";
            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                LastDirectionButton = "Horizontal";
            }
            else
            {
                LastDirectionButton = "";
            }
            IsStartingAttack = Input.GetMouseButtonDown(0);
            IsAttacking = Input.GetMouseButton(0);
            IsRotateingCameraRight = Input.GetKey(KeyCode.E);
            IsRotateingCameraLeft = Input.GetKey(KeyCode.Q);
            IsStartingSpeicalInput = Input.GetMouseButtonDown(1);
            IsProcessingSpecialInput = Input.GetMouseButton(1);
            IsStartingInteract = Input.GetKeyDown(KeyCode.Space);
            IsInteracting = Input.GetKey(KeyCode.Space);

            Ray _camRay = CameraController.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_camRay, out _hit, 100f, LayerMask.GetMask("Ground")))
            {
                MousePositionOnStage = _hit.point;
                MousePositionOnStage += CameraController.MainCameraActor.transform.forward * m_fixedMousePositionZ;
            }
        }
    }
}
