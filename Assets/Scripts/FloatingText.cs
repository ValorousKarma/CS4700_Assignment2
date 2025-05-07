using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public TextMeshProUGUI txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public Transform target;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);

        if (target != null)
        {
            go.GetComponent<RectTransform>().position = target.position;
        }
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
            Hide();

        // Move the text upward over time
        go.GetComponent<RectTransform>().position += motion * Time.deltaTime;

    
    }
}
