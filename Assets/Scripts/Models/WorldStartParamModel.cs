using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class WorldStartParamModel
    {
        public List<PlayerParamModel> Players { get; set; }
        public GoalSelection Goal { get; set; }
        public bool IsOnlneMode { get; set; }
        public bool IsSandbox { get; set; }
    }
}
