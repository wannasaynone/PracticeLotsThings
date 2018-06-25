using UnityEngine;

[System.Serializable]
public class Character : SubManager {

    public struct CharacterInfo
    {
        public Transform Transform;
        public InputDetecter InputDetecter;
        public Type Type;
        public ActorController.AttackState CurrentAttackState;
        public ActorController.MoveState CurrentMoveState;
    }

    public enum Type
    {
        Player,
        NormalCharacter
    }

    public CharacterInfo Info
    {
        get
        {
            return new CharacterInfo
            {
                Transform = m_actorController.ModelAnimator.transform,
                InputDetecter = m_actorController.InputDetecter,
                Type = m_characterType,
                CurrentAttackState = m_actorController.CurrentAttackState,
                CurrentMoveState = m_actorController.CurrentMoveState
            };
        }
    }

    private Type m_characterType = Type.NormalCharacter;
    private ActorController m_actorController;

	public Character(ActorController actorController, Type characterType) : base(actorController)
    {
        m_actorController = actorController;
        m_actorController.CharacterStatus.OnHpValueChanged += OnHpValueChnaged;
        m_characterType = characterType;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_actorController.CharacterStatus.AddHp(-100);
        }
        return;
    }

    public void RemoveCharacter(bool playDeadAnimation = false)
    {
        if(playDeadAnimation)
        {
            OnHpEqualZero();
        }
        else
        {
            Object.Destroy(m_actorController.gameObject);
        }
    }

    private void OnHpValueChnaged(int value)
    {
        if(value <= 0)
        {
            OnHpEqualZero();
        }
    }

    private void OnHpEqualZero()
    {
        m_actorController.SetDead();
        m_actorController.CharacterStatus.OnHpValueChanged -= OnHpValueChnaged;
    }

}
