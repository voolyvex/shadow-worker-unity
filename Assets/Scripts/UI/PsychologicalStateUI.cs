using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShadowWorker.Core;

public class PsychologicalStateUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PersonalityProfile targetProfile;
    
    [Header("UI Elements")]
    [SerializeField] private Slider integrationSlider;
    [SerializeField] private Image resonanceIndicator;
    [SerializeField] private TextMeshProUGUI resonanceText;
    [SerializeField] private TextMeshProUGUI enneagramText;
    
    [Header("DSM Trait Sliders")]
    [SerializeField] private Slider anxietySlider;
    [SerializeField] private Slider depressionSlider;
    [SerializeField] private Slider dissociationSlider;
    [SerializeField] private Slider traumaSlider;
    [SerializeField] private Slider psychosisSlider;
    [SerializeField] private Slider personalitySlider;
    
    [Header("Colors")]
    [SerializeField] private Color dissonantColor = Color.red;
    [SerializeField] private Color neutralColor = Color.yellow;
    [SerializeField] private Color resonantColor = Color.green;
    [SerializeField] private Color harmoniousColor = Color.blue;

    private void Update()
    {
        if (targetProfile == null) return;
        
        UpdateIntegrationUI();
        UpdateResonanceUI();
        UpdateDSMTraitsUI();
        UpdateEnneagramUI();
    }

    private void UpdateIntegrationUI()
    {
        if (integrationSlider != null)
        {
            integrationSlider.value = targetProfile.IntegrationLevel;
        }
    }

    private void UpdateResonanceUI()
    {
        if (resonanceIndicator != null)
        {
            resonanceIndicator.color = targetProfile.CurrentResonance switch
            {
                ResonanceState.Dissonant => dissonantColor,
                ResonanceState.Neutral => neutralColor,
                ResonanceState.Resonant => resonantColor,
                ResonanceState.Harmonious => harmoniousColor,
                _ => Color.white
            };
        }

        if (resonanceText != null)
        {
            resonanceText.text = targetProfile.CurrentResonance.ToString();
        }
    }

    private void UpdateDSMTraitsUI()
    {
        var traits = targetProfile.DSMTraits;
        
        if (anxietySlider != null) anxietySlider.value = traits.anxiety;
        if (depressionSlider != null) depressionSlider.value = traits.depression;
        if (dissociationSlider != null) dissociationSlider.value = traits.dissociation;
        if (traumaSlider != null) traumaSlider.value = traits.trauma;
        if (psychosisSlider != null) psychosisSlider.value = traits.psychosis;
        if (personalitySlider != null) personalitySlider.value = traits.personality;
    }

    private void UpdateEnneagramUI()
    {
        if (enneagramText != null)
        {
            enneagramText.text = $"Type {(int)targetProfile.EnneagramType}: {targetProfile.EnneagramType}";
        }
    }

    public void SetTarget(PersonalityProfile newTarget)
    {
        targetProfile = newTarget;
    }

    // Optional: Methods to handle UI interactions
    public void OnIntegrationSliderChanged(float value)
    {
        if (targetProfile != null)
        {
            targetProfile.ModifyIntegrationLevel(value - targetProfile.IntegrationLevel);
        }
    }

    public void OnDSMTraitSliderChanged(string traitName, float value)
    {
        if (targetProfile != null)
        {
            targetProfile.ModifyDSMTrait(traitName, value - GetCurrentTraitValue(traitName));
        }
    }

    private float GetCurrentTraitValue(string traitName)
    {
        var traits = targetProfile.DSMTraits;
        return traitName.ToLower() switch
        {
            "anxiety" => traits.anxiety,
            "depression" => traits.depression,
            "dissociation" => traits.dissociation,
            "trauma" => traits.trauma,
            "psychosis" => traits.psychosis,
            "personality" => traits.personality,
            _ => 0f
        };
    }
} 