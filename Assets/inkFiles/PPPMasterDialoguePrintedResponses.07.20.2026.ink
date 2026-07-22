# authors: Aren Frymire
# title: The Painter's Prismatic Palimpsest
//Player Variables. Tracked using +/- scale.
VAR anger = 0
VAR fear = 0
VAR sadness = 0
VAR stamina = 0

 	
// DEBUG mode adds a few shortcuts - remember to set to false in release!
VAR DEBUG = false


{DEBUG:
	IN DEBUG MODE!
    * [INTRO] -> INTRO
    * [Scene2] ->Scene2
    * [Scene3] ->Scene3
- else:
	// First diversion: where do we begin?
 ->INTRO
} 

/*-------------------------------------
This is where functions go, or the game don't work
--------------------------------------*/
 === function lower(ref x)
 	~ x = x - 1 
 	{x < stamina:
 	    ~ stamina = stamina - 1
    }
 === function raise(ref x)
 	~ x = x + 1 
 	
 EXTERNAL button(next)
 
 	
/*-------------------------------------

Start

----------------*/
//Scene 1
===INTRO===
 #bg Intro0
->intro_wake_up 
= intro_wake_up 
{&"zzz..."|"zzzzz..."|"zzzzzzz..."|->wakeorsleep}#sprite Anger
+[-#next]->intro_wake_up
= wakeorsleep
#sprite Fear
???: "{&Hey kiddo, it's time to wake up.|You really need to wake up...|<i>Seriously</i>, you need to wake up now.}"
+[*Wake Up*] You open your eyes and lift yourself upright.
->wakeUp
+{wakeorsleep < 3} "Just 5 more minutes..."[]
->intro_wake_up
= wakeUp
{wakeorsleep < 2 : |???: "Not much of a morning person are you?"} 
->wakeUpEnd
= wakeUpEnd  
# bg Intro1
    *[*Yawn*]->start
= start
*"Where am I?"[] 
#sprite Sadness
???: "Uh... technically this is your room, but I wouldn't want to stay there if I were you..."
*"How did I get here?"[] 
???: "We don't know... we're not even sure where you are exactly..."
-->who_are_you
= who_are_you
*"Hold on... who's there?"[] 
???: "A friend. We can get you out of there, but you have to trust us." ->theFirstNeuronTree
=theFirstNeuronTree
*"Who is <b>us</b>?"[]
->treeSprouts
= treeSprouts
# bg Intro2
!!!!!
*<b>"Wait! What's Happening!?!"[]
????: "Don't be afraid, that's your way out child."
->TouchTheFirstTree
=TouchTheFirstTree 
*"What is it?..... Some sort of plant or....."[]
->Intro3
=Intro3
# bg Intro3
->WhatDoYouDo
=WhatDoYouDo
{???: <b>"What are you waiting for?"|} 
*[*Reach out and touch it*]
->DownTheRabbitHole
*"Why would I touch that thing?"[]
->inspectitfirst
=inspectitfirst
?????: "It's your salvation little one, and ours. Please hurry, you don't have much time left in there!"
->WhatDoYouDo
=DownTheRabbitHole
?????: "{inspectitfirst > 0 :Brace yourself little one!|Rather a bold little one aren't you? Hang on!}"
->Intro4
=Intro4
# bg Intro4
*[-#next]
->Scene2

//Scene 2
===Scene2===
->Scene2Frame1
=Scene2Frame1
# bg Portal1
*[<b>"WOOOAAAAAAHHHHHH!!!!!"]
    **"Where am I?"[] 
    **"What's happening?"[] 
    **"This feels strange..."[] 
    -????: "....hard to....you."
    *"What?"[]
    ???: "...you need to...... by yourself now."
        **"I can't understand you, it's all...warbled."[]
        ?????: "...not much time.....to help..."
            ***"Help who?"[]
                ???: "...hear me....there kiddo."
                ????: "..it's up to you....child.......ust you."
                ?????: "Listen...carefully....little one.......you must..."
                ****"What's going on!?!"[]
                ****"What are you saying!?!"[]
                - ->Scene2Frame2
    =Scene2Frame2
    # bg Portal2
    ????: "....s not much time left child, can you hear me? #sprite Unknown
    *"I can hear you. What's going on?"[]
    *"Barely, what did you do to me?"[]
    -???: "You've got a pretty big journey ahead of you kiddo."
    ?????: "It's going to take a lot of strength, little one."
    ????: "I'm sorry we have to ask so much of you child..."
    ->Scene2Frame3
    =Scene2Frame3
    *"Wait... are you not coming with me?"[] # bg Portal3
        **"WOOOOAAAAAHHHH!!!!"[]
            ?????: "You must find and free the three of us."
            ???: "Then we can help you escape this prison."
            ????: "It's not going to be easy, but few things worth doing ever are..."
                ***"Wait, I have so many questions!"[] {raise(anger)}
                ***"What if I'm not ready?"[] {raise(fear)}
                ***"Are you sure I'm the right person for this?"[] {raise(sadness)}
        - ->Scene2Frame4
    =Scene2Frame4
    #bg black
    # anim Memory
    * [<i>What's happening now.....?] ->Scene2Frame5
=Scene2Frame5
# bg Scene2.1
The tearing rumble of a rocket engine fills your ears so completely that it might as well be coming from inside your own head. You can feel your blood begin to simmer, boil, and then steam. You find your jaw clenched and your hands have balled themselves into fists. Somewhere in the distant background you can swear someone is calling out to you….  
*[-#next] ->Scene2Frame6
=Scene2Frame6
#bg black #anim Memory
*[<i>That was weird, and really intense...]->Scene2Frame7
=Scene2Frame7
#bg Scene2.2
The riotous roar of an angry crowd consumes you, burying you beneath its sheer volume. Your palms begin sweating, your breaths start to shallow, and your vision narrows around the edges. Distant pops and booms from far away tug and pull at your attention as you try to process the crowd that surrounds you. Again somewhere in the distant background you can almost hear someone whispering your name.
*[-#next] ->Scene2Frame8
=Scene2Frame8
#bg black #anim Memory
*[<i>Seriously, what is happening....?] ->Scene2Frame9
=Scene2Frame9
#bg Scene2.3
Soul piercing screeches echo in chorus from the flock of dragons as they descend downwards towards the encampment. The flickering lights of the encampment remind you of the light of twinkling stars before they begin to disappear. As they go they seem to take a part of your strength with them, you feel yourself growing heavier and heavier until even drawing breath becomes difficult. The silence weighs heavy on you.
*[-#next] ->Scene2Frame10
=Scene2Frame10
#bg black #anim Memory
*[<i>These feel like..... someone's memories?] ->Scene2Frame11
=Scene2Frame11
#bg Scene2.4
Your head fills with yet another vision.... or memory...? This time the memory is calmer, more focused. You see an artist.... or some sort of painter? They sit hunched over in a room that feels almost blank save for the large window looking out into the void of space.
*[-#next]
"What do they want from me?!? I haven't been back to Earth since I was a child... I've lived almost my entire life out here in the stars... Do they not understand how holopainting works?.... I can't just...just.... slap something new together...."
**[<i>Why are they so angry?] {raise(anger)}
**[<i>They seem almost sad.] {raise(sadness)}
**[<i>They worry alot...] {raise(fear)}
-"It's not as if I can just pull new memories out of my... head.......or maybe..."
    *[<i>Oh no.......don't tell me, they didn't.] {raise(fear)}
    *[<i>Of course they did] {raise(anger)}
    *[<i>Why does this always happen to me?] {raise(sadness)}
    -"I wonder...maybe... there could be a way....to dig deeper."
        **[<i>Or we could <b>not</b> dig deeper] {raise(anger)}
        **[<i>I feel like I know where this is going....] {raise(fear)}
        **[<i>Why can't some people just leave things be?] {raise(sadness)}
            --They struggle with the strange device, prying at its iridescent edges until it finally emits several short vibrations.       
        "Oh come on...stupid..... just let me... take... this... .........off!" 
            ***[<i>That doesn't seem like it's supposed to come off...] {raise(fear)}
            ***[<i>They're really brute forcing that thing] {raise(anger)}
            ***[<i>This can't be good...] {raise(sadness)}
                ---->Scene2Frame12
=Scene2Frame12
#bg Scene2.5
"OH CRAP!"
*[<i>Oh that thing is <b>definitely</b> busted] {raise(anger)}
*[<i>They saw that crystal thing crack right?] {raise(fear)}
*[<i>I've got a bad feeling about this...] {raise(sadness)}
- ->Scene2Frame13
=Scene2Frame13
#bg Scene2.6
Twisting shadows and lights begin erupting from the crystalline core of the device, enveloping them. You can taste iron dancing on your tongue as your head begins thrumming with the deafening sound of your own blood pumping. You can hear something chanting, something cackling, and something screaming in the background.
*[<i>I really don't like this....] {raise(fear)}
*[<i>Why couldn't they just leave that thing alone?] {raise(anger)}
*[<i>I guess I can't say I'm surprised.....] {raise(sadness)}
- ->Scene2Frame14
=Scene2Frame14
#bg Scene2.7
*[-#next] ->Scene2Frame15
=Scene2Frame15
#bg black #anim Memory
*<i>Okay... now what?[] {raise(anger)}
*[<i>I can't imagine things getting better from here...]{raise(sadness)}
*[<i>Oh things are definitely going to get worse]{raise(fear)}
- ->Scene2Frame16
=Scene2Frame16
#bg Portal3
?????: ".....the Painter's memories....."
???: ".....started something...you have to finish....."
????: ".....really not fair.....ask this of you......"
*"Are you back now?"[] {raise(anger)}
*"Don't make me do this alone..."[] {raise(sadness)}
*"Please don't leave again."[] {raise(fear)}
-Faintly, just before completely fading away, you hear all three of their voices shouting together in unison: 
"Free Us, Confront the Painter, Finish the Ritual!!"
*[-#next]
->END

//Scene 3 
=== Scene3 ===
#bg Portal1
* "Wooooaaaahhh!"[]
    **[<i>It's happening again?!?] #black #anim Memory
        ***[<i>Is this... another one of their memories?] ->FearMemory1
=FearMemory1
#backdrop("FearMemory1")
You sit in a cheap studio apartment, the floor is covered in toys. "Hmmm hmmmm hmmm hmmm hmmmm," a child's voice hums a simple melody.
You hear the voices of a man and women whispering across the room.
*[-]
A women's voice calls out to you.
"Okay honey it's time to play a game."
    **[-]"...what game are we playing?"
        ***[-] "We're playing who can be the quietest and hide the longest.... if you win I'll give you a special prize."
            ****[-] ->FearMemory2
=FearMemory2
#backdrop("FearMemory2")
"okay"
You crawl underneath the bed along with her, bringing along one of your toy blocks. It's warm under the bed, and she wraps you up in her soft arms as you snuggle in to hide.
*[-]"Okay, ...ready? ...set?"
    **[-]Her voice cuts down to a whisper, "...be very quiet."
        ***[-].... #backdrop("black")
            ****[-]->Scene3Outro
=Scene3Outro
#backdrop("MemoryTransition")
*[<i>What was that?]
*[<i>Okay?]
*[<i>That was weird...]
-???: "you really aren't supposed to go in there..." #sprite("FearObscured") #backdrop("Portal2")
*["Who's there?"] 
???: ".....nobody..."
    **["Show yourself!"] 
    **["Then who am I talking to?"]
    -???: "... i'm not sssupposed to talk to you...."
            ***["It's okay, I don't bite"] {raise(fear)} ->FearShadowRevealed
            ***["Quit hiding!"] {lower(fear)}
                ->FearShadowRevealed
=FearShadowRevealed
#sprite("Fear")
*["It's you! You're one of the ones that helped me before."] 
Fear: "...you shouldn't have come here... it's not safe..."
    **["If it's not safe, then you shouldn't be here either. Let's find a way out of here together."] {raise(fear)}
    **["You brought me here... so you’re going to help me find the way out of here."] {lower(fear)}
    -#backdrop("Portal3")
    *[-]
->END
