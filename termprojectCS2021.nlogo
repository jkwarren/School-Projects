;Jessica Warren
;Dec 15, 2021
;CSCI-110 HW 7
; a game where chickens run from the player and are hunted
;see the info tab for a game-play explinations

breed [farmers farmer]
breed [chickens chicken]
breed [predators predator]
breed [killers killer]


chickens-own
[
  stomach
  hungry?
  age

]
predators-own
[
  rangled?
]
patches-own
[
  land-type
  food
  predators-here?
  chickens-here?
]



globals [
  PLAYER ;the turtle controled by the player
  MOVE-SPEED ;how fast the player moves
  MAX-FOOD ; the max food that a patch can hold
  ST-FOOD ; the standard amount of food added to a patch when grass ;
          ;command is called (min amount of food added)
  MAX-STOMACH ;the max amount of food that can be in a chickens stomach
  CHICKEN-SPEED ; how fast chickens can move
  FOOD-LOSS-RATE ;how fast patches lose food
  ;==land types==
  GRASS
  GROUND
  WATER
  BRIDGE

  ;===chicken===
  MAX-AGE  ;how long chickens live
  NEW-CHICKEN-T ; the time before a new chicken
  EATING-RATE ; how much is eaten at once
  STOMACH-LOSS-R ; how quickly the stomach goes down
  NUM-NEW-CHICK ;how many chickens spawn at a time

  ;=preadator
  WOLF-PACK ;number for how many wolves spawn
  HAWK-PACK ;number for how many chickenhawks spawn
  HUNTER-SPEED
  KILLER-SPEED

  time ;for the timer
  level ; what level is being played
  list1

]

to setup
  ca
  set level 1 ;we do this here because each level had different initialized variables
  initialize-var level
  setup-farmer
  setup-ground
  set list1 [0] ; this is set for the highscore reporter
 ; setup-chicken; i realizes that the every doesn't count to ten first so when the game starts a chicken will be made
  reset-ticks
end

to go

  grass-losing-food
  chicken-time
  predator-event
  wait .05
  timert
  level-over
  tick

end


to initialize-var [levels]
 if levels = 1 [
  set MOVE-SPEED .5
  set MAX-FOOD 20
  set ST-FOOD 7
  set FOOD-LOSS-RATE .3
  set GRASS 1
  set GROUND 0
    set WATER 2
  set BRIDGE 3

  ; chicken
  set MAX-STOMACH 80
  set CHICKEN-SPEED .25
  set MAX-AGE 60
  set NEW-CHICKEN-T 8
  set EATING-RATE 1
  set STOMACH-LOSS-R 2
  set NUM-NEW-CHICK 1

  ;predator
  set WOLF-PACK 1
  set HAWK-PACK 1
  set HUNTER-SPEED .3
  set KILLER-SPEED 1
  ]

  if levels = 2 [

  set MOVE-SPEED .5
  set MAX-FOOD 20
  set ST-FOOD 8
  set FOOD-LOSS-RATE .3
  set GRASS 1
  set GROUND 0
  set WATER 2
  set BRIDGE 3

  ; chicken
  set MAX-STOMACH 80
  set CHICKEN-SPEED .25
  set MAX-AGE 60
  set NEW-CHICKEN-T 12
  set EATING-RATE 1
  set STOMACH-LOSS-R 2
  set NUM-NEW-CHICK 2

  ;predator
  set WOLF-PACK 1
  set HAWK-PACK 2
  set HUNTER-SPEED .3
  set KILLER-SPEED .2

  ]
  if levels = 3 [

  set MOVE-SPEED .5
  set MAX-FOOD 30
  set ST-FOOD 15
  set FOOD-LOSS-RATE .3
  set GRASS 1
  set GROUND 0
  set WATER 2
  set BRIDGE 3

  ; chicken
  set MAX-STOMACH 80
  set CHICKEN-SPEED .3
  set MAX-AGE 60
  set NEW-CHICKEN-T 12
  set EATING-RATE 1
  set STOMACH-LOSS-R 2
  set NUM-NEW-CHICK 1

  ;predator
  set WOLF-PACK 1
  set HAWK-PACK 3
  set HUNTER-SPEED .25
  set KILLER-SPEED .2

  ]

  set time 50
;  set level level + 1 ;we do this so that next time the initialize variable is done, its for level two unless the player looses

end

;======CHICKEN=========

to setup-chicken
  create-chickens 1 [
    baby-chick
  ]

