using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {

    public GameManager()
    {
        RegisterSubManager(new DamageCalculator(GetPage<CombatPage>()));
        RegisterSubManager(new CharacterGenerator(GetPage<CharacterListPage>()));
        RegisterSubManager(new CharacterManager(GetPage<CharacterListPage>()));
    }

    public void StartNewGame()
    {
        GetSubManager<CharacterManager>().ResetCharacters();
        GetSubManager<CharacterGenerator>().CreateCharacter("Player");
        GetSubManager<CharacterGenerator>().CreateCharacter("AITester", new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3)));
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
