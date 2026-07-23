using System.Collections.Generic;
using System.Linq;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Lrw.Script.StatSystem
{
    public class StatModule : Module, IStatModule
    {
        [SerializeField] private StatOverride[] statOverride;

        private Dictionary<StatDataSo,Stat> _stats = new();

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _stats = statOverride.ToDictionary(x => x.statSo,x => new Stat(x.statSo, x.value));
        }

        public Stat GetStat(StatDataSo statSo)
        {
            if(statSo == null) return null;
            return _stats.GetValueOrDefault(statSo);
        }
        
        private void OnValidate()
        {
            HashSet<StatDataSo> hash = new();

            foreach (StatOverride statData in statOverride)
            {
                if(statData == null) continue;
                if (hash.Contains(statData.statSo))
                {
                    Debug.LogError("동일한 스텟이 들어가 있습니다.");
                    break;
                }
                hash.Add(statData.statSo);
            }
        }
        
        
        
    }
}