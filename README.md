# P5JS In Unity

[The Coding Train](https://www.youtube.com/@TheCodingTrain) has plenty of tutorial and guides on various algorithms.

I created extension methods in unity to allow p5js code and syntax to work in unity matching as close as possible.

Note that the 2D/3D rendering for unity is different from P5JS, so there might be some visual bugs.

# Notes
- Uses OnGUI for 2D rendering. However due to unity running OnGUI multiple times per frame, special case need to be handled for update code
  - Update code in onGUI() in P5JS will need to be in Update() instead 
- 2D/3D rendering in unity is different from P5JS, so there might be some visual bugs.
- 

# Basic Usage Setup
1. Create a new Scene
2. Create a new script and a class that inherits P5JSMonobehaviour
3. Using main sketch.js class variables, create the varaibles in the new class, and initialize them in the class Constructor
4. Copy over the functions as public functions

# Special Functions - These functions will need to be protected override
1. setup()
2. draw()
3. mousePressed()
4. keyPressed()
5. Update()

# Differences
- P5JS class variables is able to be created and assigned without a constructor, However in C# these class variables will need to be Intiailize in the Constructor
- Replace *let* with public static varirables e.g "let xSpeed" => "public static xSpeed"
- Replace *Arrays[]* with List e.g "tail = []" => "List<Vector2> tail = new()";
- Replace *console.log()* with *Debug.Log()*
- vector.mult is an extension which returns a value instead of modifying the current value. e.g. "vec.mult()" => "vec = vec.mult()"
