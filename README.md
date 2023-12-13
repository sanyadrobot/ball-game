# ball-game
# Ball Game using Unity ECS + Mono

## Task Description

### Gameplay
On the screen, the player ball is in the lower left corner, and the target is in the upper right corner, where the ball should hit. The path is blocked by obstacles. The ball player creates shots due to its size, you need to clear the path so that the ball player jumps along the cleared path to the final goal.

### Prototype
When tapping, the shot ball begins to separate from the player ball. The player ball decreases in size in proportion to how the size of the shot ball grows and depends on the holding time (the longer, the more). When released, the shot flies towards the target, and when it hits the first obstacle, it "infects" the obstacles in its radius, causing them to explode.
The power of the shot - the radius of “infection” depends on the size of the shot ball; the larger it is, the greater the radius. The closer the obstacles are to each other, the easier it is to infect them. On singles you need to take small shots so as not to waste the entire size of the player's ball.

After clearing the ball area, the player moves forward accordingly towards the target. At the end, his size is reduced, but he clearly should have enough passage to freely pass between obstacles, jumping in the center of the path. The track decreases with the size of the ball.

There should be a door at the end. The door opens when the ball approaches 5 meters towards it.
If the player holds the tap for too long, and the player’s entire ball is pumped into the shot (select the minimum critical size) - loss. If there are not enough shots to clear the way, you lose.

Initially, the size of the ball should be enough with a margin of 20% for passage.

https://youtu.be/WSxe88-ElTc
