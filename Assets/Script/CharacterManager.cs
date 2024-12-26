using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    // 캐릭터 배열 (0번 인덱스에 플레이어 캐릭터, 이후 동료 캐릭터들)
    public CharacterStatsSO[] characters = new CharacterStatsSO[3];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);  // 씬 전환 후에도 객체 유지
    }

    // 캐릭터의 경험치 갱신 및 레벨업 처리
    public void GainExperience(int slot, float exp)
    {
        CharacterStatsSO character = characters[slot];
        character.experience += exp;

        // 경험치가 최대 경험치를 초과하면 레벨업
        while (character.experience >= character.maxExperience)
        {
            character.experience -= character.maxExperience;
            LevelUp(slot);
        }
    }

    // 레벨업 처리
    private void LevelUp(int slot)
    {
        CharacterStatsSO character = characters[slot];
        character.level++;

        // 캐릭터의 성장 값만큼 스탯 증가
        character.health += character.healthUp;
        character.attack += character.attackUp;
        character.abilityPower += character.abilityPowerUp;
        character.defense += character.defenseUp;
        character.resistance += character.resistanceUp;
        character.perception += character.perceptionUp;
        character.mana += character.manaUp;
        character.manaRegen += character.manaRegenUp;

        // 레벨업 후에는 최대 경험치를 갱신
        character.maxExperience = 100 * character.level;  // 레벨마다 증가하는 최대 경험치 예시
    }

    // 선택된 캐릭터 반환
    public CharacterStatsSO GetSelectedCharacter()
    {
        return characters[0];  // 0번 인덱스의 캐릭터 반환
    }

    public int GetLevel(int slot)
    {
        return characters[slot].level;
    }

    public float GetExperience(int slot)
    {
        return characters[slot].experience;
    }

    // 레벨업에 필요한 경험치 계산 (각 캐릭터마다 maxExperience 사용)
    private float GetExperienceToLevelUp(CharacterStatsSO character)
    {
        return character.maxExperience; // 각 캐릭터의 maxExperience 값 사용
    }
}
