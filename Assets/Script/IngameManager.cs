using UnityEngine;
using TMPro;

public class IngameManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text abilityPowerText;
    public TMP_Text defenseText;
    public TMP_Text resistanceText;
    public TMP_Text perceptionText;
    public TMP_Text manaText;
    public TMP_Text manaRegenText;
    public TMP_Text levelText;
    public TMP_Text experienceText;

    void Start()
    {
        // CharacterManager에서 선택된 캐릭터 정보 가져오기
        CharacterStatsSO player = CharacterManager.Instance.GetSelectedCharacter();

        nameText.text = player.characterName;
        healthText.text = $"Health: {player.health}";
        attackText.text = $"Attack: {player.attack}";
        abilityPowerText.text = $"Ability Power: {player.abilityPower}";
        defenseText.text = $"Defense: {player.defense}";
        resistanceText.text = $"Resistance: {player.resistance}";
        perceptionText.text = $"Perception: {player.perception}";
        manaText.text = $"Mana: {player.mana}";
        manaRegenText.text = $"Mana Regen: {player.manaRegen}";
        levelText.text = $"Level: {player.level}";
        experienceText.text = $"Experience: {player.experience}/{player.maxExperience}";
    }
}
