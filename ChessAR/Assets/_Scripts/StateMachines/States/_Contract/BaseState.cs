public abstract class BaseState
{
    protected readonly IStateMachine stateMachine;

    protected BaseState(IStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void InitializeState()
    {
    }

    public virtual void UpdateState()
    {
    }
    public virtual void FixedUpdateState()
    {
    }

    public virtual void DisposeState()
    {
    }
}
