using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleAmount = 1.2f;
    public float duration = 0.2f;

    private void Start()
    {
        originalScale = transform.localScale;

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log(gameObject.name);
        LeanTween.scale(gameObject, originalScale * scaleAmount, duration);
    }

    private void OnMouseExit()
    {
        LeanTween.scale(gameObject, originalScale, duration);
    }

    private void OnClick()
    {
        Debug.Log("Button Clicked!");
    }
}