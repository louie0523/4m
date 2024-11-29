using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "GameData/Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;         // 스킬 이름
    public string description;       // 스킬 설명
    public int mpCost;               // MP 소모량
    public int cooldown;             // 쿨다운 시간
    public SkillType skillType;      // 스킬 종류
    public float baseEffectValue;    // 기본 효과 값 (예: 데미지, 회복량 등)

    public TargetType targetType;    // 단일/광역 대상 타입
    public EffectType effectType;    // 공격형/버프형/디버프형 등

    // 추가된 필드: 스킬 이펙트
    public GameObject effectPrefab;  // 이펙트 Prefab (예: 파이어볼 이펙트)

    // 계수 값 (각각의 캐릭터 스탯 기반으로 스킬 효과를 계산)
    public StatMultiplier effectMultiplier;  // 계수 설정 (공격력, 주문력 등)

    // 스킬 효과 계산 (캐릭터의 스탯을 기반으로)
    public float CalculateEffectValue(CharacterStatsSO characterStats)
    {
        float effectValue = baseEffectValue;

        // 스탯 계수 적용
        if (effectMultiplier != null)
        {
            effectValue *= (1 + effectMultiplier.attackMultiplier * characterStats.attack / 100f);
            effectValue *= (1 + effectMultiplier.abilityPowerMultiplier * characterStats.abilityPower / 100f);
            effectValue *= (1 + effectMultiplier.healthMultiplier * characterStats.health / 100f);
            effectValue *= (1 + effectMultiplier.defenseMultiplier * characterStats.defense / 100f);
            effectValue *= (1 + effectMultiplier.resistanceMultiplier * characterStats.resistance / 100f);
            effectValue *= (1 + effectMultiplier.perceptionMultiplier * characterStats.perception / 100f);
            effectValue *= (1 + effectMultiplier.manaMultiplier * characterStats.mana / 100f);
            effectValue *= (1 + effectMultiplier.manaRegenMultiplier * characterStats.manaRegen / 100f);
        }

        return effectValue;
    }

    // 스킬의 계수 설정을 위한 클래스
    [System.Serializable]
    public class StatMultiplier
    {
        public float attackMultiplier = 0f;       // 공격력 계수
        public float abilityPowerMultiplier = 0f; // 주문력 계수
        public float healthMultiplier = 0f;       // 체력 계수
        public float defenseMultiplier = 0f;      // 방어력 계수
        public float resistanceMultiplier = 0f;   // 저항력 계수
        public float perceptionMultiplier = 0f;   // 감각 계수 (회피율)
        public float manaMultiplier = 0f;         // 마나 계수
        public float manaRegenMultiplier = 0f;    // 마나 재생량 계수
    }

    public enum SkillType
    {
        PassiveGeneral,  // 일반 패시브
        PassiveUnique,   // 고유 패시브
        ActiveGeneral,   // 일반 기술
        ActiveUnique,    // 고유 기술
        UltimateSkill,   // 궁극 기술
        PhysicalAttack,  // 물리 공격 스킬
        MagicalAttack    // 마법 공격 스킬
    }

    public enum TargetType
    {
        SingleTarget,     // 단일 타겟
        MultiTarget       // 광역 효과 (AOE)
    }

    public enum EffectType
    {
        TrueAttack,       // 공격형
        Buff,             // 버프형
        Debuff,           // 디버프형
        Heal,             // 회복형
        PhysicalAttack,   // 물리 공격형
        MagicalAttack     // 마법 공격형
    }
}
