using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class ControllerFSM : MonoBehaviour
    {
        public StatesEnum CurrentState { get; private set; } = StatesEnum.NONE;

        Dictionary<StatesEnum, AbsStateFSM> states = new Dictionary<StatesEnum, AbsStateFSM>();

        public bool RegisterNewState(StatesEnum @enum, AbsStateFSM state, bool replaceExist = false)
        {
            if (states.ContainsKey(@enum))
                if (!replaceExist)
                    return false;
                else
                    states[@enum] = state;
            else
                states.Add(@enum, state);
            return true;
        }

        public bool SetState(StatesEnum state)
        {
            if (!CanChangeState(CurrentState, state))
            {
                this.LogError($"Cant move from [{CurrentState}] => [{state}]. If isn't error need register this translation.");
                return false;
            }

            this.LogWarning($"Change state [{CurrentState}] => [{state}].");

            if (states.ContainsKey(CurrentState))
                states[CurrentState]?.Exit();
            CurrentState = state;
            if (states.ContainsKey(CurrentState))
                states[CurrentState]?.Enter();
            else
                this.LogError($"Move to Unregistered state [{CurrentState}]. State wasn't call. State has to was registered.");

            return true;
        }

        bool CanChangeState(StatesEnum current, StatesEnum @new)
        {
            switch (current)
            {
                case StatesEnum.NONE:
                    return @new == StatesEnum.Initialization;
                case StatesEnum.Initialization:
                    return @new == StatesEnum.WaitUserInput;
                case StatesEnum.WaitUserInput:
                    return @new == StatesEnum.EnterUserInput
                        || @new == StatesEnum.WaitUserInput;
                case StatesEnum.EnterUserInput:
                    return @new == StatesEnum.EnterUserInput
                        || @new == StatesEnum.FindMatches
                        || @new == StatesEnum.WaitUserInput;
                case StatesEnum.FindMatches:
                    return @new == StatesEnum.WaitUserInput
                        || @new == StatesEnum.HaveNoOneTurn
                        || @new == StatesEnum.FindMatches;
                case StatesEnum.HaveNoOneTurn:
                    return @new == StatesEnum.WaitUserInput
                        || @new == StatesEnum.FindMatches;
                default:
                    this.LogError("Try to move to Unregistered state");
                    return false;
            }
        }
    }
}