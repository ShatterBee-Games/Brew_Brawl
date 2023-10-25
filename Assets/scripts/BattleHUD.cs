// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace zoe {
public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public Slider HP;

    public void setHUD(unit _unit) => (name.text, level.text, HP.maxValue, HP.value) = (_unit.unitName, "lvl " + _unit.unitLevel, _unit.maxHP, _unit.currentHP);

    public void SetHP(int hp) => HP.value = hp;
}
}
