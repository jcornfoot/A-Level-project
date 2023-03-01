This file is used to define the abstracted project in layers from the basic features up to visuals

[] - basic test map // simple area to spawn characters with a floor //
[] - simple player character // black rectangle to represent the players sprite //
[] - keyboard input // reads the currently pressed keys on the users keyboard //
[] - mouse input //reads the position of the mouse on the screen //
[] - cursor // a simple pixelated aiming reticle to show where the player is aiming relative to the character // 
[] - right and left movement // moves the cube right or left at the same speed //
[] - jumping and gravity // character moves up then decelerates till it is at a set height, controlled by a 'jump speed' variable. The player then accelerates towards the ground and returns to its original position //
[] - crouching // half the size of the cube //
[] - basic gun // hovers around the player when holding right click on the mouse, removes itself when rmb is depressed // 
[] - basic character constraints // add a colored face to the character to represent the front, this front face always faces the mouse cursor // 
[] - basic character restraints 2 // once the mouse cursor crosses to the other side of the sprite, invert the sprite to face the cursor //
[] - basic character restraints 3 // also invert the gun sprite at the same time the player sprite flips so the gun is constantly the correct way up // 
[] - map objects // add solid, immovable, objects into the test map and fix collisions with them //
[] - physics objects // add physics objects into the map as their own separate entity with physics constraints e.g. gravity and friction //
[] - physics objects 2 // make physics objects a template so they can be spawned into a map individually and can have their properties independantly edeted // 
[] - health // create a system for health //
[] - physics object 3 // create a breakable physics object that removes itself from the map after reaching zero health //
[] - bullet sprite // create a simple sprite for a standard bullet // 
[] - bullet mechanics // fire bullets from the end of the gun at a constant speed and remove them upon collision with a solid object // 
[] - bullet mechanics 2 // remove the effects of gravity from the sprites create a system to report back on the exact object they hit // 
[] - bullet mechanics 3 // remove health from the object they hit //
