// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

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

        private Enemy activeEnemy;

        string currentCharacter;
        int damageValue;

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

            dialogue.text = "You have been challenged by " + enemyUnit.unitName;

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

            CheckActiveEnemy();

            // Implement enemy AI logic here
            int randomAction = Random.Range(0, 2); // 0 for attack, 1 for heal
            int attackpower = Random.Range(0, 3);
            int[] options = new int[] { 5, 10, 15 };
  

            if (randomAction == 0)
            {
                dialogue.text = enemyUnit.unitName + " Attacks!";
                yield return new WaitForSeconds(1f);

                bool isDead = playerUnit.takeDamage(options[attackpower]);
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
                dialogue.text = "You were defeated";
            }
        }

        public void onAttack()
        {
            if (state != BattleState.PLAYERTURN)
                return;

            CheckActiveEnemy();
            CheckCurrentRecipe();
            StartCoroutine(playerAttack());
        }

        IEnumerator playerAttack()
        {
            switch (damageValue)
            {
                case 0:
                    dialogue.text = "The Potion was ineffective";
                    yield return new WaitForSeconds(2f);
                    state = BattleState.ENEMYTURN;
                    yield return StartCoroutine(enemyTurn());
                    break;
                case -1:
                    StartCoroutine(playerHeal());
                    break;
                case -2:
                    damageValue = 5;
                    StartCoroutine(playerHeal());
                    break;
                case -3:
                    damageValue = 10;
                    StartCoroutine(playerHeal());
                    break;
                case -4:
                    damageValue = 15;
                    StartCoroutine(playerHeal());
                    break;
                case -99:
                    damageValue = 15;
                    playerUnit.takeDamage(damageValue);

                    break;
                case -98:
                    damageValue = 0;
                    playerUnit.takeDamage(5);
                    dialogue.text = "Yuck! That potion didnt work!";
                    yield return new WaitForSeconds(2f);
                    state = BattleState.ENEMYTURN;
                    yield return StartCoroutine(enemyTurn());
                    break;
                default:
                    //
                    break;
            }

            bool isDead = enemyUnit.takeDamage(damageValue);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogue.text = "Attack!";
            dialogue.text = "Choose an action:";
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
            dialogue.text = "You feel renewed strength!";
            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            yield return StartCoroutine(enemyTurn());
        }

        void CheckActiveEnemy()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    activeEnemy = enemy;
                    if (activeEnemy.isGnome)
                    {
                        currentCharacter = "Gnome";
                    }
                    else if (activeEnemy.isFairy)
                    {
                        currentCharacter = "Fairy";
                    }
                    else if (activeEnemy.isFrog)
                    {
                        currentCharacter = "Frog";
                    }
                    else if (activeEnemy.isBee)
                    {
                        currentCharacter = "Bee";
                    }
                }
            }
        }

        /*
            affection 1 = 5
            ... 2 = 10
            ... 3 = 15

            0 = ineffective
            
            -1 = heal

            -2 = heal and 5 dmg
            -3 = heal and 10 dmg
            -4 = heal and 15 dmg

            specail cases:
            MR + BS
            -99 = 15 dmg to enemies and 5 to player

            FC + BS
            -98 = 5 dmg to player
        */

        private Dictionary<string, Dictionary<string, int>> characterDamageValues = new Dictionary<
            string,
            Dictionary<string, int>
        >()
        {
            {
                "Fairy",
                new Dictionary<string, int>()
                {
                    { "MRLN", 10 },
                    { "LNMR", 10 },
                    { "MRYN", 0 },
                    { "YNMR", 0 },
                    { "MRS", 0 },
                    { "SMR", 0 },
                    { "MRFC", -2 },
                    { "FCMR", -2 },
                    { "MRBS", -99 },
                    { "BSMR", -99 },
                    { "LNYN", 15 },
                    { "YNLN", 15 },
                    { "LNS", 5 },
                    { "SLN", 5 },
                    { "LNFC", -2 },
                    { "FCLN", -2 },
                    { "LNBS", 0 },
                    { "BSLN", 0 },
                    { "YNS", 0 },
                    { "SYN", 0 },
                    { "YNFC", -2 },
                    { "FCYN", -2 },
                    { "YNBS", 0 },
                    { "BSYN", 0 },
                    { "SFC", -2 },
                    { "FCS", -2 },
                    { "SBS", 5 },
                    { "BSS", 5 },
                    { "FCBS", -98 },
                    { "BSFC", -98 },
                }
            },
            {
                "Gnome",
                new Dictionary<string, int>()
                {
                    { "MRLN", 0 },
                    { "LNMR", 0 },
                    { "MRYN", 0 },
                    { "YNMR", 0 },
                    { "MRS", 10 },
                    { "SMR", 10 },
                    { "MRFC", -2 },
                    { "FCMR", -2 },
                    { "MRBS", -99 },
                    { "BSMR", -99 },
                    { "LNYN", 0 },
                    { "YNLN", 0 },
                    { "LNS", 10 },
                    { "SLN", 10 },
                    { "LNFC", -2 },
                    { "FCLN", -2 },
                    { "LNBS", 5 },
                    { "BSLN", 5 },
                    { "YNS", 10 },
                    { "SYN", 10 },
                    { "YNFC", -2 },
                    { "FCYN", -2 },
                    { "YNBS", 5 },
                    { "BSYN", 5 },
                    { "SFC", -2 },
                    { "FCS", -2 },
                    { "SBS", 10 },
                    { "BSS", 10 },
                    { "FCBS", -98 },
                    { "BSFC", -98 },
                }
            },
            {
                "Frog",
                new Dictionary<string, int>()
                {
                    { "MRLN", 5 },
                    { "LNMR", 5 },
                    { "MRYN", 5 },
                    { "YNMR", 5 },
                    { "MRS", 0 },
                    { "SMR", 0 },
                    { "MRFC", -2 },
                    { "FCMR", -2 },
                    { "MRBS", -99 },
                    { "BSMR", -99 },
                    { "LNYN", 10 },
                    { "YNLN", 10 },
                    { "LNS", 5 },
                    { "SLN", 5 },
                    { "LNFC", -2 },
                    { "FCLN", -2 },
                    { "LNBS", 0 },
                    { "BSLN", 0 },
                    { "YNS", 5 },
                    { "SYN", 5 },
                    { "YNFC", -2 },
                    { "FCYN", -2 },
                    { "YNBS", 0 },
                    { "BSYN", 0 },
                    { "SFC", -2 },
                    { "FCS", -2 },
                    { "SBS", 5 },
                    { "BSS", 5 },
                    { "FCBS", -98 },
                    { "BSFC", -98 },
                }
            },
            {
                "Bee",
                new Dictionary<string, int>()
                {
                    { "MRLN", 0 },
                    { "LNMR", 0 },
                    { "MRYN", 10 },
                    { "YNMR", 10 },
                    { "MRS", 0 },
                    { "SMR", 0 },
                    { "MRFC", -2 },
                    { "FCMR", -2 },
                    { "MRBS", -99 },
                    { "BSMR", -99 },
                    { "LNYN", 5 },
                    { "YNLN", 5 },
                    { "LNS", 0 },
                    { "SLN", 0 },
                    { "LNFC", -2 },
                    { "FCLN", -2 },
                    { "LNBS", 5 },
                    { "BSLN", 5 },
                    { "YNS", 5 },
                    { "SYN", 5 },
                    { "YNFC", -2 },
                    { "FCYN", -2 },
                    { "YNBS", 15 },
                    { "BSYN", 15 },
                    { "SFC", -2 },
                    { "FCS", -2 },
                    { "SBS", 10 },
                    { "BSS", 10 },
                    { "FCBS", -98 },
                    { "BSFC", -98 },
                }
            },
        };

        void CheckCurrentRecipe()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                string currentRecipe = gameManager.GetCurrentRecipe();

                if (characterDamageValues.ContainsKey(currentCharacter))
                {
                    Dictionary<string, int> currentCharacterDictionary = characterDamageValues[
                        currentCharacter
                    ];

                    if (currentCharacterDictionary.ContainsKey(currentRecipe))
                    {
                        damageValue = currentCharacterDictionary[currentRecipe];
                        // Apply the calculated damage value to the target
                    }
                    else
                    {
                        // Handle the case when the combination is not found for the character
                    }
                }
                else
                {
                    // Handle the case when the character is not found
                }
            }
        }
    }
}