end
to baby-chick ;makes a baby chicken
  set age 0
  set stomach MAX-STOMACH / 2
  set color yellow
  set shape "chicken"
  set size 1.5
  setxy random-xcor random-ycor
  set hungry? false
end

to chicken-time
  living
  multiplying
 dying
  moving


end

; first chickens lose food every half second
; chickens age every 1 second (not acurate to real aging)
; if the chicken is low on food it sets its state to hungry
; when it it hungry its behavior changes
to living
  every .5 [
    ask chickens [
      set stomach stomach - STOMACH-LOSS-R
    ]
  ]
  every 1 [
    ask chickens [
      set age age + 1
    ]
  ]
  ask chickens [
    ifelse stomach <= 5 [
      set hungry? true
    set color white
    ][
      if hungry? = true and stomach > 5 [
        set hungry? false
        set color yellow
      ]
    ]
  ]

end

to multiplying
  every NEW-CHICKEN-T [
    create-chickens NUM-NEW-CHICK [baby-chick]
    if level = 2 and NUM-NEW-CHICK < 2 [
      set NUM-NEW-CHICK (NUM-NEW-CHICK + 1)]

    if level = 3 and NUM-NEW-CHICK < 5 [
      set NUM-NEW-CHICK (NUM-NEW-CHICK + 1)
    ]
  ]

end

to dying
 ;old-age ;this can be added back if we want to reduce the number of chickens (but that is less fun)
  starving
end

;chickens die when they reach their max age
to old-age
  ask chickens [
    if age = MAX-AGE [die]
  ]
end

; if a chicken's stomach is empty it dies
to starving
  ask chickens [
    if stomach <= 0 [die]
  ]
end


to moving
  ;if a chicken isn't hungry it sees if there are any predators or farmers nearby
  ; a chicken avoids being on the same spot as another but also doesn't eat at a patch where one is already eating
  ;(allows for less chicken fights over food)
  ; if there are, it avoids them before eating
  ;if a chicken is hungry it ignores everything and goes for the food
  ask chickens [
    let things-to-avoid? any? (farmers in-radius 5) or any? (predators in-radius 2) or any? (other chickens in-radius .5)


    ifelse hungry? = false
    [
      ifelse not things-to-avoid? and [land-type = grass] of patch-here [
        eat-grass
        ;chicken speed is not called because they can stay where they are if the food is there
      ][
        eat-grass
        avoid-things
        fd CHICKEN-SPEED

      ]
    ][
    b-line
   fd CHICKEN-SPEED
    ]

  ]
    do-not-fall
 do-not-step-on-water

end

to do-not-step-on-water

  ;this prevents all the different agents from going on the water and prevents the animals from going on the bridge


  if any? patches with [land-type = WATER or land-type = BRIDGE] [
  ask patches with [land-type = water or land-type = BRIDGE][
    if any? chickens in-radius 1 [
      ask chickens in-radius 1 [
        face myself
          rt 130
        fd CHICKEN-SPEED
      ]
    ]
    if any? predators in-radius 1 [
     ask predators in-radius 1 [
      face myself
          rt 130
        fd HUNTER-SPEED
      ]
    ]
    if any? killers in-radius 1 [
     ask killers in-radius 1 [
        face myself
          rt 130
        fd HUNTER-SPEED
      ]
    ]
  ]
  ]
end



to do-not-fall
  ask patches with [(pxcor = max-pxcor) or (pycor = max-pycor) or (pycor = min-pycor) or (pxcor = min-pxcor)] [
    if any? chickens-here [
      ask chickens-here [
      face patch 0 0
        fd 1
      ]
    ]
  ]
end

to eat-grass
  ;this gets chickens to move towards grass without predators on it
  ifelse any? patches with [acceptable-grass-conditions ][
      face min-one-of patches with [acceptable-grass-conditions] [distance myself]
; if a chicken is now an a patch that is grass it will ask the patch to ask itself to eat
      ask patch-here [
        if land-type = grass [
        ask myself [eat]
        ]
      ]
    ]
  [
    ;if there is no food, the chicken attempts to approach the farmer (mimiking the idea that it knows where food is from)
    face min-one-of farmers [distance myself]
  ]
end

to-report acceptable-grass-conditions
  report land-type = grass and predators-here? = false and chickens-here? = false
end
to eat
  ;how a chicken eats
  if stomach <= MAX-STOMACH [


      set stomach stomach + EATING-RATE
      ask patch-here [
        set food food - EATING-RATE
      ]


  ]
end


