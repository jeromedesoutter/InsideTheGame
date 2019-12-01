using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BowserStates
{
    public enum StateName
    {
        IDLE, WALK, RUN, JUMP, PUNCH, SWIPE, ATTACK, DIE
    };
    public abstract class IState
    {
        public BowserBoss bowser;

        public StateName name;
        public Func<StateName, bool> ChangeState;
        public Func<BowserStatesMachine> StateMachine;

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        protected IState(StateName name) { this.name = name; }
    }
    public class IdleState : IState
    {
        bool breathing = false;
        public IdleState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = false;
            StateMachine().Moving = false;
            breathing = false;
            bowser.animator.SetBool("walking", false);
            bowser.animator.SetBool("running", false);
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            if (bowser.life.current < bowser.life.max / 3 && !breathing)
            {
                bowser.animator.SetBool("breathing", true);
                breathing = true;
            }
            else if(bowser.life.current > bowser.life.max / 3 && breathing)
            {
                bowser.animator.SetBool("breathing", false);
                breathing = false;
            }
        }
    }
    
    public class WalkState : IState
    {
        public WalkState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = false;
            StateMachine().Moving = true;
            bowser.IsRunning = false;
            bowser.animator.SetBool("walking", true);
            bowser.animator.SetBool("running", false);
        }

        public override void Exit()
        {
            StateMachine().Moving = false;
        }

        public override void Update()
        {
            if (bowser.SeePlayer && bowser.FarFromPlayer)
            {
                bowser.Move();
            }
            if((new System.Random()).Next(0,200) == 0)
            {
                StateMachine().Moving = false;
            }
        }
    }

    public class RunState : IState
    {
        public RunState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = false;
            StateMachine().Moving = true;
            bowser.IsRunning = true;
            bowser.animator.SetBool("running", true);
        }

        public override void Exit()
        {
            StateMachine().Moving = false;
        }

        public override void Update()
        {
            if (bowser.SeePlayer && bowser.FarFromPlayer)
            {
                bowser.Move();
            }
            if ((new System.Random()).Next(0, 200) == 0)
            {
                StateMachine().Moving = false;
            }
        }
    }

    public class JumpState : IState
    {
        public JumpState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = false;
            StateMachine().Moving = true;
            bowser.animator.SetTrigger("jump");
        }

        public override void Exit()
        {
            bowser.animator.SetTrigger("isGrounded");
        }

        public override void Update()
        {
            if(bowser.animator.GetCurrentAnimatorStateInfo(0).length >
            bowser.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                StateMachine().Moving = false;
                ChangeState(StateName.IDLE);
            }
        }
    }

    public class PunchState : IState
    {
        public PunchState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = true;
            bowser.animator.SetTrigger("punch");
        }

        public override void Exit()
        {
            bowser.animator.SetTrigger("isGrounded");
        }

        public override void Update()
        {
            if (bowser.animator.GetCurrentAnimatorStateInfo(0).length >
            bowser.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                StateMachine().Attacking = false;
                ChangeState(StateName.IDLE);
            }
        }
    }

    public class SwipeState : IState
    {
        public SwipeState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = true;
            bowser.animator.SetTrigger("swipe");
        }

        public override void Exit()
        {
            bowser.animator.SetTrigger("isGrounded");
        }

        public override void Update()
        {
            if (bowser.animator.GetCurrentAnimatorStateInfo(0).length >
            bowser.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                StateMachine().Attacking = false;
                ChangeState(StateName.IDLE);
            }
        }
    }

    public class AttackState : IState
    {
        public AttackState(StateName name) : base(name) { }
        public override void Enter()
        {
            StateMachine().Attacking = true;
            StateMachine().Moving = true;
            bowser.animator.SetTrigger("jumpattack");
        }

        public override void Exit()
        {
            bowser.animator.SetTrigger("isGrounded");
        }

        public override void Update()
        {
            if (bowser.animator.GetCurrentAnimatorStateInfo(0).length >
            bowser.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                StateMachine().Attacking = false;
                StateMachine().Moving = false;
                ChangeState(StateName.IDLE);
            }
        }
    }

    public class DieState : IState
    {
        public DieState(StateName name) : base(name) { }
        public override void Enter()
        {
            bowser.animator.SetTrigger("die");
            bowser.animator.SetBool("dead",true);
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }

}