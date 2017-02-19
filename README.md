#Snake 3D

#Game Overview

##Game Genre
Casual game

##Game Summary
Snake eats fruit in an enclosed shape and avoid obstacles. Fruits spawns at random position within the playground. There are two main parts of game, Custom levels and Procedural level. In Custom level, snake has to avoid obstacle in prebaked playground and where as in Procedural level, enclosed shape is a regular polygon with obstacles spawning at random position within playground.

##Platform
This game is developed using Unity3D game engine. Game was initially developed to be played over android platform, but game’s simple swipe control allows the gameplay to function with arrow key of joystick controller or keyboard.

#Game Mechanics

##Main Technical Requirements
The main technical requirement for this game is algorithm to procedurally generate a regular polygon playground and generating a random point inside that polygon. Other aspect of the game includes: instantiating objects from a list of object, snake movement, animations, level designing, vector math, delegates and events, etc. Also, as the game progress, difficulty should increase. In case of Custom level, we designed the level in such a way difficulty increases after each level and in case of Procedural level, we decreased the free area available for the snake.


##Game Flow
The game will begin at Main Menu screen where player can choose between Custom level and Procedural level. Main Menu screen also has a text box which describes each objectives and characteristics of each level. After selecting one of the two level, player may click on ‘Play’ Button.

If player chooses Custom level, playground corresponding to level 1 is instantiated along with Snake head and two tails. After this a Fruit is instantiated at random position inside the playground and snake is assigned with random velocity in 4 planar directions (up, down, left, right). Each time Snake eats a fruit, another fruit is instantiated at random position inside playground and a tail is increased in Snake. Snake has to eat a predefined value of minimum number of fruits to eat to surpass any level. If snake eats equal to requirement, player has now option to jump to next level or continue playing this level. If player chooses to go next level, next playground is instantiated and cycle repeats till all Custom levels are completed, after which player wins the game.

If player chooses Procedural level, an equilateral triangle (minimum sided polygon) is generated along with Snake head and two tails. After this a Fruit is instantiated at random position inside polygon. Snake is assigned with random velocity in 4 planar directions (up, down, left, right). Each time Snake eats a Fruit, another Fruit and an Obstacle are instantiated at two different random position inside the polygon and a tail is increased in Snake. Snake has to eat a predefined value of minimum number of fruits to eat to surpass any level. If snake eats equal to requirement, player has now option to jump to next level or continue playing this level. If player chooses to go next level, polygon with +1 side is generated cycle repeats endlessly.

##Difficulty level
As the player moves from level 1 to next, difficult increases in both game types. In Custom level, difficulty is increased in terms of number of obstacle and their position in playground.
In Procedural level, difficulty is increased as area within the polygon is decreased, and thus random spawning of obstacles are more clustered.

##Geometry
One of the most used concept throughout project is of Vector Mathematics and Trigonometry. Dot products of vectors to find head on collision (see Appendix 1), cross product of vectors to find is the point inside a polygon (see Appendix 2) and most importantly, generating a regular polygon with only two parameters: number of sides and inscribed circle radius (see Appendix 3).

#Motion and Animation

##Snake
Considering the Casual genre of the game, I wanted to make the Snake character very friendly and cute. Thus, I decided to give it a spring like body with unconstrained parts. Idea to use this motion was based on Ducklings who are following the one duckling in front of them and leading them is the Mama Duck.
[image]

##Fruits
Animation of Fruit is simple bouncy ball with volume constant and which follows Newton’s law of motion.
[image]

##Stars over head
This idea was followed by the cute duck and duckling motion. As the Snake collides with an obstacle it stops and stars starts rotating over its heads (much like what we see in cartoons).
[image]

#Technical Flow

