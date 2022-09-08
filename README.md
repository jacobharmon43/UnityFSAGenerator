# Unity FSA Generator
## By Jacob Harmon

### Overview

I found myself at some point a year ago or something writing a huge amount of if statements, switch cases, just to create unreadable code spaghetti. Despite my best efforts to separate code into clean, readable chunks, I just had no easy way to do so. Then I remembered that the concept of a statemachine existed, and I wrote this in a weekend. I've been using it since. It allows me to quickly and easily generate a statemachine with states, transitions, and entrapped logic to those states that all run without any extra effort from myself. This is of course a glorified switch case that calls functions, but nevertheless I enjoy the syntactic sugar it provides using this library, see use case below with comparison.
 
### Why

* I think it makes code writing easier and cleaner
* Extremely modular and easy to add to

### Things to work on

* Appending statemachines together (ie storing parts of machines in a file somewhere and conglomerating them into a super machine easily
  * This will probably just be a behaviour that attaches statemachines by decomposing into their builder and then appending said builder on
  * It already does support statemachines WITHIN statemachines which is similar, but this effect would be more like assembling a vertical movement statemachine and combining it with the horizontal movement statemachine (they could likely run separately though so maybe not)

 *Add constructors to builder that accept pre-defined states with transitions, and predefined transitions if wanted.
 *Move WithTransitionFromAnyState to a global builder command that can be placed anywhere, even where it makes no sense
 *Reword naming of builder functions to sound better in English IE
  * Create a statemachine WithState().That().FiresOnEntry().Runs().FiresOnExit().TransitionsTo().End() Where That() and End() open and close the builder section
    *Function naming tbd
    
### Example C# Unity usecase:

```cs
  float _coyoteTime;
  void Start(){
      _coyoteStateMachine = new StateMachineBuilder()
            .WithState("OnGround")
            .WithTransition("CoyoteForgiveness", () => !IsGrounded())

            .WithState("CoyoteForgiveness")
            .WithTransition("Falling", () => _coyoteStateMachine.TimeInCurrentState >= _coyoteTime)
            .WithTransition("OnGround", () => IsGrounded())

            .WithState("Falling")
            .WithTransition("OnGround", () => IsGrounded())

            .Build();

          //The functions preceded by Exit are boolean functions that determine whether it SHOULD transition
          //The functions otherwise are void returning and perform actions
          //A lot of these are short and can be inlined, but for ease of reading I make them all seperate functions
  }

  void Update(){
      _coyoteStateMachine.RunStateMachine();
      //... Rest of the code here
  }
 ```
 
 ### Comparison with the same usecase:
 ```cs
  float _coyoteDuration = 0.2;
  float _coyoteTimer = 0;
  bool _jumpAllowed = false;
  
  void Update(){
    if(!IsGrounded()){
      if(_coyoteTimer <= 0){
        _jumpAllowed = false;
      }
       else{
        _jumpAllowed = true;
        _coyoteTimer -= Time.deltaTime;
       }
    }
    else{
      _coyoteTimer = _coyoteDuration;
    }
    
    //...Rest of the logic
  }
 ```
 
 ## Comparison Overview
  While being marginally less lines of code (I didn't actually count) the non-statemachine based code is a lot less readable from the get-go, and requires 3 global variables to track information instead of just 1 (although TECHNICALLY the statemachine creates 3 class variables, 4 transition variables, and one machine variable itself) those are all hidden so I'm willingly ignoring them to make a case for my class's usage.

  But seriously, you can show my code to a middle schooler and they'd likely have a good idea of what it does, while the other code is a mixed bag due to its multiple nested if statements and tedious timer tracking that requires two variables, frankly that code might not work I didn't bother to test it. Regardless of any of that, I just think my setup is way better.
   
