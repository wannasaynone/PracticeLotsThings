using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {

    public GameManager()
    {
        RegisterSubManager(new DamageCalculator(GetPage<CombatPage>()));
        RegisterSubManager(new CharacterGenerator(GetPage<CharacterListPage>()));
        RegisterSubManager(new CharacterManager(GetPage<CharacterListPage>()));
        RegisterSubManager(new AIMotionSetter(GetPage<CharacterListPage>()));
        RegisterSubManager(new TowerMotionSetter(GetPage<CharacterListPage>()));
    }

    public void StartNewGame()
    {
        GetSubManager<CharacterManager>().ResetCharacters();
        GetSubManager<CharacterGenerator>().CreateCharacter("Player", Character.Type.Player, 0);
        GetSubManager<CharacterGenerator>().CreateCharacter("AITester", Character.Type.NormalCharacter, 1, new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3)));
        GetSubManager<CharacterGenerator>().CreateCharacter("AITester", Character.Type.NormalCharacter, 1, new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10)));
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.F9))
        {
            StartNewGame();
        }
    }

}
