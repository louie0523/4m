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
    public GameObject speakerNameFrame;
    public GameObject dialogueFrame;
    public GameObject backgroundImageObject;
    public Image fadeOverlay;  // 기존의 페이드 오버레이
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

    public GameObject overlayObject;  // 오버레이 오브젝트

    private Coroutine typingCoroutine;
    private bool skipTyping = false;
    public bool canProceed = false;  // 대사 진행 여부 체크

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
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            typingSpeed = 0.01f;  // Ctrl 누르면 빠르게 타이핑
        }
        else
        {
            typingSpeed = 0.05f;  // 기본 타이핑 속도
        }

        if (Input.GetMouseButtonDown(0) && canProceed)
        {
            canProceed = false;
            StopAllCoroutines();
            ShowNextDialogue();
        }
    }

    public void StartDialogue(string speaker, string dialogue)
    {
        // 대사 시작 전 대사창 비활성화
        dialogueFrame.SetActive(false);
        speakerNameFrame.SetActive(false);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (string.IsNullOrEmpty(speaker) || speaker == "나레이션")
        {
            speakerNameFrame.SetActive(false);
            speakerNameText.text = ""; // 발화자 이름 텍스트를 초기화
            UpdateCharacterStates(""); // 캐릭터 상태도 초기화
        }
        else
        {
            speakerNameText.text = speaker;
            speakerNameText.color = GetSpeakerColor(speaker);
            speakerNameFrame.SetActive(true); // 발화자 이름이 있는 경우만 활성화
            UpdateCharacterStates(speaker);
        }

        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        // 대사창을 활성화
        dialogueFrame.SetActive(true);
        if (!string.IsNullOrEmpty(speakerNameText.text))
        {
            speakerNameFrame.SetActive(true);
        } else
        {
            speakerNameFrame.SetActive(false);
        }

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
        // 삭제
    }

    public void SetBackground(int backgroundIndex)
    {
        if (backgroundIndex >= 0 && backgroundIndex < backgrounds.Length)
        {
            StartCoroutine(FadeBackground(backgrounds[backgroundIndex]));
        }
        else
        {
            Debug.LogError("Invalid background index: " + backgroundIndex);
        }
    }

    private IEnumerator FadeBackground(Sprite newBackground)
    {

        overlayObject.SetActive(true);
        yield return StartCoroutine(Fade(1, fadeOverlay, 1f));  // 기존 배경을 페이드 아웃

        // 2. 기존 배경을 변경
        SpriteRenderer bgRenderer = backgroundImageObject.GetComponent<SpriteRenderer>();
        if (bgRenderer != null)
        {
            bgRenderer.sprite = newBackground;  // 새로운 배경으로 변경
        }
        else
        {
            Debug.LogError("SpriteRenderer가 backgroundImageObject에 없습니다!");
        }

        // 3. 새로운 배경을 페이드 인
        yield return StartCoroutine(Fade(0, fadeOverlay, 0f));  // 새로운 배경을 페이드 인
        overlayObject.SetActive(false);
    }


    private IEnumerator Fade(float targetAlpha, Image fadeOverlay, float delay)
    {
        // Fade out/in 동안 배경과 UI 동시에 제어
        float startAlpha = fadeOverlay.color.a;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            // 알파 값 보간 (배경과 UI 요소들)
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(t / fadeDuration));

            // 배경 페이드 처리
            fadeOverlay.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        // 끝날 때는 정확한 알파 값 설정
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
            bool isActive = character.Key == activeCharacter;
            Color activeColor = new Color(225f / 255f, 225f / 255f, 225f / 255f, 1f);  // 밝은 색상
            Color inactiveColor = new Color(127f / 255f, 127f / 255f, 127f / 255f, 1f);  // 어두운 색상

            //캐릭터 색상 변경
            character.Value.characterImage.color = isActive ? activeColor : inactiveColor;
        }
    }

    public void ShowChoices(string[] choices, System.Action<int> onChoiceSelected)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                int choiceIndex = i;  //로컬 변수에 저장함
                var button = choiceButtons[i];
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    Debug.Log($"버튼 {choiceIndex} 클릭됨");
                    choicePanel.SetActive(false);
                    onChoiceSelected?.Invoke(choiceIndex);
                });
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
                character.SetActive(false);
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
            character.SetActive(true);
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
    public Vector2 position;

    public void SetActive(bool isActive)
    {
        Color activeColor = new Color(225f / 255f, 225f / 255f, 225f / 255f, 1f);  // 밝은 색상 (활성화 상태)
        Color inactiveColor = new Color(127f / 255f, 127f / 255f, 127f / 255f, 0.5f);  // 어두운 색상 (비활성화 상태)

        characterImage.color = isActive ? activeColor : inactiveColor;
        characterImage.gameObject.SetActive(isActive);  // 활성화/비활성화 처리**/

        // 캐릭터가 활성화된 상태일 때만 위치 설정
        if (isActive)
        {
            characterImage.rectTransform.anchoredPosition = position; // 활성화된 경우 위치 설정
        }
    }
}
