using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : Button
{
    public Skill Skill { get; set; }
    private TextMeshProUGUI _textMeshProUGUI;
    private GameObject _skillDescriptionFrame;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        _skillDescriptionFrame ??= GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
            .First(child => child.name == "SkillDescriptionFrame").gameObject;
        _skillDescriptionFrame.SetActive(true);
        _textMeshProUGUI ??= _skillDescriptionFrame.GetComponentInChildren<TextMeshProUGUI>();
        _textMeshProUGUI.text = Skill.description;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _skillDescriptionFrame ??= GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
            .First(child => child.name == "SkillDescriptionFrame").gameObject;
        _textMeshProUGUI ??= _skillDescriptionFrame.GetComponentInChildren<TextMeshProUGUI>();
        _textMeshProUGUI.text = "";
        _skillDescriptionFrame.SetActive(false);
    }
}