to avoid-things

  ;avoid farmer comes before predator becuase avaiding the predator is the most important
  ifelse any? (predators in-radius 2) [
    avoid-predator
    ][
    ifelse any? farmers in-radius 5 [
      avoid-player
    ][
      avoid-chickens
    ]
  ]

end

to b-line
  eat-grass

end

to avoid-chickens
  if any? other chickens in-radius .5 [
    face min-one-of chickens [distance myself]
    rt 90
  ]
end
to avoid-predator
  face min-one-of predators [distance myself]
  rt 180
end
to avoid-player
  face min-one-of farmers [distance myself]
  rt 120
end



;======GROUND==========

to setup-ground
   ask patches [
    set land-type GROUND
    set food 0
    ground-color
  ]

  if level > 1 [add-waterland]

end

to add-waterland
; now the player has water to deal with and they can go over a bridge to escape the killer
  ask patch 0 0 [
    set land-type WATER
    set pcolor blue
    repeat 48 [
      ask one-of patches in-radius 4 with [land-type = GROUND and any? neighbors4 with [land-type = WATER]][
        set land-type WATER
        set pcolor blue
      ]

    ]
  ]
add-bridge
end
to add-bridge
  crt 1 [
    setxy 0 4
    set heading 180
    repeat 9[
      color-patch-bridge
      fd 1
    ]
    die
  ]
end

to color-patch-bridge
  ask patch-here [
    set pcolor white
    set land-type BRIDGE
  ]
end

to ground-color
   set pcolor (random-float 1) + brown + 1
end

;this is the standard command to make grass
;we ensure that if this is called there will at least be the standard amount of food
;the color of the patch roughly tells the player how much food was originally there
;until the food disapears (see grass-losing-food)
to be-grass
  set land-type GRASS
  set food (random MAX-FOOD) + ST-FOOD
  set pcolor scale-color green food MAX-FOOD 3
  ;shade-of lets us use the color

end

;this is how the farmer grows grass.
;if the land is ground, it tells it to be grass (have food)
;if the land is already grass, we ask will adding the standard amount of food
;be more than the max amount of food? if not food is then added
to grass-command
   ask patch-here [
      ifelse land-type = GROUND
       [ be-grass
      ][
        if (food + ST-FOOD) <= MAX-FOOD [set food food + ST-FOOD]
    ]
  ]

end

;The grass slowly loses the amount of food it holds over time (the food loss rate)
to grass-losing-food
  ask patches with [land-type = GRASS] [
    set predators-here? false
    set chickens-here? false

    set food food - FOOD-LOSS-RATE

    set pcolor scale-color green food MAX-FOOD 3

    if any? predators-here [set predators-here? true]
    if any? chickens-here [set chickens-here? true]

    if food <= 0 [
      ground-color
      set land-type GROUND
    ]
  ]
end

;======PERSON=========

to setup-farmer
  create-farmers 1 [
    set heading 90   ;this helps situate the player for the controls
    set PLAYER self  ; easier to call the player a player in the code
    set shape "person farmer"
    set size 3

  ]
end

; these movements allow the player to move within the world without falling off the mat or going in water
; (because of the max-cor part)
; with each step determined by the players movement speed there is a 40% chance they will grow
; food (grass) with the grass-command
to move-right
  ask PLAYER [if (xcor < max-pxcor) and no-go-patch-r = true [set xcor xcor + MOVE-SPEED]
   if random-float 1 < .40 [grass-command]
    ]
end

to move-up
  ask PLAYER [if ycor < max-pycor and no-go-patch-u = true [set ycor ycor + MOVE-SPEED]
    if random-float 1 < .40 [grass-command]
    ]
end

to move-down
  ask PLAYER [if ycor > min-pycor and no-go-patch-d = true [set ycor ycor - MOVE-SPEED]
    if random-float 1 < .40 [grass-command]
  ]
end
to move-left
  ask PLAYER [ if xcor > min-pxcor and no-go-patch-l = true [set xcor xcor - MOVE-SPEED]
    if random-float 1 < .40 [grass-command]
    ]


end

to-report no-go-patch-r
  ifelse [land-type != WATER] of patch-at-heading-and-distance 90 1  [report true]
    [report false]
end
to-report no-go-patch-u
   ifelse [land-type != WATER] of patch-at-heading-and-distance 0 1 [report true]
    [report false]
end
to-report no-go-patch-d
  ifelse [land-type != WATER] of patch-at-heading-and-distance 180 1[report true]
  [report false]
