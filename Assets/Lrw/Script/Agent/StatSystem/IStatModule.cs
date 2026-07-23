namespace Lrw.Script.Agent.StatSystem
{
    public interface IStatModule
    {
        Stat GetStat(StatDataSo statSo);
        bool TryGetStat(StatDataSo statSo, out Stat stat);
    }
}