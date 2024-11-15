using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public Sprite pointSprite;  // 점 모양의 스프라이트
    public float pointSpacing = 0.5f;  // 점 간격
    public float lineLength = 5f;  // 긴 사각형 오브젝트의 길이

    void Start()
    {
        SpawnPoints();
    }

    void SpawnPoints()
    {
        // 사각형의 길이에 따라 점의 개수 계산
        int pointCount = Mathf.FloorToInt(lineLength / pointSpacing);

        // 점을 생성할 위치 계산
        for (int i = 0; i < pointCount; i++)
        {
            float xPosition = transform.position.x + i * pointSpacing;  // 점의 X 위치
            Vector3 pointPosition = new Vector3(xPosition, transform.position.y, transform.position.z);

            // 점을 생성하고 SpriteRenderer 추가
            GameObject point = new GameObject("Point_" + i);
            point.transform.position = pointPosition;
            point.transform.parent = transform;  // 점을 부모 객체에 붙여줌

            // SpriteRenderer 추가하고 점 스프라이트 설정
            SpriteRenderer spriteRenderer = point.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = pointSprite;
            spriteRenderer.sortingLayerName = "Default";  // 렌더링 순서 설정 (필요시 조정)
        }
    }
}