end
to-report no-go-patch-l
  ifelse [land-type != WATER] of patch-at-heading-and-distance 270 1 [report true]
  [report false]
end
;========Predators======

to make-wolf-pack
  create-killers WOLF-PACK [
    set shape "wolf"
    set color black
    set size 3
    set heading 90
    setxy min-pxcor max-pycor

  ]

end

to make-chicken-hawk
  create-predators HAWK-PACK [
    set shape "bird side"
    set color red
    set size 3
    set heading 90
    setxy max-pxcor max-pycor
    set rangled? false

  ]
end

to predator-event

  ; at 35 seconds the level's specific amount of chickenhawks are made
  if time < (40) [
  every 27 [
      make-chicken-hawk
    ]

    if count chickens > 0 [

      run-from-farmer
      if any? links [
        ask player [
          ask links [
            ask other-end [set rangled? true] ;this is for if a predator has been rangled by a farmer, it will now be on a line to the farmer
          ]
        ]

      ]

      hunt chickens predators HUNTER-SPEED
    ]
  ]
  if level > 1 [

    if time < 20 [
      every 22 [
        make-wolf-pack]
      hunt farmers killers KILLER-SPEED
    ]
  ]

end


to hunt [prey hunters speed]
; this is a general hunting statement for the hunters (wolves and chickenhawks)
    if count hunters > 0[
    ask hunters [
      ifelse rangled? = true [stop][ ;now the rangles predator wont kill
       if count prey > 0 [
    face min-one-of prey [distance myself]
    if any? prey in-radius 1 [
      kill prey
      ]
    fd speed
      ]
      ]
    ]
  ]


end
to kill [prey]
  ;the pry is passed in from the hunt procedure
  ask one-of prey in-radius 1 [die]
end

to run-from-farmer
  ;the chicken-hawk tries to avoid the farmer (self preservation)
  ask predators [
    if any? farmers in-radius 3 [
      avoid-player
      fd HUNTER-SPEED
    ]
  ]
end



to rangle-predator
  ;farmers have to "catch" (link-with) a predator before it can be killed
if [any? predators in-radius 7] of player [
    ask player [
      create-link-to one-of predators [tie]
      ask links [
        set color yellow + 2
        set thickness .25
      ]
     ]
  ]

end


to-report predators-in-link
  ;needed for next procedure
  report count [my-links] of player
end
to bye-bye-bird
   ;the player must catch all the hawks before they can be killed
  if predators-in-link = HAWK-PACK [
    ask player [
       ask links [ ask other-end [die]
      ]
    ]
  ]
end

;====how the levels start and end===========
to-report time-left
  ;lets the player know how much time they have in the level


  ;want to say if the level is three then report the time as infinity



  ifelse level != 3 [
    ifelse time > 1 [
      report time
    ][
      report "level complete"
    ]
  ][
    report "infinite time"
  ]

end
to timert
  if level != 3 [ every 1 [set time time - 1]]

end
to level-over
  ;if the player completes the level they get a green screen and move to the next level
  ; but if all the chickens die, it's game-over
  ifelse level != 3 [
  ifelse time <= 0 [
    clear-patches
    clear-turtles
    ask patches [set pcolor green]
    wait 2
    set time 0
    next-level

  ][
      end-conditions
  ]
  ][
end-conditions
  ]
end
to end-conditions
     if count chickens < 1 and time <= 45 [
     game-over
    ]
    if count farmers = 0 [game-over]
end
to game-over
  ; level one is called to setup when the player fails
   wait 1
   ca
   ask patches [set pcolor red]
   wait 2
   setup

end

to next-level
  ;needed to do this because to keep moving as the player the go button has to be in a forever loop
  ;(instead of having conditional iteration for the levels),
  ; so when the next level is called it needs to be setup but the world has to be cleared
  ; so the level number isn't remembered when cleared but if we setup the next level then clear, it is set up properly
  ifelse level = 1 [
    setup-level-2
  ]
 [
    setup-level-3
  ]
end
to setup-level-2
  ca
  set level 2 ;we do this here because each level had different initialized variables and it's easier to change as a whole command center for coder
  initialize-var level
  setup-farmer
  setup-ground
  setup-chicken
  set list1 [0]
  reset-ticks

end
to setup-level-3
  ca
  set level 3 ;we do this here because each level had different initialized variables and it's easier to change as a whole command center for coder
  initialize-var level
  setup-farmer
  setup-ground
  setup-chicken
  set list1 [0]
  reset-ticks


end

;=====reporters for player=======

