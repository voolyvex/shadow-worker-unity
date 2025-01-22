using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ShadowWorker.Core;

public class PersonalitySystemTests
{
    private GameObject testObject;
    private PersonalityProfile profile;
    private ConsciousnessField field;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestEntity");
        testObject.AddComponent<CircleCollider2D>();
        profile = testObject.AddComponent<PersonalityProfile>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testObject);
        if (field != null)
        {
            Object.DestroyImmediate(field.gameObject);
        }
    }

    [Test]
    public void PersonalityProfile_InitializesWithDefaultValues()
    {
        Assert.AreEqual(ResonanceState.Neutral, profile.CurrentResonance);
        Assert.That(profile.IntegrationLevel, Is.InRange(0f, 1f));
    }

    [Test]
    public void PersonalityProfile_ModifyIntegrationLevel_ClampsValues()
    {
        profile.ModifyIntegrationLevel(2f);
        Assert.AreEqual(1f, profile.IntegrationLevel);

        profile.ModifyIntegrationLevel(-2f);
        Assert.AreEqual(0f, profile.IntegrationLevel);
    }

    [Test]
    public void PersonalityProfile_ModifyDSMTrait_ClampsValues()
    {
        profile.ModifyDSMTrait("anxiety", 2f);
        Assert.AreEqual(1f, profile.DSMTraits.anxiety);

        profile.ModifyDSMTrait("anxiety", -2f);
        Assert.AreEqual(0f, profile.DSMTraits.anxiety);
    }

    [Test]
    public void PersonalityProfile_UpdatesResonanceState()
    {
        // Test Dissonant state
        profile.ModifyIntegrationLevel(-1f);
        Assert.AreEqual(ResonanceState.Dissonant, profile.CurrentResonance);

        // Test Neutral state
        profile.ModifyIntegrationLevel(0.4f);
        Assert.AreEqual(ResonanceState.Neutral, profile.CurrentResonance);

        // Test Resonant state
        profile.ModifyIntegrationLevel(0.3f);
        Assert.AreEqual(ResonanceState.Resonant, profile.CurrentResonance);

        // Test Harmonious state
        profile.ModifyIntegrationLevel(0.3f);
        Assert.AreEqual(ResonanceState.Harmonious, profile.CurrentResonance);
    }

    [UnityTest]
    public IEnumerator ConsciousnessField_AffectsEntityInRange()
    {
        // Create consciousness field
        var fieldObject = new GameObject("TestField");
        field = fieldObject.AddComponent<ConsciousnessField>();
        fieldObject.AddComponent<CircleCollider2D>();

        // Position field and entity
        fieldObject.transform.position = Vector3.zero;
        testObject.transform.position = Vector3.right;

        // Initial values
        float initialIntegration = profile.IntegrationLevel;
        float initialAnxiety = profile.DSMTraits.anxiety;

        // Wait for physics and updates
        yield return new WaitForFixedUpdate();
        yield return null;

        // Verify field effects
        Assert.That(profile.IntegrationLevel, Is.Not.EqualTo(initialIntegration));
        Assert.That(profile.DSMTraits.anxiety, Is.Not.EqualTo(initialAnxiety));
    }

    [Test]
    public void ConsciousnessField_RangeCheck_WorksCorrectly()
    {
        // Create consciousness field
        var fieldObject = new GameObject("TestField");
        field = fieldObject.AddComponent<ConsciousnessField>();
        var collider = fieldObject.AddComponent<CircleCollider2D>();
        
        // Set field radius
        float testRadius = 2f;
        collider.radius = testRadius;

        // Test in range
        testObject.transform.position = new Vector3(testRadius - 0.1f, 0f, 0f);
        Assert.IsTrue(field.IsEntityInRange(profile));

        // Test out of range
        testObject.transform.position = new Vector3(testRadius + 0.1f, 0f, 0f);
        Assert.IsFalse(field.IsEntityInRange(profile));
    }
} 