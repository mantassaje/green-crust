using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CityStatePersonalityTest
    {
        [Test]
        public void WhenAddingOnce_SumShouldBeOne()
        {
            // Given
            var sut = new CityStatePersonality();

            // When
            sut.Trading.Add(0.2f);

            // Then
            Assert.AreEqual(1f, sut.RateSum.GetRounded());
        }


        [Test]
        public void WhenAddingTooMuch_SumShouldBeOne()
        {
            // Given
            var sut = new CityStatePersonality();

            // When
            sut.Trading.Add(0.2f);
            sut.Trading.Add(0.2f);
            sut.Trading.Add(0.2f);
            sut.Trading.Add(0.2f);

            // Then
            Assert.AreEqual(1f, sut.RateSum.GetRounded());
        }

        [Test]
        public void WhenAddingTheOneWhoHasZero_SumShouldBeOne()
        {
            // Given
            var sut = new CityStatePersonality();

            // When
            sut.Trading.Add(1f);

            // Then
            Assert.AreEqual(1, sut.RateSum.GetRounded());
            Assert.AreEqual(0.9f, sut.Trading.Rate);
        }

        [Test]
        public void WhenRemoving_SumShouldBeOne()
        {
            // Given
            var sut = new CityStatePersonality();

            // When
            sut.Trading.Add(-0.2f);

            // Then
            Assert.AreEqual(1f, sut.RateSum.GetRounded());
        }

        [Test]
        public void WhenOverflow_SumShouldBeOne()
        {
            // Given
            var sut = new CityStatePersonality();

            // When
            sut.Rading.Add(1f);
            sut.Rading.Add(0.1f);
            sut.Rading.Add(-0.1f);

            // Then
            Assert.AreEqual(1f, sut.RateSum.GetRounded());
        }
    }
}
