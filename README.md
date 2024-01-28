# Unity FSA Generator
## By Jacob Harmon

### Overview

I found myself at some point a year ago or something writing a huge amount of if statements, switch cases, just to create unreadable code spaghetti. Despite my best efforts to separate code into clean, readable chunks, I just had no easy way to do so. Then I remembered that the concept of a statemachine existed, and I wrote this in a weekend. I've been using it since. It allows me to quickly and easily generate a statemachine with states, transitions, and entrapped logic to those states that all run without any extra effort from myself. This is of course a glorified switch case that calls functions, but nevertheless I enjoy the syntactic sugar it provides using this library, see use case below with comparison.
 
### Why

* I think it makes code writing easier and cleaner
* Extremely modular and easy to add to

### Things to work on

- [ ] Appending statemachines together (ie storing parts of machines in a file somewhere and conglomerating them into a super machine easily
  - This will probably just be a behaviour that attaches statemachines by decomposing into their builder and then appending said builder on
  - It already does support statemachines WITHIN statemachines which is similar, but this effect would be more like assembling a vertical movement statemachine and combining it with the horizontal movement statemachine (they could likely run separately though so maybe not)

- [ ] Add constructors to builder that accept pre-defined states with transitions, and predefined transitions if wanted.
- [ ] Move WithTransitionFromAnyState to a global builder command that can be placed anywhere, even where it makes no sense
- [ ] Reword naming of builder functions to sound better in English:Createing a statemachine    ```cs.WithState().That().FiresOnEntry().Runs().FiresOnExit().TransitionsTo().End()```
 Where That() and End() open and close the builder section (Function naming tbd)
- [ ] Visualization tool
- [ ] Intentional transition priority (not just creation order) that uses creation order ONLY as a fallback
    
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
      _coyoteStateMachine.RunStateMachine(Time.deltaTime);
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

### Another usecase and comparison where it may be easier NOT to use this library
```cs
        /// GOAL: Make sprite flip left when you walk left
        /// GOAL: Make sprite flip right when you walk right
        /// GOAL: Sprite will not flip if you are not moving
        /// NOTE: LookedRight => Positive movement input
        /// NOTE: LookedLeft => Negative movement input
        _spriteFlipStateMachine = new StateMachineBuilder()
            .WithState("Left")
            .WithOnEnter(() => _sprite.flipX = true)
            .WithTransition("Right", LookedRight)

            .WithState("Right")
            .WithOnEnter(() => _sprite.flipX = false)
            .WithTransition("Left", LookedLeft)

            .Build();

        /// VS
        
        if (_sprite.flipX && LookedRight()) {
            _sprite.flipX = false;
        } else if (!_sprite.flipX && LookedLeft()) {
            _sprite.flipX = true;
        }
        /// OR
        _sprite.flipX = (_sprite.flipX && LookedRight()) ? false : (!_sprite.flipX && LookedLeft()) ? true : _sprite.flipX;
        /// The one liner is horrific to read, and the if statements above are fine I guess, I just prefer the look of my solution.
```   
