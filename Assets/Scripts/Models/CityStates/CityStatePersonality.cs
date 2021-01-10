using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CityStatePersonality
{
    private List<Axis> _axies;

    public int AxisCount => _axies.Count();
    public float RateSum => _axies.Sum(axis => axis.Rate);

    public Axis Worshiper { get; private set; }
    public Axis Trading { get; private set; }
    public Axis Rading { get; private set; }

    /// <summary>
    /// Personality to sink points into when event curses done and to not confuse player by all description changing.
    /// High common folk rate will be more susceptible to change.
    /// </summary>
    public Axis CommonFolk { get; private set; }

    public CityStatePersonality()
    {
        Worshiper = new Axis(this);
        Trading = new Axis(this);
        Rading = new Axis(this);
        CommonFolk = new Axis(this);

        _axies = new List<Axis>
        {
            Worshiper,
            Trading,
            Rading,
            CommonFolk
        };

        foreach(var axis in _axies)
        {
            axis.InitRate(_axies.Count);
        }

        Validate();
    }

    public void Validate()
    {
        var sum = _axies.Sum(axis => axis.Rate);

        if (sum.GetRounded() != 1f)
        {
            Debug.LogError($"Personality sum is not 1f. Found {sum}");
        }
    }

    public class Axis
    {
        private CityStatePersonality _parent;

        /// <summary>
        /// Should be between 0 and 1.
        /// </summary>
        public float Rate { get; private set; }

        public Axis(CityStatePersonality parent)
        {
            _parent = parent;
        }

        public void AddFromCommonFolk(float add)
        {
            add = add.GetMinMax(-Rate, _parent.CommonFolk.Rate);
            Rate += add;
            _parent.CommonFolk.Rate -= add;

            _parent.Validate();
        }

        public void AddHalfFromCommonFolk(float add)
        {
            AddFromCommonFolk(add * 0.5f);
            Add(add * 0.5f);
        }

        /// <summary>
        /// Can be negative to subtract
        /// </summary>
        public void Add(float add)
        {
            //Instead of 1f use 0.9f so that ratio would be left is reaching some dominant trait.
            var maxAdd = 0.9f - Rate;
            var maxSubtract = -Rate;
            add = add.GetMinMax(maxSubtract, maxAdd);

            var axies = _parent._axies.Where(axis => axis != this);

            //If add by 10 ( / 100)
            //this has 40 other have 40 and 20
            //it should increase to 50
            //other should be ~33.44444 an ~16.66666 ??
            //40 / 60 * 10 = 6.666
            //40 - 6.6666 = 33.4444
            var sumOfOther = 1f - Rate;

            Rate += add;
            Rate = Rate.GetMinMax(0f, 1f);

            foreach (var axis in axies)
            {
                if (sumOfOther == 0
                    && add < 0)
                {
                    //In case reducing axis that is 1 and other axies are 0
                    axis.Rate -= add / axies.Count();
                    axis.Rate = axis.Rate.GetMinMax(0f, 1f);
                }
                else
                {
                    //Default behaviour on almost all cases
                    axis.Rate -= axis.Rate / sumOfOther * add;
                    axis.Rate = axis.Rate.GetMinMax(0f, 1f);
                }
            }

            _parent.Validate();
        }

        public void InitRate(int axisCount)
        {
            Rate = 1f / axisCount;
        }
    }
}

