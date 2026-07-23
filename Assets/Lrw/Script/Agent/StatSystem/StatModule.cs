using System.Collections.Generic;
using System.Linq;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Lrw.Script.Agent.StatSystem
{
    public class StatModule : Module, IStatModule
    {
        [SerializeField] private StatOverride[] statOverride;

        private Dictionary<StatDataSo,Stat> _stats = new();

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _stats = statOverride.Where(x => x != null && x.StatSo != null)
                .ToDictionary(x => x.StatSo,x => new Stat(x.StatSo, x.BaseValue));
        }

        public Stat GetStat(StatDataSo statSo)
        {
            if(statSo == null) return null;
            return _stats.GetValueOrDefault(statSo);
        }

        public bool TryGetStat(StatDataSo statSo, out Stat stat)
        {
            stat = GetStat(statSo);
            return stat != null;
        }
        
        private void OnValidate()
        {
            if (statOverride == null) return;
            
            HashSet<StatDataSo> hash = new();
            foreach (StatOverride statData in statOverride)
            {
                if (statData == null || statData.StatSo == null) continue;
                if (hash.Contains(statData.StatSo))
                {
                    Debug.LogError("동일한 스텟이 들어가 있습니다.");
                    break;
                }
                hash.Add(statData.StatSo);
            }
        }
        
        
        
    }
}