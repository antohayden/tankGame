#C\# Tank Game

###Overview
This was a project as part of Object Orietated programming module in second year of Computer Science course.
It used the (now deprecated) XNA game library for Visual Studio and was done to learn the basic principles of OO programming as well as elements of game programming. 
The player tank model was created in  
The project involved the use of heightmaps to generate terrain as well as some simple physics for the player controlled tank to move over it smoothly.
It also included some basic collision detection in the form of the tank not being able to move past the walls in the terrain, as well as projectile interaction with enemy tanks.
A build exists [here](http://antohayden.com/downloads/tankgame.zip)

###Description:
3-D FPS: Player is situatated within a tank. Object is to kill as many enemies as possible before dying.


###Controls:
Targeting is controlled by mouse movement.
**W**: Forward
**A**: Turn Left
**S**: Back
**D**: Turn Right
**LeftClick**: Fire Bullet

Bullets have a 2 second reload timer.

##Note:
The game is unfinished and there are some missing graphics and functionality. 
Enemies do not emit a projectile when firing. Simply the act of being in "firing range" will cause the firing
sound to play and the player to take damage ( even if the enemy is facing away from play ).
Scores are saved to a text file within the exe folder


