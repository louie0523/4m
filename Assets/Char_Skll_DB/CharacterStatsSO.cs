using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "GameData/CharacterStats")]
public class CharacterStatsSO : ScriptableObject
{
    // 캐릭터 기본 정보
    public string characterName;  // 이름
    public Sprite portrait;       // 초상화
    public string description;    // 캐릭터 설명

    // 기본 스탯
    public int health;           // 체력
    public int attack;           // 공격력
    public int abilityPower;     // 주문력
    public int defense;          // 방어력
    public int resistance;       // 저항력
    public int perception;       // 감각 (회피율)
    public int mana;             // 현재 마나
    public int manaRegen;        // 마나 재생량
    public int skillSlots;       // 스킬 슬롯 (캐릭터가 장착할 수 있는 스킬 개수)
}

