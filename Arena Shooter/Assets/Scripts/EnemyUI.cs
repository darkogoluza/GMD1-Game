using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private List<Transform> targets = new();
    [SerializeField] private float activationDistance = 5f;
    [SerializeField] private float scaleSpeed = 5f;
    [SerializeField] private float verticalOffset = 3f;
    [SerializeField] private RectTransform uiElement;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI nameTextMeshProUGUI;
    [SerializeField] private Slider healthSlider;

    private Vector3 _visibleScale = new Vector3(0.005f, 0.005f, 0.005f);
    private Vector3 _hiddenScale = Vector3.zero;
    private float _maxHealth;

    void Start()
    {
        uiElement.localScale = _hiddenScale;
    }

    void Update()
    {
        float closestDistance = float.MaxValue;

        foreach (Transform t in targets)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < closestDistance)
                closestDistance = dist;
        }

        Vector3 targetScale = closestDistance <= activationDistance ? _visibleScale : _hiddenScale;
        uiElement.localScale = Vector3.Lerp(uiElement.localScale, targetScale, Time.deltaTime * scaleSpeed);
        uiElement.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        uiElement.position = transform.position + new Vector3(0, verticalOffset, 0);
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }

    public void SetName(string newName)
    {
        nameTextMeshProUGUI.text = newName;
    }

    public void SetMaxHealth(float health)
    {
        _maxHealth = health;
    }

    public void UpdateHealth(float health)
    {
        healthSlider.value = health / _maxHealth;
    }
}
