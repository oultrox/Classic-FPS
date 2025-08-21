namespace Enemies.PluggableAI.DataStructures.Decisions
{
    public interface IStateController
    {
        T GetComponent<T>();
    }
}