using UnityEngine;

[System.Serializable]
public class Character : SubManager {

    public enum Type
    {
        Player,
        NormalCharacter
    }

    public ActorController Actor { get; private set; }
    public Type CharacterType { get; private set; }

    public Character(ActorController actorController, Type characterType) : base(actorController)
    {
        Actor = actorController;
        Actor.CharacterStatus.OnHpValueChanged += OnHpValueChnaged;
        CharacterType = characterType;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Actor.CharacterStatus.AddHp(-100);
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
            Object.Destroy(Actor.gameObject);
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
        Actor.SetDead();
        Actor.CharacterStatus.OnHpValueChanged -= OnHpValueChnaged;
    }

}