to-report count-chickens

  report count chickens
end

to-report highscore
    if count-chickens > item 0 list1 [
    set list1 replace-item 0 list1 count-chickens
  ]
  report list1

end
@#$#@#$#@
GRAPHICS-WINDOW
210
10
614
415
-1
-1
12.0
1
10
1
1
1
0
0
0
1
-16
16
-16
16
0
0
1
ticks
30.0

BUTTON
53
248
138
281
NIL
move-up
NIL
1
T
OBSERVER
NIL
W
NIL
NIL
1

BUTTON
43
313
148
346
NIL
move-down
NIL
1
T
OBSERVER
NIL
S
NIL
NIL
1

BUTTON
101
280
203
313
NIL
move-right
NIL
1
T
OBSERVER
NIL
D
NIL
NIL
1

BUTTON
0
280
92
313
NIL
move-left
NIL
1
T
OBSERVER
NIL
A
NIL
NIL
1

BUTTON
78
10
156
43
NIL
setup 
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
37
52
180
85
READY PLAYER 1 ?
go
T
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
21
347
164
380
NIL
rangle-predator
NIL
1
T
OBSERVER
NIL
Q
NIL
NIL
1

MONITOR
0
165
91
210
NIL
time-left
17
1
11

BUTTON
40
384
154
417
NIL
bye-bye-bird
NIL
1
T
OBSERVER
NIL
E
NIL
NIL
1

BUTTON
48
91
164
124
skip to level 2
setup-level-2
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
48
133
164
166
skip to level 3
setup-level-3
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

MONITOR
0
213
96
258
chicken count
count-chickens
17
1
11

MONITOR
91
166
213
211
high-score chickens
highscore
17
1
11

@#$#@#$#@
## CHICKEN FEED (the game)

An awsome game where your job is to feed chickens, except the chickens don't like you so you need to keep moving to feed them and keep them alive. There will be chicken-hawks that try to eat your chickens and a wolf that trys to eat you later on.

## what happens: level 1

learn to feed your chickens and protect them from the chicken-hawk. You complete the level when the time runs out at you get a green screen. If your chickens all die, you get a red screen and the level resets. The chickens do get hungry, when they are starving they turn right and go straight for the food. They are also more scared of the chicken-hawks than you.

## what happens: level 2

Now there are two chicken-hawks and more chickens to compete for the food. There is also a wolf that will try to eat you, try escaping to the bridge where it can't reach you.

## what happens: level 3

It's a free for all, no preadators will try to eat you or your chickens. Try seeing how many chickens you can keep alive at a given time (recorded in you high score).

## button explanations 

  * setup
    * this will setup the first level 
  * ready-player-one?
    * this will start the fist level and, if you make it, will load the other levels after completion 
  * movement
    * use WASD
  * rangle-preador 
    * when you get close enough to a chicken-hawk you can rangle it with your laso 
  * bye-bye-bird
    * once you have rangled **ALL** predators you can get rid of them
  * skip to level 2 and three
    * you can skip to other levels (but real players get through them all in order)

## monitors

Time-left tell the player how much longer they must survive
chicken count says how many chickens there are
high-score chickens tells you the highest number of chickens that was alive at one point

## design process

First I started with the idea of the game. I wanted to create a game where you feed chickens and it would be somewhat challenging. So I had the chickens avoid the farmer but try to get food. Next I wanted a predator to eat the chickens and to give the player something else to do. To stop the predators, the player has to tie to them (rangle them) then can kill them. The chickens also run from the predator and prioritize running from the predator over avoiding the farmer (unless starving, where the chickens just try to eat).
The food starts to disapear once placed so the player has to keep moving to keep "throwing out" food to the chickens. 
The wolf in level two is to make life hard for the player. Its only goal is to eat the player, but the player can escape it by going on the bridge where non of the other agents can go.

## Challeging part

The hardest part was getting the chickens to avoid a lot of things. Going from lowest priority to highest, the chickens try to not be on top of each other. They then avoid the farmer, and next avoid the chicken-hawk. Finally if the chicken is starving it just goes strait for the food (to give it a vaugly realistic sense of preservation).

## CREDITS AND REFERENCES

I refenced the code for WASD from the pong game we made in class and modified it a little.
@#$#@#$#@
default
true
0
Polygon -7500403 true true 150 5 40 250 150 205 260 250

airplane
true
0
Polygon -7500403 true true 150 0 135 15 120 60 120 105 15 165 15 195 120 180 135 240 105 270 120 285 150 270 180 285 210 270 165 240 180 180 285 195 285 165 180 105 180 60 165 15

