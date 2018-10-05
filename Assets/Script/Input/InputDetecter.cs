using UnityEngine;

namespace PracticeLotsThings.Input
{
    public abstract class InputDetecter : ScriptableObject
    {
        public static Vector3 MousePositionOnStage { get; protected set; }

        [SerializeField] protected float m_fixedMousePositionZ = -1.5f;

        public float Horizontal { get; protected set; }
        public float Vertical { get; protected set; }
        public string LastDirectionButton { get; protected set; }
        public bool IsMovePressed { get; protected set; }
        public bool IsMoving { get; protected set; }
        public bool IsRotateingCameraRight { get; protected set; }
        public bool IsRotateingCameraLeft { get; protected set; }
        public bool IsStartingAttack { get; protected set; }
        public bool IsAttacking { get; protected set; }
        public bool IsStartingSpeicalInput { get; protected set; }
        public bool IsProcessingSpecialInput { get; protected set; }
        public bool IsStartingInteract { get; protected set; }
        public bool IsInteracting { get; protected set; }

        public abstract void Update();
    }
}
