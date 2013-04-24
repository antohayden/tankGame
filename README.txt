Unzip and open in VS10+

OOP assignment: Overview of TankGame

Description:

3-D FPS: Player is situatated within a tank. Object is to kill as many enemies as possible before dying.


Controls:
Targeting is controlled by mouse movement.
W: Forward
A: Turn Left
S: Back
D: Turn Right
LeftClick: Fire Bullet
Bullets have a 2 second reload timer.

Note:
Enemies do not emit a projectile when firing. Simply the act of being in "firing range" will cause the firing
sound to play and the player to take damage ( even if the enemy is facing away from play ).

Scores are saved to a text file on the local hard drive ( normally found in C:\scores.txt ).

Known Issues/Bugs:
1: Rarely on build, the camera will float to one side of screen. Requires restart.
2: Rarely, the players health will drop from a single enemies shot
3: Occasionally, the enemies will loop around the terrain edges. AI problem, will correct if the player is near.
4: Enemies, when following a player will "jitter" when trying adjust the direction they need to move




