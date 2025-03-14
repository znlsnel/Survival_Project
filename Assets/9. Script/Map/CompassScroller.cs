using UnityEngine;
using UnityEngine.UI;

public class CompassScroll : MonoBehaviour
{
    public RectTransform content; 
    public Transform player; 
    public float scrollSpeed;

    private float lastPlayerAngle;
    private float contentWidth;

    void Start()
    {
        lastPlayerAngle = player.eulerAngles.y;
        contentWidth = content.rect.width / 2; 
    }

    void Update()
    {
        float currentAngle = player.eulerAngles.y;
        float deltaAngle = Mathf.DeltaAngle(lastPlayerAngle, currentAngle);
        lastPlayerAngle = currentAngle;

        content.anchoredPosition += Vector2.left * deltaAngle * scrollSpeed;


        if (content.anchoredPosition.x <= -contentWidth)
        {
            content.anchoredPosition += new Vector2(contentWidth * 2, 0);
        }
        else if (content.anchoredPosition.x >= contentWidth)
        {
            content.anchoredPosition -= new Vector2(contentWidth * 2, 0);
        }
    }
}
