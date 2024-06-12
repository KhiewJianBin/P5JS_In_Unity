# P5JS In Unity

[The Coding Train](https://www.youtube.com/@TheCodingTrain) has plenty of tutorial and guides on various algorithms.

I created extension methods in unity to allow p5js code and syntax to work in unity matching as close as possible.

Note that the 2D/3D rendering for unity is different from P5JS, so there might be some visual bugs.

# Basic Usage Setup
1. Create a new Scene
2. Create a new script and a class that inherits P5JSMonobehaviour
3. Using main sketch.js class variables, create the varaibles in the new class, and initialize them in the class Constructor
4. Copy over the functions as public functions

# Differences
- P5JS class variables is able to be created and assigned without a constructor, However in C# these class variables will need to be Intiailize in the Constructor
- Replace *let* with public static varirables e.g "let xSpeed" => "public static xSpeed"
- Replace *Arrays[]* with List e.g "tail = []" => "List<Vector2> tail = new()";
- Replace *console.log()* with *Debug.Log()*
