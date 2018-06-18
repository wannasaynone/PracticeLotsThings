using UnityEngine;

[System.Serializable]
public class Character : SubManager {

    private ActorController m_actorController;

	public Character(ActorController actorController) : base(actorController)
    {
        m_actorController = actorController;
        m_actorController.CharacterStatus.OnHpValueChanged += OnHpValueChnaged;
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
            // TODO: make actor play dead animation
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
            m_actorController.SetDead();
            m_actorController.CharacterStatus.OnHpValueChanged -= OnHpValueChnaged;
        }
    }

}
