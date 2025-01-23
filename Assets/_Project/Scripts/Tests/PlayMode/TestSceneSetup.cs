using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ShadowWorker.Core;

namespace ShadowWorker.Tests.PlayMode
{
    public class TestSceneSetup
    {
        [UnityTest]
        public IEnumerator TestManagersInitialization()
        {
            // Load test scene
            yield return new WaitForSeconds(0.1f);
            
            // Check if managers are initialized
            Assert.IsNotNull(GameManager.Instance, "GameManager not initialized");
            Assert.IsNotNull(AudioManager.Instance, "AudioManager not initialized");
            Assert.IsNotNull(InputManager.Instance, "InputManager not initialized");
        }
    }
} 