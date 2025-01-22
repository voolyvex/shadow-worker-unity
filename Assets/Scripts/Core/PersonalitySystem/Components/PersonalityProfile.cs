using UnityEngine;

namespace ShadowWorker.Core
{
    [AddComponentMenu("Shadow Worker/Personality System/Personality Profile")]
    public class PersonalityProfile : PsychologicalEntityBase
    {
        [Header("Integration Decay")]
        [SerializeField] private float integrationDecayRate = 0.1f;
        [SerializeField] private float decayInterval = 1f;
        
        [Header("DSM Trait Decay")]
        [SerializeField] private float anxietyDecayRate = 0.05f;
        [SerializeField] private float depressionDecayRate = 0.03f;
        [SerializeField] private float dissociationDecayRate = 0.08f;
        [SerializeField] private float traumaDecayRate = 0.02f;
        [SerializeField] private float psychosisDecayRate = 0.07f;
        [SerializeField] private float personalityDecayRate = 0.04f;

        private float lastDecayTime;

        protected override void Awake()
        {
            base.Awake();
            lastDecayTime = Time.time;
        }

        protected override void Update()
        {
            base.Update();
            
            // Handle decay over time
            if (Time.time - lastDecayTime >= decayInterval)
            {
                UpdateIntegrationDecay();
                UpdateTraitDecay();
                lastDecayTime = Time.time;
            }
        }

        private void UpdateIntegrationDecay()
        {
            ModifyIntegrationLevel(-integrationDecayRate * decayInterval);
        }

        private void UpdateTraitDecay()
        {
            ModifyDSMTrait("anxiety", -anxietyDecayRate * decayInterval);
            ModifyDSMTrait("depression", -depressionDecayRate * decayInterval);
            ModifyDSMTrait("dissociation", -dissociationDecayRate * decayInterval);
            ModifyDSMTrait("trauma", -traumaDecayRate * decayInterval);
            ModifyDSMTrait("psychosis", -psychosisDecayRate * decayInterval);
            ModifyDSMTrait("personality", -personalityDecayRate * decayInterval);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var field = other.GetComponent<IConsciousnessField>();
            if (field != null)
            {
                OnConsciousnessFieldEnter(field);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var field = other.GetComponent<IConsciousnessField>();
            if (field != null)
            {
                OnConsciousnessFieldExit(field);
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            // Additional visualization for personality profile
            var position = transform.position;
            
            // Draw DSM traits visualization
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Red for anxiety
            Gizmos.DrawWireSphere(position, dsmTraits.anxiety);
            
            Gizmos.color = new Color(0f, 0f, 1f, 0.3f); // Blue for depression
            Gizmos.DrawWireSphere(position, dsmTraits.depression);
            
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Yellow for dissociation
            Gizmos.DrawWireSphere(position, dsmTraits.dissociation);
        }
    }
} 