// zoe - 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState { START,PLAYERTURN,ENEMYTURN,WON,LOST }

namespace zoe
{
    public class BattleSystem : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject enemyPrefab;

        public Transform playerSpawn;
        public Transform enemySpawn;

        unit playerUnit;
        unit enemyUnit;
        public BattleState state;

        public TextMeshProUGUI dialogue;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        void Start()
        {
            state = BattleState.START;
            StartCoroutine(battle());
        }

        IEnumerator battle()
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawn);
            playerUnit = playerObject.GetComponent<unit>();

            GameObject enemyObject = Instantiate(enemyPrefab, enemySpawn);
            enemyUnit = enemyObject.GetComponent<unit>();

            dialogue.text = "A Wild " + enemyUnit.unitName + " Has Appeared";

            playerHUD.setHUD(playerUnit);
            enemyHUD.setHUD(enemyUnit);

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            playerTurn();
        }

        void playerTurn() => dialogue.text = "Choose an action:";

        IEnumerator enemyTurn()
        {
            dialogue.text = enemyUnit.unitName + " is deciding their move...";
            yield return new WaitForSeconds(1f);

            // Implement enemy AI logic here
            int randomAction = Random.Range(0, 2); // 0 for attack, 1 for heal

            if (randomAction == 0)
            {
                dialogue.text = enemyUnit.unitName + " Attacks!";
                yield return new WaitForSeconds(1f);

                bool isDead = playerUnit.takeDamage(enemyUnit.damage);
                playerHUD.SetHP(playerUnit.currentHP);
                yield return new WaitForSeconds(1f);

                state = isDead ? BattleState.LOST : BattleState.PLAYERTURN;
                if (isDead)
                    endBattle();
                else
                    playerTurn();
            }
            else if (randomAction == 1)
            {
                enemyUnit.heal(5);
                enemyHUD.SetHP(enemyUnit.currentHP);
                dialogue.text = enemyUnit.unitName + " heals themselves!";
                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                playerTurn();
            }
        }

        void endBattle() 
        {
            if (state == BattleState.WON) 
            {
                dialogue.text = "You Won!";
            }
            else if (state == BattleState.LOST) 
            {
                dialogue.text = "You where defeated";
            }
        }

        public void onAttack()
        {
            if (state != BattleState.PLAYERTURN)
                return;

            StartCoroutine(playerAttack());
        }

        public void onHeal()
        {
            if (state != BattleState.PLAYERTURN)
                return;

            StartCoroutine(playerHeal());
        }

        IEnumerator playerAttack()
        {
            bool isDead = enemyUnit.takeDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogue.text = "Attack!";
            dialogue.text = "Choose an action";
            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                endBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                yield return StartCoroutine(enemyTurn());
            }
        }

        IEnumerator playerHeal()
        {
            playerUnit.heal(5);
            playerHUD.SetHP(playerUnit.currentHP);
            dialogue.text = "you feel renewed strength!";
            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            yield return StartCoroutine(enemyTurn());
        }
    }
}
