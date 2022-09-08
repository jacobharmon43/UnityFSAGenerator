# UnityFSAGenerator
# By Jacob Harmon

* States have an Enter and Exit that fire once upon enter and exit
* and an Update method that fires every frame it is in that state

The builder class is simply a class that makes constructing a statemachine pretty and simple, as seen below, rather than a mess of a single constructor.

Example C# Unity usecase:

```cs
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
 ```
 
# Why

* I think it makes code writing easier and cleaner
* Extremely modular and easy to add to

# Things to work on

* Appending statemachines together (ie storing parts of machines in a file somewhere and conglomerating them into a super machine easily
  * This will probably just be a behaviour that attaches statemachines by decomposing into their builder and then appending said builder on
  * It already does support statemachines WITHIN statemachines which is similar, but this effect would be more like assembling a vertical movement statemachine
  * and combining it with the horizontal movement statemachine (they could likely run separately though so maybe not)

*Add constructors to builder that accept pre-defined states with transitions, and predefined transitions if wanted.
*Move WithTransitionFromAnyState to a global builder command that can be placed anywhere, even where it makes no sense
*Reword naming of builder functions to sound better in English IE
  * Create a statemachine WithState().That().FiresOnEntry().Runs().FiresOnExit().TransitionsTo().End() Where That() and End() open and close the builder section
    *Function naming tbd
    

   
