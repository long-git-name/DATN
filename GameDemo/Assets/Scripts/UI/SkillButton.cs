using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillCDOverlay;
    [SerializeField] private Image timeTriggerFilled;
    [SerializeField] private Text amountTxt;
    [SerializeField] private Text cdTxt;
    [SerializeField] private Button btnComp;

    private SkillType skillType;
    private SkillController skillController;
    private int currentAmount;

    #region EVENTS

    private void RegisterEvents()
    {
        if(skillController == null) return;

        skillController.OnCooldown.AddListener(UpdateCooldown);
        skillController.OnSkillUpdate.AddListener(UpdateTimeTrigger);
        skillController.OnCDStop.AddListener(UpdateUI);
    }
    private void UnregisterEvents()
    {
        if(skillController == null) return;
        
        skillController.OnCooldown.RemoveListener(UpdateCooldown);
        skillController.OnSkillUpdate.RemoveListener(UpdateTimeTrigger);
        skillController.OnCDStop.RemoveListener(UpdateUI);
    }

    #endregion

    public void Initialize(SkillType type)
    {
        skillType = type;
        skillController = SkillManager.Ins.GetSkillController(type);
        timeTriggerFilled.transform.parent.gameObject.SetActive(false);

        UpdateUI();

        if(btnComp != null)
        {
            btnComp.onClick.RemoveAllListeners();
            btnComp.onClick.AddListener(TriggerSkill);
        }
        RegisterEvents();
    }


    private void UpdateUI()
    {
        if(skillController == null) return;
        if(skillIcon != null)
        {
            skillIcon.sprite = skillController.skillStat.skillIcon;
        }

        UpdateAmountTxt();
        UpdateCooldown();
        UpdateTimeTrigger();

        bool canActive = currentAmount > 0 || skillController.IsCD;
        gameObject.SetActive(canActive);
    }

    private void UpdateAmountTxt()
    {
        currentAmount = SkillManager.Ins.GetSkillAmount(skillType);
        if(amountTxt)
        {
            amountTxt.text = $"x{currentAmount}";
        }
    }

    private void UpdateCooldown()
    {
        if(cdTxt)
        {
            cdTxt.text = skillController.CdTime.ToString("F1");
        }
        float cdProgress = skillController.cdProgress;

        if(skillCDOverlay)
        {
            skillCDOverlay.fillAmount = cdProgress;
            skillCDOverlay.gameObject.SetActive(skillController.IsCD);
        }
    }

    private void UpdateTimeTrigger()
    {
        if(timeTriggerFilled == null || skillController == null) return;
        float triggerProgress = skillController.triggerProgress;
        timeTriggerFilled.fillAmount = triggerProgress;
        timeTriggerFilled.transform.parent.gameObject.SetActive(skillController.IsTriggered);
    }

    private void TriggerSkill()
    {
        if(skillController == null) return;
        skillController.Trigger();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }
}
