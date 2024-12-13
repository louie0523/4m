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
    public float walkSpeed = 0.1f;
    public Vector2 walkScale = new Vector2(1.2f, 1.2f);
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

    public void StartDialogue(string speaker, string dialogue, int backgroundIndex, bool useWalkEffect = false)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        speakerNameText.text = speaker;
        speakerNameText.color = GetSpeakerColor(speaker);
        UpdateCharacterStates(speaker);

        if (backgroundIndex >= 0 && backgroundIndex < backgrounds.Length)
        {
            StartCoroutine(useWalkEffect ? WalkEffectTransition(backgrounds[backgroundIndex]) : FadeEffectTransition(backgrounds[backgroundIndex]));
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
        // 여기서 대사 후 진행될 다음 행동을 추가
        // 예를 들어, 대사 끝나고 새로운 대사를 보여준다거나, 선택지를 보여준다거나
        // 예: StartDialogue("주인공", "이제 어떻게 해야 하지?", 5, false);

        // 선택지가 있으면 그때 그때 처리해주고, 대사만 있으면 자동으로 이어서 진행할 수도 있습니다.
    }

    private IEnumerator FadeEffectTransition(Sprite newBackground)
    {
        yield return Fade(1);
        // backgroundImageObject가 GameObject이므로 Image 컴포넌트 가져오기
        Image bgImage = backgroundImageObject.GetComponent<Image>();
        bgImage.sprite = newBackground;  // 이미지 변경
        yield return Fade(0);
    }

    private IEnumerator WalkEffectTransition(Sprite newBackground)
    {
        Vector3 originalScale = backgroundImageObject.GetComponent<RectTransform>().localScale;

        yield return LerpScale(originalScale, walkScale);
        Image bgImage = backgroundImageObject.GetComponent<Image>();
        bgImage.sprite = newBackground;
        yield return LerpScale(walkScale, originalScale);
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

    private IEnumerator LerpScale(Vector3 from, Vector3 to)
    {
        for (float t = 0; t < 1; t += Time.deltaTime * walkSpeed)
        {
            backgroundImageObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    private Color GetSpeakerColor(string speaker)
    {
        return speakerColors.ContainsKey(speaker) ? speakerColors[speaker] : speakerColors["기본"];
    }

    private void UpdateCharacterStates(string activeCharacter)
    {
        foreach (var character in characterMap)
        {
            character.Value.SetActive(character.Key == activeCharacter);
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

    public void Show(string characterName, Vector3 position)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            character.SetActive(true);
            // UI 이미지 위치 변경: RectTransform을 사용하여 위치 설정
            character.characterImage.rectTransform.anchoredPosition = position;
        }
    }

    public void Hide(string characterName)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            character.SetActive(false);  // 캐릭터를 비활성화
        }
    }

    public void Move(string characterName, Vector3 targetPosition, float duration)
    {
        if (characterMap.TryGetValue(characterName, out CharacterUI character))
        {
            StartCoroutine(MoveCharacter(character, targetPosition, duration));
        }
    }

    private IEnumerator MoveCharacter(CharacterUI character, Vector3 targetPosition, float duration)
    {
        // 캐릭터의 RectTransform을 사용하여 위치를 부드럽게 이동
        RectTransform rectTransform = character.characterImage.rectTransform;
        Vector3 startPosition = rectTransform.anchoredPosition;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t / duration);
            yield return null;
        }

        // 최종 목표 위치로 설정
        rectTransform.anchoredPosition = targetPosition;
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
        // Active 상태에서의 색상 (225, 225, 225, 255)
        Color activeColor = new Color(225f / 255f, 225f / 255f, 225f / 255f, 1f);
        // 비활성 상태에서의 색상 (127, 127, 127, 225)
        Color inactiveColor = new Color(127f / 255f, 127f / 255f, 127f / 255f, 0.88f);

        // 캐릭터 이미지의 색상을 activeColor 또는 inactiveColor로 설정
        characterImage.color = isActive ? activeColor : inactiveColor;
        characterImage.gameObject.SetActive(isActive); // 실제 이미지 게임 오브젝트 활성화
        if (isActive)
        {
            characterImage.rectTransform.anchoredPosition = position; // 활성화된 경우 위치 설정
        }
    }
}