arrow
true
0
Polygon -7500403 true true 150 0 0 150 105 150 105 293 195 293 195 150 300 150

bird side
false
0
Polygon -7500403 true true 0 120 45 90 75 90 105 120 150 120 240 135 285 120 285 135 300 150 240 150 195 165 255 195 210 195 150 210 90 195 60 180 45 135
Circle -16777216 true false 38 98 14

box
false
0
Polygon -7500403 true true 150 285 285 225 285 75 150 135
Polygon -7500403 true true 150 135 15 75 150 15 285 75
Polygon -7500403 true true 15 75 15 225 150 285 150 135
Line -16777216 false 150 285 150 135
Line -16777216 false 150 135 15 75
Line -16777216 false 150 135 285 75

bug
true
0
Circle -7500403 true true 96 182 108
Circle -7500403 true true 110 127 80
Circle -7500403 true true 110 75 80
Line -7500403 true 150 100 80 30
Line -7500403 true 150 100 220 30

butterfly
true
0
Polygon -7500403 true true 150 165 209 199 225 225 225 255 195 270 165 255 150 240
Polygon -7500403 true true 150 165 89 198 75 225 75 255 105 270 135 255 150 240
Polygon -7500403 true true 139 148 100 105 55 90 25 90 10 105 10 135 25 180 40 195 85 194 139 163
Polygon -7500403 true true 162 150 200 105 245 90 275 90 290 105 290 135 275 180 260 195 215 195 162 165
Polygon -16777216 true false 150 255 135 225 120 150 135 120 150 105 165 120 180 150 165 225
Circle -16777216 true false 135 90 30
Line -16777216 false 150 105 195 60
Line -16777216 false 150 105 105 60

car
false
0
Polygon -7500403 true true 300 180 279 164 261 144 240 135 226 132 213 106 203 84 185 63 159 50 135 50 75 60 0 150 0 165 0 225 300 225 300 180
Circle -16777216 true false 180 180 90
Circle -16777216 true false 30 180 90
Polygon -16777216 true false 162 80 132 78 134 135 209 135 194 105 189 96 180 89
Circle -7500403 true true 47 195 58
Circle -7500403 true true 195 195 58

chicken
false
0
Circle -2674135 true false 45 120 30
Circle -7500403 true true 72 104 156
Circle -7500403 true true 51 46 108
Polygon -1184463 true false 60 105 75 135 30 120 60 105
Rectangle -16777216 true false 75 90 90 105
Circle -2674135 true false 75 30 30
Circle -2674135 true false 75 15 30
Circle -2674135 true false 90 15 30

circle
false
0
Circle -7500403 true true 0 0 300

circle 2
false
0
Circle -7500403 true true 0 0 300
Circle -16777216 true false 30 30 240

cow
false
0
Polygon -7500403 true true 200 193 197 249 179 249 177 196 166 187 140 189 93 191 78 179 72 211 49 209 48 181 37 149 25 120 25 89 45 72 103 84 179 75 198 76 252 64 272 81 293 103 285 121 255 121 242 118 224 167
Polygon -7500403 true true 73 210 86 251 62 249 48 208
Polygon -7500403 true true 25 114 16 195 9 204 23 213 25 200 39 123

cylinder
false
0
Circle -7500403 true true 0 0 300

dot
false
0
Circle -7500403 true true 90 90 120

egg
false
0
Circle -7500403 true true 96 76 108
Circle -7500403 true true 72 104 156
Polygon -7500403 true true 221 149 195 101 106 99 80 148

face happy
false
0
Circle -7500403 true true 8 8 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Polygon -16777216 true false 150 255 90 239 62 213 47 191 67 179 90 203 109 218 150 225 192 218 210 203 227 181 251 194 236 217 212 240

face neutral
false
0
Circle -7500403 true true 8 7 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Rectangle -16777216 true false 60 195 240 225

face sad
false
0
Circle -7500403 true true 8 8 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Polygon -16777216 true false 150 168 90 184 62 210 47 232 67 244 90 220 109 205 150 198 192 205 210 220 227 242 251 229 236 206 212 183

fish
false
0
Polygon -1 true false 44 131 21 87 15 86 0 120 15 150 0 180 13 214 20 212 45 166
Polygon -1 true false 135 195 119 235 95 218 76 210 46 204 60 165
Polygon -1 true false 75 45 83 77 71 103 86 114 166 78 135 60
Polygon -7500403 true true 30 136 151 77 226 81 280 119 292 146 292 160 287 170 270 195 195 210 151 212 30 166
Circle -16777216 true false 215 106 30

