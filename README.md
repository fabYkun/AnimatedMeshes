# AnimatedMeshes
Procedural Mesh Geometry for unity (VFX)

This is a naive approach to how 3D effects are made in some games that I wrote last year (2016), I wouldn't code it this way nowadays but anyway... The observation was that some effects cannot be reproduced with particles only and that sometimes meshes are coupled with particles to give the effect a volumetric sense. Using scriptable objects my goal was to simplify the creation of procedural mesh geometry and to implement it on an animation system that would orchestrate all the effets to build up more complexe effects that would be reusable.

This package gives you the possibility of:
- Instantiating animated meshes that will be animated by bending/rotating/translating
- Putting material modifiers on animated meshes to modify their colors/[x/y] tiling/[x/y] offsets
- Create animated patterns (like the ones that are used on danmaku-type games)

For the moment the only mesh that can be instantiated are cones because they filled every needs I had (plus you can make cylinders and circles out of them), but more could easily be added like spheres or cubes... 

## Step 1 : the art
Of course to make effects you need art and maybe some shaders. 
Unity provides us with some neat shaders like those that are listed on Particles>(additive/alphablended/etc...). Use what correspond the most to your effect. 
Once you have an idea of what to use you can use GameObject > Create Other > Cone to create a static mesh and test your materials on it. I used a script originaly made by Wolfram Kresse (http://wiki.unity3d.com/index.php/CreateCone), I just had to tweak it a little bit because the uv mapping wasn't finished and I cut off some of the code that I didn't need. 

## Step 2 : Making the animation
On the project window, right clic > Create > AnimationFX > AnimatedMeshes > Cone to create an animated cone. It gives you plenty of parameters. 

![animatedconeinspector](https://user-images.githubusercontent.com/2204781/28326150-3f861090-6be0-11e7-80b2-fa76ba42561f.png)

The booleans "Create Assets" and "Auto Destroy" decides if the animation should be cached somewhere on the project (if the animation is used a lot it could be useful) and if the animated mesh should destroy itself when the animation is over. 
Another worth-explaining parameter is the "interpolationCurve", it gives more or less weight to the Initial OR Final State depending on where the animation is time-wise. (It could be read as "if InterpolationCurve(time) = 0 the state is equal to initial state and if IC(time) = 1 then the state will be equal to the final state).

![animatedconeMaterialModifierInspector](https://user-images.githubusercontent.com/2204781/28326405-fb5e7924-6be0-11e7-8f4c-3cf89838986e.png)

Then you need to link a MaterialModifier to the animatedCone. It basically defines the behaviour of the material that will be attached to the animation. You can tweak offsets/colors and tilings with a MaterialModifier. 


## Step 3 : Put many animations together and make it a prefab
This is the final step before having fun with patterns (I will edit the post later to explain what animated patterns are) : creating a prefab full of procedural meshes.

# Example

This project contains a sample of Charizard's Down Smash in Smash Bros.

Original Animation

![charizardoriginal](https://user-images.githubusercontent.com/2204781/28324466-bfbc5328-6bdb-11e7-880d-2f9b9bde91e9.gif)

Reproduced in Unity with multiple animated cones

![charizardunity](https://user-images.githubusercontent.com/2204781/28324456-bab3ce4c-6bdb-11e7-87f4-f3cbd4aec7f2.gif)
