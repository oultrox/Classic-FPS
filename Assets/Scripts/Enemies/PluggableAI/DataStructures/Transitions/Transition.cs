
using Enemies.PluggableAI.DataStructures.Decisions;
using Enemies.PluggableAI.DataStructures.States;

[System.Serializable]
public class Transition
{
    public SM_Decision decision;
    public SM_State trueState;
    public SM_State falseState;
}
