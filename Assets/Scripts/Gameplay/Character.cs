using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Animator anim { get; private set; }

    [Header("Attack Colliders")]
    public Transform AttColl;
    protected BoxCollider2D UpAttColl;
    protected BoxCollider2D BottomAttColl;
    protected BoxCollider2D UpHardAttColl;
    protected BoxCollider2D BottomHardAttColl;

    [Header("Recieve Colliders")]
    public Transform RecvColl;
    protected BoxCollider2D UpRecvColl;
    protected BoxCollider2D BottomRecvColl;

    [Header("Blocking Colliders")]
    public Transform BlockColl;
    protected BoxCollider2D UpBlockColl;
    protected BoxCollider2D BottomBlockColl;

    [Header("Health")]
    public float CurrentHealth;
    public float MaxHealth;

    [Header("Stamina")]
    public float CurrentStamina;
    public float MaxStamina;
    float CurrentMaxStamina;
    public float RecuperationPerSecond;

    #region AttReceptors
    protected ReceptorScript AttBottom;
    protected ReceptorScript AttUp;
    protected ReceptorScript HardAttBottom;
    protected ReceptorScript HardAttUp;
    #endregion
    public bool PunchEnded;

    #region Recievers
    protected RecieverScript RecvBottom;
    protected RecieverScript RecvUp;
    protected RecieverScript BlockBottom;
    protected RecieverScript BlockUp;
    #endregion
    public bool PunchRecieved;

    bool pause = false;

    public bool Punch;
    public PunchInfo info;

    public float SD2Dodge = 30;

    [Header("States and Times")]
    public bool IsTired;
    public bool PunchFailed;

    [Header("Punch Infos")]
    public PunchInfo PIBottom;
    public PunchInfo PIUp;
    public PunchInfo PIHardBottom;
    public PunchInfo PIHardUp;

    public Stateinfos stateinfo;



    #region FunctionsForAnimations

    protected Vector2 up = Vector2.up;
    protected Vector2 bottom = Vector2.down;
    protected Vector2 left = Vector2.left;
    protected Vector2 right = Vector2.right;
    

    public void BeginPunch()
    {
        //info = AuxInfo;
        AttBottom.Info = AttUp.Info = HardAttBottom.Info = HardAttUp.Info = info;
        StaminaDown(info.Stamina);
    }

    public void BeginDodge()
    {
        StaminaDown(SD2Dodge);
    }

    protected virtual bool Conditions2Fail() {
        return !PunchEnded;
    }

    public virtual void CheckFailPunch() {
        if (Conditions2Fail()) {
            FailingProcess();
        }
        PunchEnded = false;
    }

    protected virtual void FailingProcess() {
        anim.SetBool("PunchFailed", true);
        PunchFailed = true;
        print("i failed punch");
    }
    #endregion

    #region StateinfoClass
    [System.Serializable]
    public class Stateinfos {

        public AnimatorStateInfo StateInfo { get; private set; }


        [Header("Idle-Block")]
        public bool BlockIdle;
        public bool BlockedUp;
        public bool BlockedBottom;
        public bool Blocked;
        protected void GetBlocks() {
            BlockIdle = StateInfo.IsName("BlockV");
            BlockedUp = StateInfo.IsName("BlockedUp");
            BlockedBottom = StateInfo.IsName("BlockedBottom");
            Blocked = BlockedUp || BlockedBottom;
        }

        [Header("Dodge")]
        public bool DodgeLeft;
        public bool DodgeRight;
        public bool Dodging;
        protected void GetDodge() {
            DodgeLeft = StateInfo.IsName("DodgeLeft");
            DodgeRight = StateInfo.IsName("DodgeRight");
            Dodging = DodgeLeft || DodgeRight;
        }

        [Header("Normal Punch")]
        public bool PunchBottomLeft;
        public bool PunchBottomRight;
        public bool PunchUpLeft;
        public bool PunchUpRight;
        public bool Punching;
        protected void GetPunchs() {
            PunchBottomLeft = StateInfo.IsName("PunchBottomLeft");
            PunchBottomRight = StateInfo.IsName("PunchBottomRight");
            PunchUpLeft = StateInfo.IsName("PunchUpLeft");
            PunchUpRight = StateInfo.IsName("PunchUpRight");
            for (int i = 0; i < 10; i++)
            {
                PunchBottomLeft = PunchBottomLeft || StateInfo.IsName("PunchBottomLeft" + i);
                PunchBottomRight = PunchBottomRight || StateInfo.IsName("PunchBottomRight" + i);
                PunchUpLeft = PunchUpLeft || StateInfo.IsName("PunchUpLeft" + i);
                PunchUpRight = PunchUpRight|| StateInfo.IsName("PunchUpRight" + i);
            }
            

            Punching = PunchBottomLeft || PunchBottomRight || PunchUpLeft || PunchUpRight;
        }

        [Header("Hard Punch")]
        public bool HardPunchBottomLeft;
        public bool HardPunchBottomRight;
        public bool HardPunchUpLeft;
        public bool HardPunchUpRight;
        public bool HardPunching;
        protected void GetHardPunchs() {
            HardPunchBottomLeft = StateInfo.IsName("HardPunchBottomLeft");
            HardPunchBottomRight = StateInfo.IsName("HardPunchBottomRight");
            HardPunchUpLeft = StateInfo.IsName("HardPunchUpLeft");
            HardPunchUpRight = StateInfo.IsName("HardPunchUpRight");
            for (int i = 0; i < 10; i++)
            {
                HardPunchBottomLeft = HardPunchBottomLeft || StateInfo.IsName("HardPunchBottomLeft" + i);
                HardPunchBottomRight = HardPunchBottomRight || StateInfo.IsName("HardPunchBottomRight" + i);
                HardPunchUpLeft = HardPunchUpLeft || StateInfo.IsName("HardPunchUpLeft" + i);
                HardPunchUpRight = HardPunchUpRight || StateInfo.IsName("HardPunchUpRight" + i);
            }

            HardPunching = HardPunchBottomLeft || HardPunchBottomRight || HardPunchUpLeft || HardPunchUpRight;
        }

        [Header("Punch Failed")]
        public bool PunchFailedBottomLeft;
        public bool PunchFailedBottomRight;
        public bool PunchFailedUpLeft;
        public bool PunchFailedUpRight;
        public bool NormalPunchFailed;

        public bool PunchFailedHBottomLeft;
        public bool PunchFailedHBottomRight;
        public bool PunchFailedHUpLeft;
        public bool PunchFailedHUpRight;
        public bool HardpunchFailed;

        public bool PunchFailed;

        public bool FailingPunch;
        protected void GetPunchFailed() {
            PunchFailedBottomLeft = StateInfo.IsName("PunchFailedBottomLeft");
            PunchFailedBottomRight = StateInfo.IsName("PunchFailedBottomRight");
            PunchFailedUpLeft = StateInfo.IsName("PunchFailedUpLeft");
            PunchFailedUpRight = StateInfo.IsName("PunchFailedUpRight");
            NormalPunchFailed = PunchFailedBottomLeft || PunchFailedBottomRight || PunchFailedUpLeft || PunchFailedUpRight;

            PunchFailedHBottomLeft = StateInfo.IsName("PunchFailedHBottomLeft");
            PunchFailedHBottomRight = StateInfo.IsName("PunchFailedHBottomRight");
            PunchFailedHUpLeft = StateInfo.IsName("PunchFailedHUpLeft");
            PunchFailedHUpRight = StateInfo.IsName("PunchFailedHUpRight");
            HardpunchFailed = PunchFailedHBottomLeft || PunchFailedHBottomRight || PunchFailedHUpLeft || PunchFailedHUpRight;

            PunchFailed = StateInfo.IsName("PunchFailed");

            FailingPunch = NormalPunchFailed || HardpunchFailed || PunchFailed;
        }

        [Header("Recieve Punch")]
        public bool RecieveBottomLeft;
        public bool RecieveBottomRight;
        public bool RecieveUpLeft;
        public bool RecieveUpRight;
        public bool RecievePunch;
        protected void GetRecievePunch() {
            RecieveBottomLeft = StateInfo.IsName("RecieveBottomLeft");
            RecieveBottomRight = StateInfo.IsName("RecieveBottomRight");
            RecieveUpLeft = StateInfo.IsName("RecieveUpLeft");
            RecieveUpRight = StateInfo.IsName("RecieveUpRight");

            RecievePunch = RecieveBottomLeft || RecieveBottomRight || RecieveUpLeft || RecieveUpRight;
        }

        [Header("Recieve Hard Punch")]
        public bool RecieveHardBottomLeft;
        public bool RecieveHardBottomRight;
        public bool RecieveHardUpLeft;
        public bool RecieveHardUpRight;
        public bool RecieveHardPunch;
        protected void GetRecieveHardPunch() {
            RecieveHardBottomLeft = StateInfo.IsName("RecieveHardBottomLeft");
            RecieveHardBottomRight = StateInfo.IsName("RecieveHardBottomRight");
            RecieveHardUpLeft = StateInfo.IsName("RecieveHardUpLeft");
            RecieveHardUpRight = StateInfo.IsName("RecieveHardUpRight");

            RecieveHardPunch = RecieveHardBottomLeft || RecieveHardBottomRight || RecieveHardUpLeft || RecieveHardUpRight;
        }

        [Header("Tired")]
        public bool Tired;
        protected void GetTired() {
            Tired = StateInfo.IsName("Tired");
        }

        public virtual void GetStatesInfo(AnimatorStateInfo StateInf) {
            StateInfo = StateInf;

            GetBlocks();
            GetDodge();
            GetPunchs();
            GetHardPunchs();
            GetPunchFailed();
            GetRecievePunch();
            GetRecieveHardPunch();
            GetTired();
        }
    }

    #endregion


    public virtual void DoPunch() {
        Punch = true;
    }

    public virtual void LoadData() {
        anim = GetComponent<Animator>();

        //Attack Colliders
        UpAttColl = AttColl.GetChild(0).GetComponent<BoxCollider2D>();
        BottomAttColl = AttColl.GetChild(1).GetComponent<BoxCollider2D>();
        UpHardAttColl = AttColl.GetChild(2).GetComponent<BoxCollider2D>();
        BottomHardAttColl = AttColl.GetChild(3).GetComponent<BoxCollider2D>();

        //Recieve Colliders
        UpRecvColl = RecvColl.GetChild(0).GetComponent<BoxCollider2D>();
        BottomRecvColl = RecvColl.GetChild(1).GetComponent<BoxCollider2D>();
        
        //Blocking Colliders
        UpBlockColl = BlockColl.GetChild(0).GetComponent<BoxCollider2D>();
        BottomBlockColl = BlockColl.GetChild(1).GetComponent<BoxCollider2D>();

        //Receptors
        AttBottom = BottomAttColl.transform.GetComponent<ReceptorScript>();
        AttUp = UpAttColl.transform.GetComponent<ReceptorScript>();
        HardAttBottom = BottomHardAttColl.transform.GetComponent<ReceptorScript>();
        HardAttUp = UpHardAttColl.transform.GetComponent<ReceptorScript>();

        //Recievers
        RecvBottom = BottomRecvColl.transform.GetComponent<RecieverScript>();
        RecvUp = UpRecvColl.transform.GetComponent<RecieverScript>();
        BlockBottom = BottomBlockColl.transform.GetComponent<RecieverScript>();
        BlockUp = UpBlockColl.transform.GetComponent<RecieverScript>();

        //Bars
        CurrentHealth = MaxHealth;
        CurrentMaxStamina = MaxStamina;
        CurrentStamina = CurrentMaxStamina;
    }

    public virtual void UpdateThis() {
        if (pause)
            return;

        

        CurrentStamina += RecuperationPerSecond * Time.deltaTime;
        CurrentHealth += 10f * Time.deltaTime;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        CurrentMaxStamina = Mathf.Clamp(CurrentMaxStamina, 0, MaxStamina);
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, CurrentMaxStamina);
    }

    public virtual void FixedUpdatethis() {
        if (pause)
            return;
        stateinfo.GetStatesInfo(anim.GetCurrentAnimatorStateInfo(0));
        if (PunchFailed && stateinfo.FailingPunch)
        {
            anim.SetBool("PunchFailed", false);
            PunchFailed = false;
        }
    }

    public virtual void Damaged(PunchInfo punchInfo) {
        CurrentHealth -= punchInfo.Damage;
        string Trigger = "Recieve";

        if (punchInfo.Hard)
            Trigger += "Hard";

        if (punchInfo.PunchRawLocal.y == -1)
            Trigger += "Bottom";
        else if (punchInfo.PunchRawLocal.y == 1)
            Trigger += "Up";
        else print("Error in Y input of PunchInfo");

        if (punchInfo.PunchRawLocal.x == -1)
            Trigger += "Left";
        else if (punchInfo.PunchRawLocal.x == 1)
            Trigger += "Right";
        else print("Error in X input of PunchInfo");

        anim.SetTrigger(Trigger);

        if (stateinfo.Punching || stateinfo.HardPunching) {
            anim.SetBool("PunchFailed", true);
            print("i get punch whlie i punch");
            PunchFailed = true;
        }


        if (CurrentHealth <= 0) {
            Defeated();
            return;
        }
    }

    public virtual void StaminaDown(float minusStamina) {
        CurrentStamina -= minusStamina;
        if (CurrentStamina <= 0) {
            Tired();
            return;
        }
    }

    public virtual void Blocked(PunchInfo info) {
        anim.SetTrigger("Blocked");
    }

    public virtual void Tired() {
        print("tired");
    }

    public virtual void Defeated() {
        print("Defeated");
    }

    // Start is called before the first frame update
    void Start() {
        stateinfo = new Stateinfos();
        LoadData();
    }

    // Update is called once per frame
    void Update() {
        if (pause)
            return;
        UpdateThis();
    }

    void FixedUpdate() {
        FixedUpdatethis();
        //WatchAttackColliders();
    }
}
