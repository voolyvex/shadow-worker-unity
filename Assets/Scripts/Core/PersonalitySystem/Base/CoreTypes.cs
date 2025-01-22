using UnityEngine;

namespace ShadowWorker.Core
{
    public enum EnneagramType
    {
        Reformer = 1,
        Helper = 2,
        Achiever = 3,
        Individualist = 4,
        Investigator = 5,
        Loyalist = 6,
        Enthusiast = 7,
        Challenger = 8,
        Peacemaker = 9
    }

    public enum ResonanceState
    {
        Dissonant,
        Neutral,
        Resonant,
        Harmonious
    }

    public enum PerceptionLayer
    {
        Physical,
        Emotional,
        Mental,
        Spiritual
    }

    [System.Serializable]
    public struct DSMDimensions
    {
        [Range(0f, 1f)] public float anxiety;
        [Range(0f, 1f)] public float depression;
        [Range(0f, 1f)] public float dissociation;
        [Range(0f, 1f)] public float trauma;
        [Range(0f, 1f)] public float psychosis;
        [Range(0f, 1f)] public float personality;
    }

    public interface IPsychologicalEntity
    {
        EnneagramType EnneagramType { get; }
        ResonanceState CurrentResonance { get; }
        DSMDimensions DSMTraits { get; }
        float IntegrationLevel { get; }
        
        void UpdatePsychologicalState();
        void ModifyIntegrationLevel(float delta);
        void ModifyDSMTrait(string traitName, float delta);
        void OnConsciousnessFieldEnter(IConsciousnessField field);
        void OnConsciousnessFieldExit(IConsciousnessField field);
    }

    public interface IConsciousnessField
    {
        PerceptionLayer Layer { get; }
        float Intensity { get; }
        float Radius { get; }
        
        void AffectEntity(IPsychologicalEntity entity);
        bool IsEntityInRange(IPsychologicalEntity entity);
        void ModifyIntensity(float delta);
    }
} 