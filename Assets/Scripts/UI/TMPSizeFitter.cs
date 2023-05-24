using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TMPSizeFitter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        private RectTransform _rectTransform;
        private float _preferredHeight;

        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _preferredHeight = textMeshProUGUI.preferredHeight;
        }

        private void SetHeight()
        {
            _preferredHeight = textMeshProUGUI.preferredHeight;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _preferredHeight + 200);
        }

        private void Update()
        {
            if (Math.Abs(_preferredHeight - textMeshProUGUI.preferredHeight) > 1e-3)
                SetHeight();
        }
    }
}