flag
false
0
Rectangle -7500403 true true 60 15 75 300
Polygon -7500403 true true 90 150 270 90 90 30
Line -7500403 true 75 135 90 135
Line -7500403 true 75 45 90 45

flower
false
0
Polygon -10899396 true false 135 120 165 165 180 210 180 240 150 300 165 300 195 240 195 195 165 135
Circle -7500403 true true 85 132 38
Circle -7500403 true true 130 147 38
Circle -7500403 true true 192 85 38
Circle -7500403 true true 85 40 38
Circle -7500403 true true 177 40 38
Circle -7500403 true true 177 132 38
Circle -7500403 true true 70 85 38
Circle -7500403 true true 130 25 38
Circle -7500403 true true 96 51 108
Circle -16777216 true false 113 68 74
Polygon -10899396 true false 189 233 219 188 249 173 279 188 234 218
Polygon -10899396 true false 180 255 150 210 105 210 75 240 135 240

house
false
0
Rectangle -7500403 true true 45 120 255 285
Rectangle -16777216 true false 120 210 180 285
Polygon -7500403 true true 15 120 150 15 285 120
Line -16777216 false 30 120 270 120

leaf
false
0
Polygon -7500403 true true 150 210 135 195 120 210 60 210 30 195 60 180 60 165 15 135 30 120 15 105 40 104 45 90 60 90 90 105 105 120 120 120 105 60 120 60 135 30 150 15 165 30 180 60 195 60 180 120 195 120 210 105 240 90 255 90 263 104 285 105 270 120 285 135 240 165 240 180 270 195 240 210 180 210 165 195
Polygon -7500403 true true 135 195 135 240 120 255 105 255 105 285 135 285 165 240 165 195

line
true
0
Line -7500403 true 150 0 150 300

line half
true
0
Line -7500403 true 150 0 150 150

pentagon
false
0
Polygon -7500403 true true 150 15 15 120 60 285 240 285 285 120

person
false
0
Circle -7500403 true true 110 5 80
Polygon -7500403 true true 105 90 120 195 90 285 105 300 135 300 150 225 165 300 195 300 210 285 180 195 195 90
Rectangle -7500403 true true 127 79 172 94
Polygon -7500403 true true 195 90 240 150 225 180 165 105
Polygon -7500403 true true 105 90 60 150 75 180 135 105

person farmer
false
0
Polygon -7500403 true true 105 90 120 195 90 285 105 300 135 300 150 225 165 300 195 300 210 285 180 195 195 90
Polygon -1 true false 60 195 90 210 114 154 120 195 180 195 187 157 210 210 240 195 195 90 165 90 150 105 150 150 135 90 105 90
Circle -7500403 true true 110 5 80
Rectangle -7500403 true true 127 79 172 94
Polygon -13345367 true false 120 90 120 180 120 195 90 285 105 300 135 300 150 225 165 300 195 300 210 285 180 195 180 90 172 89 165 135 135 135 127 90
Polygon -6459832 true false 116 4 113 21 71 33 71 40 109 48 117 34 144 27 180 26 188 36 224 23 222 14 178 16 167 0
Line -16777216 false 225 90 270 90
Line -16777216 false 225 15 225 90
Line -16777216 false 270 15 270 90
Line -16777216 false 247 15 247 90
Rectangle -6459832 true false 240 90 255 300

plant
false
0
Rectangle -7500403 true true 135 90 165 300
Polygon -7500403 true true 135 255 90 210 45 195 75 255 135 285
Polygon -7500403 true true 165 255 210 210 255 195 225 255 165 285
Polygon -7500403 true true 135 180 90 135 45 120 75 180 135 210
Polygon -7500403 true true 165 180 165 210 225 180 255 120 210 135
Polygon -7500403 true true 135 105 90 60 45 45 75 105 135 135
Polygon -7500403 true true 165 105 165 135 225 105 255 45 210 60
Polygon -7500403 true true 135 90 120 45 150 15 180 45 165 90

