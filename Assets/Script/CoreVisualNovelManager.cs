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
    public Image backgroundImage;
    public Image fadeOverlay; // 검은 페이드 효과
    public GameObject choicePanel;
    public Button[] choiceButtons;

    [Header("Character Settings")]
    public List<CharacterUI> characters; // 캐릭터 UI 목록
    public Image[] characterImages;  // 캐릭터 이미지 배열
    public float dimmedAlpha = 0.5f;  // 어두운 캐릭터의 투명도

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    [Header("Background Settings")]
    public float fadeDuration = 1f;
    public bool isWalkEffect = false;
    public float walkSpeed = 0.1f;
    public Vector2 walkScale = new Vector2(1.2f, 1.2f);
    public Sprite[] backgrounds;  // 여러 배경 이미지를 저장

    private Coroutine typingCoroutine;
    private bool skipTyping = false;
    private bool isTyping = false;  // 대사가 출력 중인지 확인하는 플래그

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            skipTyping = true;
        }
    }

    // 대화 시작
    public void StartDialogue(string speaker, string dialogue, int backgroundIndex, bool useWalkEffect = false)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // 화자 설정
        speakerNameText.text = speaker;
        speakerNameText.color = GetSpeakerColor(speaker);

        // 캐릭터 이미지 업데이트
        UpdateCharacterStates(speaker);

        // 배경 전환
        isWalkEffect = useWalkEffect;
        if (backgroundIndex >= 0 && backgroundIndex < backgrounds.Length)
        {
            if (useWalkEffect)
                StartCoroutine(WalkEffectTransition(backgrounds[backgroundIndex]));
            else
                StartCoroutine(FadeEffectTransition(backgrounds[backgroundIndex]));
        }

        // 대화 출력
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    // 대화 텍스트 출력
    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
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

        isTyping = false;
        skipTyping = false;
    }

    // 배경 전환: 페이드 효과
    private IEnumerator FadeEffectTransition(Sprite newBackground)
    {
        fadeOverlay.color = new Color(0, 0, 0, 1);
        yield return Fade(fadeOverlay, 0);
        backgroundImage.sprite = newBackground;
        yield return Fade(fadeOverlay, 1);
    }

    // 배경 전환: 걷는 효과
    private IEnumerator WalkEffectTransition(Sprite newBackground)
    {
        Vector3 originalScale = backgroundImage.rectTransform.localScale;

        for (float t = 0; t < 1; t += Time.deltaTime * walkSpeed)
        {
            backgroundImage.rectTransform.localScale = Vector3.Lerp(originalScale, walkScale, t);
            yield return null;
        }

        backgroundImage.sprite = newBackground;

        for (float t = 0; t < 1; t += Time.deltaTime * walkSpeed)
        {
            backgroundImage.rectTransform.localScale = Vector3.Lerp(walkScale, originalScale, t);
            yield return null;
        }
    }

    // 페이드 애니메이션
    private IEnumerator Fade(Image overlay, float targetAlpha)
    {
        float startAlpha = overlay.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            overlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        overlay.color = new Color(0, 0, 0, targetAlpha);
    }

    // 화자 색상 설정
    private Color GetSpeakerColor(string speaker)
    {
        if (speaker == "특정 인물") return Color.yellow;
        return Color.white;
    }

    // 캐릭터 상태 업데이트
    private void UpdateCharacterStates(string activeCharacter)
    {
        foreach (var character in characters)
        {
            if (character.characterName == activeCharacter)
            {
                character.SetActive(true);
            }
            else
            {
                character.SetActive(false);
            }
        }
    }

    // 캐릭터 하이라이트 설정
    public void SetCharacterHighlight(int characterIndex)
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            Color color = characterImages[i].color;
            color.a = (i == characterIndex) ? 1f : dimmedAlpha;
            characterImages[i].color = color;
        }
    }

    // 선택지 표시
    public void ShowChoices(string[] choices, System.Action<int> onChoiceSelected)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                int index = i;
                choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(index, onChoiceSelected));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceClicked(int index, System.Action<int> onChoiceSelected)
    {
        choicePanel.SetActive(false);
        onChoiceSelected?.Invoke(index);
    }

    // 전투 씬으로 이동
    public void GoToBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
}

[System.Serializable]
public class CharacterUI
{
    public string characterName;
    public Image characterImage;
    public Vector3 position; // 캐릭터 위치
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0, 0, 0, 0.5f); // 어두운 상태 색상

    public void SetActive(bool isActive)
    {
        characterImage.color = isActive ? activeColor : inactiveColor;
        characterImage.rectTransform.anchoredPosition = position;
    }
}
