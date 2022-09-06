# UnityFSAGenerator

/*
* States have an Enter and Exit that fire once upon enter and exit
* and an Update method that fires every frame it is in that state
*/

//The builder class is simply a class that makes constructing a statemachine pretty and simple, as seen below, rather than a mess of a single constructor.

Example C# Unity usecase:

  void Start(){
      _playerStateMachine = new StateMachineBuilder()
          .WithState("Idle")
          .WithOnEnter(IdleEnter)
          .WithTransition("Walking", ExitIdleToWalk)
          .WithTransition("Punch1", ExitIdleToPunch)

          .WithState("Walking")
          .WithOnEnter(WalkEnter)
          .WithOnRun(WalkUpdate)
          .WithTransition("Idle", ExitWalkToIdle)
          .WithTransition("Punch1", ExitIdleToPunch)

          .WithState("Punch1")
          .WithOnEnter(PunchEnter)
          .WithOnRun(PunchUpdate)
          .WithTransition("Idle", ExitPunchToIdle)
          .WithTransition("Punch2", ExitPunchToPunch2)

          .WithState("Punch2")
          .WithOnEnter(Punch2Enter)
          .WithOnRun(Punch2Update)
          .WithTransition("Idle", ExitPunch2ToIdle)
          .WithTransition("Punch3", ExitPunch2ToPunch3)

          .WithState("Punch3")
          .WithOnEnter(Punch3Enter)
          .WithTransition("Idle", ExitPunch3ToIdle)

          .Build();

          //The functions preceded by Exit are boolean functions that determine whether it SHOULD transition
          //The functions otherwise are void returning and perform actions
          //A lot of these are short and can be inlined, but for ease of reading I make them all seperate functions
  }

  void Update()
  {
      _playerStateMachine.RunStateMachine();
  }
