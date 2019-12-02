using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BowserStates;
public class BowserStatesMachine : MonoBehaviour
{
    private BowserBoss bowser;
    private StateName m_currentState = StateName.IDLE;
    private Dictionary<StateName,IState> m_states;
    public StateName beginState = StateName.IDLE;

    public float DelayBetweenAttacks = 2;
    float TimeSinceLastAttack = 0; 
    public float DelayBetweenJumpAttacks = 10;
    float TimeSinceLastJumpAttack = 0;

    public bool Attacking = false;
    public bool Moving = false;
    public bool Dead = false;

    public IState CurrentState()
    {
        return StateAt(m_currentState);
    }

    IState StateAt(StateName name)
    {
        if (m_states.ContainsKey(name))
        {
            return m_states[name];
        }
        else
        {
            return null;
        }
    }
    IState CreateState(StateName name)
    {
        switch (name)
        {
            case StateName.WALK:
                return new WalkState(name);
            case StateName.RUN:
                return new RunState(name);
            case StateName.JUMP:
                return new JumpState(name);
            case StateName.PUNCH:
                return new PunchState(name);
            case StateName.SWIPE:
                return new SwipeState(name);
            case StateName.ATTACK:
                return new AttackState(name);
            case StateName.DIE:
                return new DieState(name);
            case StateName.IDLE:
            default:
                return new IdleState(name);
        }
    }
    public bool ChangeState(StateName name)
    {
        if (m_currentState.Equals(name)) return false;
        IState curr = CurrentState();
        IState next_state = StateAt(name);
        if (next_state != null)
        {
            m_currentState = name;
            curr.Exit();
            next_state.Enter();
        }
        else
        {
            next_state = StateAt(StateName.IDLE);
            m_currentState = StateName.IDLE;
            curr.Exit();
            next_state.Enter();
        }
        return true;
    }

    void InitializeStateMachine(StateName state)
    {
        Dictionary<StateName,IState> states = new Dictionary<StateName, IState>();
        foreach (StateName name in (StateName[])Enum.GetValues(typeof(StateName)))
        {
            IState new_state = CreateState(name);
            new_state.ChangeState = ChangeState;
            new_state.bowser = bowser;
            new_state.StateMachine = () => { return this; };
            if (name.Equals(state)) m_currentState = name;
            states.Add(name, new_state);
        }
        m_states = states;
    }

    protected void Start()
    {
        bowser = GetComponent<BowserBoss>();
        InitializeStateMachine(beginState);
    }
    protected void Update()
    {
        TimeSinceLastAttack += Time.deltaTime;
        TimeSinceLastJumpAttack += Time.deltaTime;

        if (bowser.life.current <= 0 && !Dead)
        {
            ChangeState(StateName.DIE);
            Dead = true;
            return;
        }

        if (!Dead)
        {
            if (bowser.SeePlayer)
            {
                System.Random rnd = new System.Random();
                if (TimeSinceLastJumpAttack > DelayBetweenJumpAttacks && TimeSinceLastAttack > DelayBetweenAttacks
                    && bowser.ToDistanceFromPlayer && bowser.FarFromPlayer)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        ChangeState(StateName.ATTACK);
                    }
                    else
                    {
                        ChangeState(StateName.JUMP);
                    }
                    TimeSinceLastAttack = 0;
                    TimeSinceLastJumpAttack = 0;
                }
                else
                {
                    if (!bowser.FarFromPlayer && TimeSinceLastAttack > DelayBetweenAttacks && !Attacking)
                    {
                        TimeSinceLastAttack = 0;
                        if (rnd.Next(0, 2) == 0)
                        {
                            ChangeState(StateName.SWIPE);
                        }
                        else
                        {
                            ChangeState(StateName.PUNCH);
                        }
                    }
                    else if (!Moving && !Attacking)
                    {
                        switch (rnd.Next(0, 3))
                        {
                            case 1:
                                ChangeState(StateName.WALK);
                                break;
                            case 2:
                                ChangeState(StateName.RUN);
                                break;
                            default:
                                ChangeState(StateName.WALK);
                                break;
                        }
                    }
                }
            }
            else
            {
                ChangeState(StateName.IDLE);
            }
        }

        IState curr = CurrentState();
        if (curr != null)
        {
            curr.Update();
        }
    }
}