using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.IO.LowLevel.Unsafe;
using static UnityEditor.VersionControl.Asset;

[Serializable]
public class StateMachine 
{
    public IState CurrentState { get; private set; }

    private Dictionary<Type, IState> states;

    private event Action<IState> stateChanged;

    public StateMachine(Dictionary<Type, IState> stateList)
    {
        states = stateList;
    }


    // set the starting state
    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();


        // notify other objects that state has changed
        stateChanged?.Invoke(state);
    }


    // exit this state and enter another
    public void TransitionTo(Type nextState)
    {
        CurrentState?.Exit();
        CurrentState = states[nextState];
        CurrentState?.Enter();


        // notify other objects that state has changed
        stateChanged?.Invoke(CurrentState);
    }

    // allow the StateMachine to update this state
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void Addlistener(Action<IState> callback)
    {
        stateChanged += callback;
    }

    public void RemoveListener(Action<IState> callback)
    {
        stateChanged -= callback;
    }

    public void ClearListener()
    {
        stateChanged = null;
    }
}
