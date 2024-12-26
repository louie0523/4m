using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero_slot : MonoBehaviour
{
    public string sceneToLoad;
    public Button start;

    public GameObject[] HerosSlot;
    public CharacterStatsSO[] heroesStats; // 캐릭터 데이터 배열

    public TMP_Text nameText;
    public TMP_Text explainText;

    public Slider healthSlider;
    public Slider attackSlider;
    public Slider abilityPowerSlider;
    public Slider defenseSlider;
    public Slider resistanceSlider;
    public Slider perceptionSlider;
    public Slider manaSlider;
    public Slider manaRegenSlider;

    private int slot;

    void Start()
    {
        start.onClick.AddListener(OnButtonClick);
        ShowSelectedSlot();
    }

    void OnButtonClick()
    {
        if (CharacterManager.Instance != null)
        {
            if (heroesStats[slot] != null)
            {
                Debug.Log("Setting Player Character");
                CharacterManager.Instance.characters[0] = heroesStats[slot];
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("Selected hero is null!");
            }
        }
        else
        {
            Debug.LogError("CharacterManager.Instance is null!");
        }
    }

    void OnDestroy()
    {
        if (start != null)
        {
            start.onClick.RemoveListener(OnButtonClick);
        }
    }

    void Slot_reset()
    {
        foreach (GameObject obj in HerosSlot)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    void ShowSelectedSlot()
    {
        Slot_reset();

        if (slot >= 0 && slot < HerosSlot.Length)
        {
            if (HerosSlot[slot] != null)
            {
                // 선택된 HeroSlot과 모든 하위 오브젝트 활성화
                HerosSlot[slot].SetActive(true);
                foreach (Transform child in HerosSlot[slot].transform)
                {
                    child.gameObject.SetActive(true);
                }
            }

            CharacterStatsSO selectedHero = heroesStats[slot]; // 선택된 캐릭터

            nameText.text = selectedHero.characterName;  // 캐릭터 이름
            explainText.text = selectedHero.description; // 캐릭터 설명

            // 비율 계산 후 슬라이더 값 설정
            SetStatSliders(selectedHero);
        }
    }

    void SetStatSliders(CharacterStatsSO selectedHero)
    {
        // 각 스탯의 최대값을 기준으로 비율 계산
        float maxHealth = 1000f;
        float maxAttack = 100f;
        float maxAbilityPower = 150f;
        float maxDefense = 100f;
        float maxResistance = 100f;
        float maxPerception = 150f;
        float maxMana = 200f;
        float maxManaRegen = 20f;

        // 선택된 캐릭터의 스탯을 슬라이더에 반영
        healthSlider.value = selectedHero.health / maxHealth;
        attackSlider.value = selectedHero.attack / maxAttack;
        abilityPowerSlider.value = selectedHero.abilityPower / maxAbilityPower;
        defenseSlider.value = selectedHero.defense / maxDefense;
        resistanceSlider.value = selectedHero.resistance / maxResistance;
        perceptionSlider.value = selectedHero.perception / maxPerception;
        manaSlider.value = selectedHero.mana / maxMana;
        manaRegenSlider.value = selectedHero.manaRegen / maxManaRegen;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slot = Mathf.Max(0, slot - 1);
            ShowSelectedSlot();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            slot = Mathf.Min(HerosSlot.Length - 1, slot + 1);
            ShowSelectedSlot();
        }
    }
}
