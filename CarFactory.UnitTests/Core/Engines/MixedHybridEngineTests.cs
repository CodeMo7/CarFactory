using System;
using System.Collections;

using CarFactory;
using CarFactory.Core;
using CarFactory.Core.Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarFactory.Core.Engines.UnitTests
{
    [TestClass]
    public class MixedHybridEngineTests
    {
        /// <summary>
        /// Verifies that calling Increase once increments Speed from its default value (0) to 1.
        /// Arrange: new MixedHybridEngine with default state.
        /// Act: call Increase() once.
        /// Assert: Speed equals 1 and no exceptions thrown.
        /// </summary>
        [TestMethod]
        public void Increase_Once_SpeedIncrementsByOne()
        {
            // Arrange
            var engine = new MixedHybridEngine();

            // Act
            engine.Increase();

            // Assert
            Assert.AreEqual(1, engine.Speed, "Speed should be incremented by one after a single Increase call.");
        }

        /// <summary>
        /// Verifies that multiple calls to Increase accumulate the Speed correctly.
        /// Arrange: new MixedHybridEngine with default state.
        /// Act: call Increase() N times for a variety of N values.
        /// Assert: Speed equals the number of Increase calls for each N.
        /// </summary>
        [TestMethod]
        public void Increase_MultipleTimes_AccumulatesSpeed()
        {
            // We test several counts including boundary-like small and larger counts to exercise repeated increments.
            int[] countsToTest = new[] { 0, 1, 5, 100, 1000 };

            foreach (int calls in countsToTest)
            {
                // Arrange
                var engine = new MixedHybridEngine();

                // Act
                for (int i = 0; i < calls; i++)
                {
                    engine.Increase();
                }

                // Assert
                Assert.AreEqual(calls, engine.Speed, $"After {calls} Increase calls, Speed should be {calls}.");
            }
        }

        /// <summary>
        /// Verifies that UpdateSpeed assigns the provided integer value to the Speed property for a variety of integer edge cases.
        /// Input conditions: multiple carSpeed values including int.MinValue, negative, zero, boundary values around 50, and int.MaxValue.
        /// Expected result: MixedHybridEngine.Speed equals the provided carSpeed for each input and no exception is thrown.
        /// </summary>
        [TestMethod]
        public void UpdateSpeed_VariousIntegerValues_SetsSpeedExactly()
        {
            // Arrange
            var sut = new MixedHybridEngine();
            int[] testValues = new[]
            {
                int.MinValue,
                -1,
                0,
                49,   // just below threshold
                50,   // threshold
                100,
                int.MaxValue
            };

            // Act & Assert
            foreach (int value in testValues)
            {
                // Act
                sut.UpdateSpeed(value);

                // Assert
                Assert.AreEqual(value, sut.Speed, $"Expected Speed to equal provided carSpeed ({value}).");
            }
        }

        /// <summary>
        /// Ensures that calling UpdateSpeed with a value below the hybrid threshold (carSpeed &lt; 50) does not throw and documents inability to verify internal engine selection.
        /// Input conditions: carSpeed = 49 (below threshold).
        /// Expected result: no exception is thrown; test is marked inconclusive regarding which internal engine was updated because the internal engines are not injectable.
        /// </summary>
        [TestMethod]
        public void UpdateSpeed_BelowThreshold_NoException_InconclusiveForInternalEngineCall()
        {
            // Arrange
            var sut = new MixedHybridEngine();
            int carSpeed = 49;

            // Act
            sut.UpdateSpeed(carSpeed);

            // Assert basic observable behavior
            Assert.AreEqual(carSpeed, sut.Speed, "Speed should be set to the provided carSpeed.");

            // Cannot verify which internal engine (electricEngine) received UpdateSpeed because it is a private concrete instance created internally.
            // Marking as inconclusive and providing guidance to refactor for testability (inject dependencies).
            Assert.Inconclusive("MixedHybridEngine creates concrete internal engines; unable to verify that electronic engine's UpdateSpeed was invoked. Consider refactoring to inject dependencies to allow mocking and verification.");
        }

        /// <summary>
        /// Ensures that calling UpdateSpeed with a value at or above the hybrid threshold (carSpeed >= 50) does not throw and documents inability to verify internal engine selection.
        /// Input conditions: carSpeed = 50 (threshold) and carSpeed = int.MaxValue (extreme).
        /// Expected result: Speed is updated accordingly; test is marked inconclusive regarding which internal engine was updated because internal engines are private and not injectable.
        /// </summary>
        [TestMethod]
        public void UpdateSpeed_AtOrAboveThreshold_NoException_InconclusiveForInternalEngineCall()
        {
            // Arrange
            var sut = new MixedHybridEngine();

            // Test threshold value
            int thresholdSpeed = 50;
            sut.UpdateSpeed(thresholdSpeed);
            Assert.AreEqual(thresholdSpeed, sut.Speed, "Speed should be set to the provided threshold carSpeed.");

            // Test extreme high value
            int extremeSpeed = int.MaxValue;
            sut.UpdateSpeed(extremeSpeed);
            Assert.AreEqual(extremeSpeed, sut.Speed, "Speed should be set to the provided extreme carSpeed.");

            // Cannot verify that gasEngine.UpdateSpeed was invoked because gasEngine is a private concrete instance.
            Assert.Inconclusive("MixedHybridEngine creates concrete internal engines; unable to verify that gasoline engine's UpdateSpeed was invoked. Consider refactoring to inject dependencies to allow mocking and verification.");
        }

        /// <summary>
        /// Verifies that Decrease correctly decrements Speed only when Speed &gt; 0 and leaves Speed unchanged otherwise.
        /// Test covers boundary and extreme numeric values: int.MinValue, negative values, zero, small positives, and int.MaxValue.
        /// Expected: if initial Speed &gt; 0 then Speed decreases by 1; otherwise Speed remains the same.
        /// </summary>
        [TestMethod]
        public void Decrease_FromVariousSpeeds_ExpectedBehavior()
        {
            // Arrange & Act & Assert for multiple cases in one test to avoid redundancy.
            (int initial, int expected)[] cases = new[]
            {
                (int.MinValue, int.MinValue),                // extreme negative should remain unchanged
                (-1, -1),                                    // negative should remain unchanged
                (0, 0),                                      // zero should remain unchanged
                (1, 0),                                      // minimal positive decrements to zero
                (2, 1),                                      // small positive decrements by one
                (int.MaxValue, int.MaxValue - 1)             // large positive decrements by one
            };

            foreach (var (initial, expected) in cases)
            {
                // Arrange
                var engine = new MixedHybridEngine();

                // Use UpdateSpeed to set the internal Speed state prior to testing Decrease.
                // UpdateSpeed is part of the same class and provided in the source; it's used here only for setup.
                engine.UpdateSpeed(initial);

                // Act
                engine.Decrease();

                // Assert
                Assert.AreEqual(expected, engine.Speed, $"For initial Speed={initial}, expected Speed={expected} after Decrease.");
            }
        }
    }
}