sheep
false
15
Circle -1 true true 203 65 88
Circle -1 true true 70 65 162
Circle -1 true true 150 105 120
Polygon -7500403 true false 218 120 240 165 255 165 278 120
Circle -7500403 true false 214 72 67
Rectangle -1 true true 164 223 179 298
Polygon -1 true true 45 285 30 285 30 240 15 195 45 210
Circle -1 true true 3 83 150
Rectangle -1 true true 65 221 80 296
Polygon -1 true true 195 285 210 285 210 240 240 210 195 210
Polygon -7500403 true false 276 85 285 105 302 99 294 83
Polygon -7500403 true false 219 85 210 105 193 99 201 83

square
false
0
Rectangle -7500403 true true 30 30 270 270

square 2
false
0
Rectangle -7500403 true true 30 30 270 270
Rectangle -16777216 true false 60 60 240 240

star
false
0
Polygon -7500403 true true 151 1 185 108 298 108 207 175 242 282 151 216 59 282 94 175 3 108 116 108

target
false
0
Circle -7500403 true true 0 0 300
Circle -16777216 true false 30 30 240
Circle -7500403 true true 60 60 180
Circle -16777216 true false 90 90 120
Circle -7500403 true true 120 120 60

tree
false
0
Circle -7500403 true true 118 3 94
Rectangle -6459832 true false 120 195 180 300
Circle -7500403 true true 65 21 108
Circle -7500403 true true 116 41 127
Circle -7500403 true true 45 90 120
Circle -7500403 true true 104 74 152

triangle
false
0
Polygon -7500403 true true 150 30 15 255 285 255

triangle 2
false
0
Polygon -7500403 true true 150 30 15 255 285 255
Polygon -16777216 true false 151 99 225 223 75 224

truck
false
0
Rectangle -7500403 true true 4 45 195 187
Polygon -7500403 true true 296 193 296 150 259 134 244 104 208 104 207 194
Rectangle -1 true false 195 60 195 105
Polygon -16777216 true false 238 112 252 141 219 141 218 112
Circle -16777216 true false 234 174 42
Rectangle -7500403 true true 181 185 214 194
Circle -16777216 true false 144 174 42
Circle -16777216 true false 24 174 42
Circle -7500403 false true 24 174 42
Circle -7500403 false true 144 174 42
Circle -7500403 false true 234 174 42

turtle
true
0
Polygon -10899396 true false 215 204 240 233 246 254 228 266 215 252 193 210
Polygon -10899396 true false 195 90 225 75 245 75 260 89 269 108 261 124 240 105 225 105 210 105
Polygon -10899396 true false 105 90 75 75 55 75 40 89 31 108 39 124 60 105 75 105 90 105
Polygon -10899396 true false 132 85 134 64 107 51 108 17 150 2 192 18 192 52 169 65 172 87
Polygon -10899396 true false 85 204 60 233 54 254 72 266 85 252 107 210
Polygon -7500403 true true 119 75 179 75 209 101 224 135 220 225 175 261 128 261 81 224 74 135 88 99

wheel
false
0
Circle -7500403 true true 3 3 294
Circle -16777216 true false 30 30 240
Line -7500403 true 150 285 150 15
Line -7500403 true 15 150 285 150
Circle -7500403 true true 120 120 60
Line -7500403 true 216 40 79 269
Line -7500403 true 40 84 269 221
Line -7500403 true 40 216 269 79
Line -7500403 true 84 40 221 269

wolf
false
0
Polygon -16777216 true false 253 133 245 131 245 133
Polygon -7500403 true true 2 194 13 197 30 191 38 193 38 205 20 226 20 257 27 265 38 266 40 260 31 253 31 230 60 206 68 198 75 209 66 228 65 243 82 261 84 268 100 267 103 261 77 239 79 231 100 207 98 196 119 201 143 202 160 195 166 210 172 213 173 238 167 251 160 248 154 265 169 264 178 247 186 240 198 260 200 271 217 271 219 262 207 258 195 230 192 198 210 184 227 164 242 144 259 145 284 151 277 141 293 140 299 134 297 127 273 119 270 105
Polygon -7500403 true true -1 195 14 180 36 166 40 153 53 140 82 131 134 133 159 126 188 115 227 108 236 102 238 98 268 86 269 92 281 87 269 103 269 113

x
false
0
Polygon -7500403 true true 270 75 225 30 30 225 75 270
Polygon -7500403 true true 30 75 75 30 270 225 225 270
@#$#@#$#@
NetLogo 6.2.0
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
default
0.0
-0.2 0 0.0 1.0
0.0 1 1.0 0.0
0.2 0 0.0 1.0
link direction
true
0
Line -7500403 true 150 150 90 180
Line -7500403 true 150 150 210 180
@#$#@#$#@
0
@#$#@#$#@