##Architectural Design
![alt text](https://raw.githubusercontent.com/nrjnicks/snake3D/master/Image/ArchitecturalDesign.png "ArchitecturalDesign")

##Technical Flow in words
Game starts with initializing variable which should be initialized once throughout the game like Class objects, tag names, setting parents/child, delegates and events, game music etc. by init() function. UI shows information of two game types (Custom or Procedural) and then waits for player’s response to UI which decide the game type and reset parameters in GameManagerClass like level, etc.

If player chooses Custom level, custom level parameters in LevelManagerClass is initialized and Playground, Snake head and body are instantiated in game world. Snake head is assigned a random velocity and then Fruit is instantiated at point which is inside the playground (Appendix 2) and below which no obstacle is present (if obstacle is present, Snake will not be able to eat it) (Appendix 3). Fruit is instantiated and a bouncy animation is added to it. As Snake eats a fruit, a callback is called, which calls function to play eating sound clip (SoundManagerScript) and instantiate another fruit at another ‘valid’ random position (LevelManagerClass). If Snake collides with the wall, rotating stars instantiate over its head anad a callback alerts UIManagerClass to display ‘Restart’ button on screen and SoundManagerClass to play crash sound. If Snake eats more than minimum requirement of, a callback alerts LevelManagerClass which callback another function of UIManagerClass which set active button on screenby clicking which player can jump to next level. If player decides to not click, he/she can still play this level till snake collides with obstacle. If player clicks on that button, UIManagerClass calls GameManagerClass to upgrade and update level and instantiate next playground in list. And thus, cycle repeats till max level (count of playground in list of LevelManagerClass) for Custom level, after which a game screen congratulating player is displayed.

If player chooses Procedural level, procedural level parameters in LevelManagerClass is initialized and a polygon of 3 sides (triangle) of side length depending on radius of inscribing a circle, is generated. Along with this, Snake head and body are instantiated in game world. Snake head is assigned a random velocity and then Fruit is instantiated at point which is inside the playground (Appendix 2). Fruit is instantiated and a bouncy animation is added to it. As Snake eats a fruit, a callback is called, which calls function to play eating sound clip (SoundManagerScript) and instantiate another fruit at another ‘valid’ random position (LevelManagerClass). This time, LevelManagerClass also instantiate a movable obstacle as part of the game. Snake can also use its tail to move the obstacle away from the fruit. If Snake collides with the obstacle, rotating stars instantiate over its head and a callback alerts UIManagerClass to display ‘Restart’ button on screen and SoundManagerClass to play crash sound. If Snake eats more than minimum requirement of, a callback alerts LevelManagerClass which callback another function of UIManagerClass which set active button on screen by clicking which player can jump to next level. If player decides to not click, he/she can still play this level till snake collides with obstacle. If player clicks on that button, UIManagerClass calls GameManagerClass to upgrade and update level and generate another polygon with +1 sides as previous level. This cycle, thus keeps repeating. 

#Requirements

1. Make Sure Tags provided in Scripts are configured. If not, configure them from Edit> Project Setting> Tags and Layers.
Current Tags: “Wall” for obstacles, “SnakeBody” for Snake tails and “Fruit” for fruits.
2. Place all empty GameObjects to world location (0,0,0). Ignoring this will not change the way our game is working, but it is good practice to place empty GameObjects at origin.

#Public Variables open to developer/designer/tester

##LevelManagerClass
1. Inscribed radius of procedural polygon: Radius of circle which can be inscribed the polygon for Procedural playground generation.
2. Playground list: list of all custom made playground for Custom levels.
3. Max procedural level: assigning a winning level for Procedural levels. (0 or negative means-not defined)
4. Min fruit for next level: how many fruits to consumed by snake to jump to next level

##SnakeManagerClass
1. Snake head prefab: pre-fabricated head of snake
2. Snake hail prefab: pre-fabricated tail of snake
3. Collision star prefab: pre-fabricated animation object of rotating stars
4. Speed: speed by which snake will move
5. Length of snake body: length of body to estimate distance to instantiate next body at
6. Min distance between two snake body: distance between two body parts (or ducklings (as mentioned way above))

##FruitManagerClass
1. Fruit Types: list of all type of fruits which can be instantiated

##UIManagerClass
1. [all public variable are pointers to GameObjects]

##SoundManagerClass
1. Music volume: volume of game music 
2. Sound volume: volume of game sound
3. [Other sound clips: sound clips for different events]

#End Note
I used name of variables and methods such everything in intuitive and easy to understand. I hope you find it easy to read and comply.


