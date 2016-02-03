# MapEditor
2D map using Voronoi Diagrams as a start.
This project was the item I was working on when Yogventures came to an ubrupt end.
The goal of the project was to create an addon to Yogventures that would allow players to generate their own landscapes.
Yogventures was, at it heart, a game about allowing players to build their own adventures. The engine was largely a proceduraly generated voxel engine. The major downside to that, is that by and large players couldn't predict what kind of world they would recieve when they started a new adventure. This is fine if you are building a game about exploration, but this was supposed to be about player created adventures that they could share with their friends.

This Map Editor would have been the first step to help players create maps quickly and easily for their adventures. It uses some clever map generation tricks inspired by http://www-cs-students.stanford.edu/~amitp/game-programming/polygon-map-generation/

This was an unfinished project that I would someday like to revisit.

I'm uploading it here as an example of code I wrote while working on Yogventures as I'm unable to share the rest of it for three reasons. 1. I signed a contract with the Yogscast LTD stating that I wouldn't share it.
2. The actual game code is so old now it doesn't actually run because I abandonded it so long ago I no longer have an actual working version.
3. It would take far too much time to get it back into working order.


The project can be ran in Unity v 5.3.1f that is the only version it has been tested in so far.
When run, there are a few fields and buttons on the left hand side of the screen.
These fields affect the voronoi diagram and subsequently the resultant map that's generated.
Try changing the seed first, that's the most robust way to see varied maps.
The NumSites field will make a more detailed map at the expense of speed / memory. It does this by increasing the number of cells generated.
Most of the other fields just affect various aspects of procedural noise.

The Build Button will generate and re-generate the map.

The button under the label Current Biom Brush: will allow the user to select a "Biome Brush" this feature was to allow users to paint their maps after the generator had given them something to start with.

Please note, in previous versions of Unity you could have a concave mesh collider that was also a trigger, this is no longer the case. As such the painting feature works but is buggy because the collision detection for each cell is too large due to the collider being marked as convex when it really isn't.

Finally this project was unfinished and unpolished to give an idea of what work in progress looked like then. In the future I aspire to adopt much better coding standards. I would also like to upgrade the UI and finish this project one day, procedural geometry mixed with user interaction is a powerful combination for players to generate their own content.
