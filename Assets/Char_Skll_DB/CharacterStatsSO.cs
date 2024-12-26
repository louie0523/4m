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

    // 성장 스탯
    public int healthUp;           // 체력
    public int attackUp;           // 공격력
    public int abilityPowerUp;     // 주문력
    public int defenseUp;          // 방어력
    public int resistanceUp;       // 저항력
    public int perceptionUp;       // 감각 (회피율)
    public int manaUp;             // 현재 마나
    public int manaRegenUp;        // 마나 재생량

    // 레벨과 경험치
    public int level = 1;          // 기본 레벨
    public float experience = 0f;  // 현재 경험치
    public float maxExperience = 100f; // 레벨업에 필요한 최대 경험치
}
