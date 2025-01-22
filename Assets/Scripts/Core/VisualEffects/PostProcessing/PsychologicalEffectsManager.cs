using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ShadowWorker.Core;

[RequireComponent(typeof(Volume))]
public class PsychologicalEffectsManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PersonalityProfile targetProfile;
    
    [Header("Effect Intensities")]
    [SerializeField] private float maxVignetteIntensity = 0.5f;
    [SerializeField] private float maxChromaticAberration = 0.5f;
    [SerializeField] private float maxBloom = 2f;
    [SerializeField] private float maxLensDistortion = 0.5f;
    
    [Header("Transition")]
    [SerializeField] private float effectTransitionSpeed = 2f;

    private Volume postProcessVolume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private LensDistortion lensDistortion;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        
        // Get or add post-processing effects
        if (!postProcessVolume.profile.TryGet(out vignette))
            vignette = postProcessVolume.profile.Add<Vignette>();
            
        if (!postProcessVolume.profile.TryGet(out chromaticAberration))
            chromaticAberration = postProcessVolume.profile.Add<ChromaticAberration>();
            
        if (!postProcessVolume.profile.TryGet(out bloom))
            bloom = postProcessVolume.profile.Add<Bloom>();
            
        if (!postProcessVolume.profile.TryGet(out lensDistortion))
            lensDistortion = postProcessVolume.profile.Add<LensDistortion>();
    }

    private void Update()
    {
        if (targetProfile == null) return;

        UpdateEffects();
    }

    private void UpdateEffects()
    {
        // Calculate base effect intensity from integration level
        float baseIntensity = 1f - targetProfile.IntegrationLevel;
        
        // Get psychological state factors
        var traits = targetProfile.DSMTraits;
        float anxietyFactor = traits.anxiety;
        float dissociationFactor = traits.dissociation;
        float psychosisFactor = traits.psychosis;

        // Update vignette based on anxiety
        float targetVignette = baseIntensity * maxVignetteIntensity * anxietyFactor;
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, Time.deltaTime * effectTransitionSpeed);

        // Update chromatic aberration based on dissociation
        float targetCA = baseIntensity * maxChromaticAberration * dissociationFactor;
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetCA, Time.deltaTime * effectTransitionSpeed);

        // Update bloom based on psychosis
        float targetBloom = baseIntensity * maxBloom * psychosisFactor;
        bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, targetBloom, Time.deltaTime * effectTransitionSpeed);

        // Update lens distortion based on overall psychological state
        float overallDistortion = (anxietyFactor + dissociationFactor + psychosisFactor) / 3f;
        float targetDistortion = baseIntensity * maxLensDistortion * overallDistortion;
        lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetDistortion, Time.deltaTime * effectTransitionSpeed);

        // Set color adjustments based on resonance state
        UpdateResonanceEffects();
    }

    private void UpdateResonanceEffects()
    {
        Color targetTint = targetProfile.CurrentResonance switch
        {
            ResonanceState.Dissonant => new Color(1f, 0.8f, 0.8f),
            ResonanceState.Neutral => Color.white,
            ResonanceState.Resonant => new Color(0.8f, 1f, 0.8f),
            ResonanceState.Harmonious => new Color(0.8f, 0.8f, 1f),
            _ => Color.white
        };

        // You can add additional effects based on resonance state here
    }

    public void SetTarget(PersonalityProfile newTarget)
    {
        targetProfile = newTarget;
    }
} 