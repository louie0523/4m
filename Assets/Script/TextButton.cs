using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string sceneToLoad; // 이동할 씬 이름

    private TextMeshProUGUI text;

    private void Awake()
    {
        // TextMeshProUGUI 컴포넌트 참조
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 텍스트 위로 올라갔을 때 색 변경
        text.color = Color.yellow;
        Debug.Log("마우스가 텍스트 위에 있습니다.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 텍스트에서 벗어났을 때 색 복원
        text.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 텍스트를 클릭했을 때 씬 이동
        SceneManager.LoadScene(sceneToLoad);
        Debug.Log("텍스트 클릭됨, 씬 이동 중...");
    }
}
