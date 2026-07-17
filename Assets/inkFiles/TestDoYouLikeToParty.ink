EXTERNAL increaseSIL(choice)
EXTERNAL getSIL(choice)


->Shadow

==Shadow==
Which shadow tool do you want to develop?
    + * []
        ~increaseSIL("Bucket")
        ->Bucket
    + [Spray Can]
        ~increaseSIL("Spray")
        ->Spray
    + [Lantern]
        ~increaseSIL("Lantern")
        ->Lantern
    + [GET ME OUTTA HERE!!]
        
        ->END


== Bucket ==
Bucket level: {getSIL("Bucket")}
-> Shadow


== Spray ==
Spray level: {getSIL("Spray")}
-> Shadow

== Lantern ==
Lantern level: {getSIL("Lantern")}
-> Shadow







































==Party==
Do you like to party?

 * [Yeah]
    ->Heck_Yeah
 * [Not so much]
    ->Not_so_much

- They lived happily ever after.
    -> END

== Heck_Yeah==
Excellent!......I'm thinking we should party together?
    *[Absolutely]
        ->PartyTogether
    *[Nah way]
        ->Later

==Later==
Oh okay thats cool I guess, later then.
    ->END

==PartyTogether==
Excellent, Let's Party!
    ->END

== Not_so_much ==
I'd be a lot cooler if you did......
What do you like?
    * [Books.]
        ->Books
    * [Chucking Bottles]
        ->NotCool
    * [Making video games!]
        ->JoinMyCLub
    
==Books==
Damn thats like really intelligent...
->WannaHangOut

==NotCool==
Yo Punk you better not be leaving a mess of broken glass everywhere...
**[Or What?]
    ->YourFace
**[Sorry my bad]
    ->ThatsWhatIThought

==YourFace==
Or I'll break your face. So check yourself.
->OkayBye

==ThatsWhatIThought==
That's what I thought, clean up your messes or I'll make you clean them.
->OkayBye

==JoinMyCLub==
That's rad! You should totally join our game development club!! 
    ->WannaHangOut

==WannaHangOut==
You seem cool, do you wanna hang out?
    *[Heck yeah!]
        ->LetsGo
    *[Sorry no.]
        ->OkayBye

==LetsGo==
LET'S GOOOO!!!!!!!
->END

==OkayBye==
Okay Bye Felicia.
->END 