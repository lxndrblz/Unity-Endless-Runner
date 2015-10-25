**Unity Endless Runner Script**
Create an endless runner in Unity 3D
======
With the wish to create my own endless runner game in Unity 3D. I started this project in December 2014. My attemp was to create an endless runner, that generates a map both randomly and on the fly. In this repository you can find my current development progress. Eventhough most of the basic functions are working I want to point out that it is definitely not finished. If you want to help me  finish it, or if you have any ideas on how I could improve it feel free to reach out to me. 
## Whats working so far?
-Custom Editor: The custom editor allows you to add as many different objects to one type of elements as you wish. That way you can add various straight elements and the script will then randomly pick one of these
-Object Pooling: Instead of creating and deleting all elements after they were used the script enables and disables them. It will only create new elements if its really necessary.
-Player Controls: By swiping and tilting you device you can control the player just like in a classical endless runner like Temple Run
-Forks are working: This script is one of the few out there which can handle fork elements(look like a T). If the player walked in one direction at the fork, the generation process will be only continued in that direction
-Collision detection: The PlayerScript always provides the current position of the player
-Supports stairs: This script can also deal with elements like stairs or ramps that might change the level on which the player is walking 
##Whats missing?
-A real intelligent collision detection for the building process: Right now the building process is randomly and it doesn't check if the newly placed part intersects with a previous one. 
## License
* see [LICENSE](https://github.com/lxndrblz/Unity-Endless-Runner/blob/master/LICENSE.md) file

## Version
* Version 0.1

## Contact
#### Developer/Company
* Homepage: www.alexbilz.com
* e-mail: mail@alexbilz.com
