using UnityEngine;

[CreateAssetMenu(menuName = "InputDeater_KeyBoard")]
public class InputDetecter_Keybaord : InputDetecter {

    public override void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        IsStartingAttack = Input.GetMouseButtonDown(0);
        IsAttacking = Input.GetMouseButton(0);
        IsRotateingCameraRight = Input.GetKey(KeyCode.E);
        IsRotateingCameraLeft = Input.GetKey(KeyCode.Q);
        IsStartingSpeicalInput = Input.GetMouseButtonDown(1);
        IsProcessingSpecialInput = Input.GetMouseButton(1);
        IsStartingInteract = Input.GetKeyDown(KeyCode.Space);
        IsInteracting = Input.GetKey(KeyCode.Space);
    }

}
