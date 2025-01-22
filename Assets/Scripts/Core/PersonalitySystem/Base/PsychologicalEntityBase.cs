using UnityEngine;
using System.Collections.Generic;

namespace ShadowWorker.Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PsychologicalEntityBase : MonoBehaviour, IPsychologicalEntity
    {
        [SerializeField] protected EnneagramType enneagramType;
        [SerializeField] protected DSMDimensions dsmTraits;
        [SerializeField] protected float baseIntegrationLevel = 0.5f;
        [SerializeField] protected float maxIntegrationLevel = 1.0f;
        [SerializeField] protected float minIntegrationLevel = 0.0f;
        
        protected ResonanceState currentResonance = ResonanceState.Neutral;
        protected float integrationLevel;
        protected HashSet<IConsciousnessField> activeFields = new HashSet<IConsciousnessField>();
        
        public EnneagramType EnneagramType => enneagramType;
        public ResonanceState CurrentResonance => currentResonance;
        public DSMDimensions DSMTraits => dsmTraits;
        public float IntegrationLevel => integrationLevel;

        protected virtual void Awake()
        {
            integrationLevel = baseIntegrationLevel;
        }

        protected virtual void Update()
        {
            UpdatePsychologicalState();
        }

        public virtual void UpdatePsychologicalState()
        {
            foreach (var field in activeFields)
            {
                if (field.IsEntityInRange(this))
                {
                    field.AffectEntity(this);
                }
            }
            
            UpdateResonanceState();
        }

        protected virtual void UpdateResonanceState()
        {
            if (integrationLevel < 0.25f)
                currentResonance = ResonanceState.Dissonant;
            else if (integrationLevel < 0.5f)
                currentResonance = ResonanceState.Neutral;
            else if (integrationLevel < 0.75f)
                currentResonance = ResonanceState.Resonant;
            else
                currentResonance = ResonanceState.Harmonious;
        }

        public virtual void ModifyIntegrationLevel(float delta)
        {
            integrationLevel = Mathf.Clamp(integrationLevel + delta, minIntegrationLevel, maxIntegrationLevel);
            UpdateResonanceState();
        }

        public virtual void ModifyDSMTrait(string traitName, float delta)
        {
            var traits = dsmTraits;
            switch (traitName.ToLower())
            {
                case "anxiety": traits.anxiety = Mathf.Clamp01(traits.anxiety + delta); break;
                case "depression": traits.depression = Mathf.Clamp01(traits.depression + delta); break;
                case "dissociation": traits.dissociation = Mathf.Clamp01(traits.dissociation + delta); break;
                case "trauma": traits.trauma = Mathf.Clamp01(traits.trauma + delta); break;
                case "psychosis": traits.psychosis = Mathf.Clamp01(traits.psychosis + delta); break;
                case "personality": traits.personality = Mathf.Clamp01(traits.personality + delta); break;
            }
            dsmTraits = traits;
        }

        public virtual void OnConsciousnessFieldEnter(IConsciousnessField field)
        {
            if (field != null)
            {
                activeFields.Add(field);
            }
        }

        public virtual void OnConsciousnessFieldExit(IConsciousnessField field)
        {
            if (field != null)
            {
                activeFields.Remove(field);
            }
        }

        protected virtual void OnDrawGizmos()
        {
            // Visualize psychological state
            Gizmos.color = currentResonance switch
            {
                ResonanceState.Dissonant => Color.red,
                ResonanceState.Neutral => Color.yellow,
                ResonanceState.Resonant => Color.green,
                ResonanceState.Harmonious => Color.blue,
                _ => Color.white
            };
            
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
} 