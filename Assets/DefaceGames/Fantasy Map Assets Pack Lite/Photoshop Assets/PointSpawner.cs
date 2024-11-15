using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public Sprite pointSprite;  // �� ����� ��������Ʈ
    public float pointSpacing = 0.5f;  // �� ����
    public float lineLength = 5f;  // �� �簢�� ������Ʈ�� ����

    void Start()
    {
        SpawnPoints();
    }

    void SpawnPoints()
    {
        // �簢���� ���̿� ���� ���� ���� ���
        int pointCount = Mathf.FloorToInt(lineLength / pointSpacing);

        // ���� ������ ��ġ ���
        for (int i = 0; i < pointCount; i++)
        {
            float xPosition = transform.position.x + i * pointSpacing;  // ���� X ��ġ
            Vector3 pointPosition = new Vector3(xPosition, transform.position.y, transform.position.z);

            // ���� �����ϰ� SpriteRenderer �߰�
            GameObject point = new GameObject("Point_" + i);
            point.transform.position = pointPosition;
            point.transform.parent = transform;  // ���� �θ� ��ü�� �ٿ���

            // SpriteRenderer �߰��ϰ� �� ��������Ʈ ����
            SpriteRenderer spriteRenderer = point.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = pointSprite;
            spriteRenderer.sortingLayerName = "Default";  // ������ ���� ���� (�ʿ�� ����)
        }
    }
}
