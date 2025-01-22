using UnityEngine;

namespace ShadowWorker.Core
{
    [AddComponentMenu("Shadow Worker/Personality System/Consciousness Field")]
    [RequireComponent(typeof(CircleCollider2D))]
    public class ConsciousnessField : MonoBehaviour, IConsciousnessField
    {
        [Header("Field Properties")]
        [SerializeField] private PerceptionLayer perceptionLayer;
        [SerializeField] private float baseIntensity = 0.5f;
        [SerializeField] private float baseRadius = 2f;
        [SerializeField] private float pulseFrequency = 1f;
        [SerializeField] private float pulseAmplitude = 0.2f;
        
        [Header("Effect Settings")]
        [SerializeField] private float integrationEffect = 0.1f;
        [SerializeField] private float anxietyEffect = 0.05f;
        [SerializeField] private float depressionEffect = 0.05f;
        [SerializeField] private float dissociationEffect = 0.05f;

        private CircleCollider2D fieldCollider;
        private float currentIntensity;
        private float elapsedTime;

        public PerceptionLayer Layer => perceptionLayer;
        public float Intensity => currentIntensity;
        public float Radius => fieldCollider.radius;

        private void Awake()
        {
            fieldCollider = GetComponent<CircleCollider2D>();
            fieldCollider.isTrigger = true;
            fieldCollider.radius = baseRadius;
            currentIntensity = baseIntensity;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            
            // Update intensity with pulsing effect
            float pulseEffect = Mathf.Sin(elapsedTime * pulseFrequency * 2f * Mathf.PI) * pulseAmplitude;
            currentIntensity = Mathf.Clamp01(baseIntensity + pulseEffect);
            
            // Update field radius based on intensity
            fieldCollider.radius = baseRadius * (1f + pulseEffect);
        }

        public void AffectEntity(IPsychologicalEntity entity)
        {
            if (entity == null) return;

            float distanceFactor = GetDistanceFactor(entity);
            float effectStrength = currentIntensity * distanceFactor;

            // Apply integration effect
            entity.ModifyIntegrationLevel(integrationEffect * effectStrength * Time.deltaTime);

            // Apply DSM trait effects
            entity.ModifyDSMTrait("anxiety", anxietyEffect * effectStrength * Time.deltaTime);
            entity.ModifyDSMTrait("depression", depressionEffect * effectStrength * Time.deltaTime);
            entity.ModifyDSMTrait("dissociation", dissociationEffect * effectStrength * Time.deltaTime);
        }

        public bool IsEntityInRange(IPsychologicalEntity entity)
        {
            if (entity == null) return false;
            
            var entityObj = (entity as MonoBehaviour)?.gameObject;
            if (entityObj == null) return false;

            float distance = Vector2.Distance(transform.position, entityObj.transform.position);
            return distance <= fieldCollider.radius;
        }

        public void ModifyIntensity(float delta)
        {
            baseIntensity = Mathf.Clamp01(baseIntensity + delta);
        }

        private float GetDistanceFactor(IPsychologicalEntity entity)
        {
            var entityObj = (entity as MonoBehaviour)?.gameObject;
            if (entityObj == null) return 0f;

            float distance = Vector2.Distance(transform.position, entityObj.transform.position);
            return 1f - Mathf.Clamp01(distance / fieldCollider.radius);
        }

        private void OnDrawGizmos()
        {
            // Visualize field
            Color fieldColor = perceptionLayer switch
            {
                PerceptionLayer.Physical => new Color(1f, 0f, 0f, 0.2f),
                PerceptionLayer.Emotional => new Color(0f, 1f, 0f, 0.2f),
                PerceptionLayer.Mental => new Color(0f, 0f, 1f, 0.2f),
                PerceptionLayer.Spiritual => new Color(1f, 1f, 0f, 0.2f),
                _ => new Color(1f, 1f, 1f, 0.2f)
            };

            Gizmos.color = fieldColor;
            Gizmos.DrawWireSphere(transform.position, baseRadius);
            
            // Draw intensity indicator
            Gizmos.color = Color.white;
            Vector3 intensityLine = transform.up * (baseRadius * currentIntensity);
            Gizmos.DrawLine(transform.position, transform.position + intensityLine);
        }
    }
} 