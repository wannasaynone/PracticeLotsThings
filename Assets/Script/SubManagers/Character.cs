using UnityEngine;

public class Character : SubManager {

    private ActorController m_actorController;

	public Character(ActorController actorController) : base(actorController)
    {
        m_actorController = actorController;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_actorController.CharacterStatus.AddHp(10);
            m_actorController.CharacterStatus.AddAtk(10);
            m_actorController.CharacterStatus.AddDef(10);
            m_actorController.CharacterStatus.AddMat(10);
        }
        return;
    }

}
