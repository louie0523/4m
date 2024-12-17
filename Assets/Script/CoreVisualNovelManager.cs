using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoreVisualNovelManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject backgroundImageObject;  // Background image object (GameObject)
    public Image fadeOverlay;
    public GameObject choicePanel;
    public Button[] choiceButtons;

    [Header("Character Settings")]
    public List<CharacterUI> characters;
    public float dimmedAlpha = 0.5f;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    [Header("Background Settings")]
    public float fadeDuration = 1f;
    public Sprite[] backgrounds;

    private Coroutine typingCoroutine;
    private bool skipTyping = false;
    private bool canProceed = false;  // 대사 진행 여부 체크

    private Dictionary<string, Color> speakerColors;
    private Dictionary<string, CharacterUI> characterMap;

    void Awake()
    {
        speakerColors = new Dictionary<string, Color>
        {
            { "특정 인물", Color.yellow },
            { "기본", Color.white }
        };

        characterMap = new Dictionary<string, CharacterUI>();
        foreach (var character in characters)
        {
            characterMap[character.characterName] = character;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canProceed)  // 좌클릭을 감지
        {
            canProceed = false;  // 클릭 후 대사 진행을 잠시 멈춤
            StopAllCoroutines();  // 기존 코루틴을 중단하고 대사를 마무리

            // 다음 대사 또는 선택지를 보여주는 메서드 호출
            ShowNextDialogue();
        }
    }

    public void StartDialogue(string speaker, string dialogue, int backgroundIndex)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        speakerNameText.text = speaker;
        speakerNameText.color = GetSpeakerColor(speaker);
        UpdateCharacterStates(speaker);

        if (backgroundIndex >= 0 && backgroundIndex < backgrounds.Length)
        {
            BackGroundSet(backgrounds[backgroundIndex]);  // 배경 설정 메서드 호출
        }

        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            if (skipTyping)
            {
                dialogueText.text = dialogue;
                break;
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        skipTyping = false;
        canProceed = true;  // 대사가 끝나면 클릭을 통해 진행할 수 있게 설정
    }

    private void ShowNextDialogue()
    {
        // 여기에 대사 후 다음 행동을 추가할 수 있습니다.
        // 예: StartDialogue("주인공", "이제 어떻게 해야 하지?", 5);
    }

    private void BackGroundSet(Sprite newBackground)
    {
        StartCoroutine(FadeEffectTransition(newBackground));  // FadeEffectTransition만 사용
    }

    private IEnumerator FadeEffectTransition(Sprite newBackground)
    {
        yield return Fade(1);  // 화면 어둡게

        SpriteRenderer bgRenderer = backgroundImageObject.GetComponent<SpriteRenderer>();
        if (bgRenderer != null)
        {
            bgRenderer.sprite = newBackground;  // 배경 스프라이트 교체
        }
        else
        {
            Debug.LogError("SpriteRenderer가 backgroundImageObject에 없습니다!");
        }

        yield return Fade(0);  // 화면 밝게
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeOverlay.color.a;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(t / fadeDuration));
            fadeOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
    }

    private Color GetSpeakerColor(string speaker)
    {
        return speakerColors.ContainsKey(speaker) ? speakerColors[speaker] : speakerColors["기본"];
    }

    private void UpdateCharacterStates(string activeCharacter)
    {
        foreach (var character in characterMap)
        {
            bool isActive = character.Key == activeCharacter; // 현재 대사를 진행하는 캐릭터만 활성화
            character.Value.SetActive(isActive);  // 대사 중인 캐릭터는 밝은 색상, 아닌 캐릭터는 어두운 색상
        }
    }

    public void ShowChoices(string[] choices, System.Action<int> onChoiceSelected)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                var button = choiceButtons[i];
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { choicePanel.SetActive(false); onChoiceSelected?.Invoke(i); });
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void GoToBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void Hide(string characterName)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            // 캐릭터가 이미 비활성화 상태인 경우 아무 작업도 하지 않음
            if (character.characterImage.gameObject.activeSelf)
            {
                character.SetActive(false);  // 캐릭터를 비활성화
            }
        }
        else
        {
            Debug.LogError($"Character {characterName} not found in characterMap.");
        }
    }

    public void Show(string characterName, Vector3 position)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            // 캐릭터를 보여주기 전에 반드시 비활성화 처리
            character.SetActive(false);  // 먼저 비활성화 처리

            character.SetActive(true);   // 캐릭터 활성화
            StartCoroutine(MoveCharacter(character, position, 0.1f));  // 위치 이동
        }
        else
        {
            Debug.LogError($"Character {characterName} not found in characterMap.");
        }
    }

    public void Move(string characterName, Vector3 targetPosition, float duration)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            Debug.Log($"Moving {characterName} to {targetPosition} over {duration} seconds.");
            StartCoroutine(MoveCharacter(character, targetPosition, duration));
        }
        else
        {
            Debug.LogError($"Character {characterName} not found in characterMap.");
        }
    }

    private IEnumerator MoveCharacter(CharacterUI character, Vector3 targetPosition, float duration)
    {
        RectTransform rectTransform = character.characterImage.rectTransform;
        Vector3 startPosition = rectTransform.anchoredPosition;

        float elapsedTime = 0f; // 경과 시간 변수 추가

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition; // 최종 목표 위치 설정
    }
}

[System.Serializable]
public class CharacterUI
{
    public string characterName;
    public Image characterImage;
    public Vector2 position; // Vector2로 변경

    public void SetActive(bool isActive)
    {
        Color activeColor = new Color(225f / 255f, 225f / 255f, 225f / 255f, 1f);  // 밝은 색상 (활성화 상태)
        Color inactiveColor = new Color(127f / 255f, 127f / 255f, 127f / 255f, 0.5f);  // 어두운 색상 (비활성화 상태)

        characterImage.color = isActive ? activeColor : inactiveColor;
        //characterImage.gameObject.SetActive(isActive);  // 활성화/비활성화 처리

        // 캐릭터가 활성화된 상태일 때만 위치 설정
        if (isActive)
        {
            characterImage.rectTransform.anchoredPosition = position; // 활성화된 경우 위치 설정
        }
    }
}
