using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Character {

    public PlayerScript Player;

    public float ProbToUpBlock;
    public float ProbToBottomBlock;
    public float ProbtoDodge;
    public float RecuperationTime;
    float StartTime;

    [Header("Times to Combos")]
    public float ClampedTimeA;
    public float ClampedTimeB;
    public float time2NextCombo;
    public float actualtime;

    float PupBlock;
    float PbottomBlock;
    float PDodge;

    //[Header("Validators")]
    //public bool InCombo;
    //public bool PunchFailed;

    public List<bool> Combos;

    // Start is called before the first frame update
    public override void LoadData() {
        base.LoadData();

        time2NextCombo = Random.Range(ClampedTimeA, ClampedTimeB);
        StartTime = 0;

        PupBlock = ProbToUpBlock;
        PbottomBlock = ProbToBottomBlock;
        PDodge = ProbtoDodge;
    }

    float BlockDir;
    public bool Blocking;
    float Block;

    protected override void FailingProcess()
    {
        string Trigger = "PunchFailed";

        if (info.Hard)
            Trigger += "H";

        if (info.PunchRawLocal.y == -1)
            Trigger += "Bottom";
        else if (info.PunchRawLocal.y == 1)
            Trigger += "Up";
        else {
            print("Error input in punchrawlocal");
            return;
        }

        if (info.PunchRawLocal.x == -1)
            Trigger += "Left";
        else if (info.PunchRawLocal.y == 1)
            Trigger += "Right";
        else {
            print("Error input in punchrawlocal");
            return;
        }

        anim.SetTrigger(Trigger);
        print("i failed punch");
    }

    // Update is called once per frame
    public override void UpdateThis() {
        base.UpdateThis();

        actualtime = Time.time - StartTime;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        

        if (Player.Punch && stateInfo.IsName("BlockV"))
        {
            Player.Punch = false;
            float rand2block = Random.Range(0f, 100f);
            float rand2Dodge = Random.Range(0f, 100f);
            PunchInfo inf = Player.info;
            if (!inf.Hard)
            {
                float AuxProbDodge = PDodge / 2;
                if (inf.PunchRawLocal.y == -1f)
                {
                    bool IsBlock = rand2block < PbottomBlock;
                    bool IsDodge = rand2Dodge < AuxProbDodge;
                    if (IsBlock)
                    {
                        BlockDir = -1;
                        Blocking = true;
                    }
                    else if (IsDodge)
                    {
                        if (inf.PunchRawLocal.x == -1)
                        {
                            anim.SetTrigger("DodgeLeft");
                        }
                        else
                        {
                            anim.SetTrigger("DodgeRight");
                        }
                    }
                }
                else if (inf.PunchRawLocal.y == 1f)
                {
                    bool IsBlock = rand2block < PupBlock;
                    bool IsDodge = rand2Dodge < AuxProbDodge;
                    if (IsBlock)
                    {
                        BlockDir = 1;
                        Blocking = true;
                    }
                    else if (IsDodge)
                    {
                        if (inf.PunchRawLocal.x == -1)
                        {
                            anim.SetTrigger("DodgeLeft");
                        }
                        else
                        {
                            anim.SetTrigger("DodgeRight");
                        }
                    }
                }
            }
            else {
                if (inf.PunchRawLocal.y == -1f)
                {
                    bool IsBlock = rand2block < PbottomBlock;
                    bool IsDodge = rand2Dodge < 1.5f * PDodge;
                    if (IsDodge)
                    {
                        if (inf.PunchRawLocal.x == -1)
                        {
                            anim.SetTrigger("DodgeLeft");
                        }
                        else
                        {
                            anim.SetTrigger("DodgeRight");
                        }
                    }
                    if (IsBlock)
                    {
                        BlockDir = -1;
                        Blocking = true;
                    }
                }
                else if (inf.PunchRawLocal.y == 1f)
                {
                    bool IsBlock = rand2block < PupBlock;
                    bool IsDodge = rand2Dodge <  2 * PDodge;
                    
                    if (IsDodge)
                    {
                        if (inf.PunchRawLocal.x == -1)
                        {
                            anim.SetTrigger("DodgeLeft");
                        }
                        else
                        {
                            anim.SetTrigger("DodgeRight");
                        }
                    }
                    else if (IsBlock)
                    {
                        BlockDir = 1;
                        Blocking = true;
                    }
                }
            }
        }
        float time = Time.time;
        if (PunchRecieved)
        {
            Blocking = false;
            PunchRecieved = false;
        }
        if (BlockUp.Recieve) {
            print("Blocked UP");
        }

        if (!Blocking) {
            BlockDir = 0;
        }

        anim.SetFloat("Blockv", BlockDir);

        bool Trigg = Time.time - StartTime > time2NextCombo;

        if (Trigg) {

            StartTime = Time.time;
            time2NextCombo = Random.Range(ClampedTimeA, ClampedTimeB);
            float B = Random.Range(1f, 2f) - 1;
            int WCombo = B < 0.5 ? 1 : 2;
            //print( B + " is " + WCombo);
            if(stateInfo.IsName("BlockV"))
                anim.SetTrigger("Combo" + WCombo);

        }

        Player.Punch = false;
    }

    #region SettersForAnimation
    public void SetPBottomLeft() {
        info = PIBottom;
        info.PunchRawLocal = bottom + left;
    }
    public void SetPBottomRight() {
        info = PIBottom;
        info.PunchRawLocal = bottom + right;
    }
    public void SetPUpLeft()
    {
        info = PIUp;
        info.PunchRawLocal = up + left;
    }
    public void SetPUpRight()
    {
        info = PIUp;
        info.PunchRawLocal = up + right;
    }

    public void SetHardPBottomLeft()
    {
        info = PIHardBottom;
        info.PunchRawLocal = bottom + left;
    }
    public void SetHardPBottomRight()
    {
        info = PIHardBottom;
        info.PunchRawLocal = bottom + right;
    }
    public void SetHardPUpLeft()
    {
        info = PIHardUp;
        info.PunchRawLocal = up + left;
    }
    public void SetHardPUpRight()
    {
        info = PIHardUp;
        info.PunchRawLocal = up + right;
    }
    #endregion
}
