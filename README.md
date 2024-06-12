# P5JS In Unity

[The Coding Train](https://www.youtube.com/@TheCodingTrain) has plenty of tutorial and guides on various algorithms.

I created extension methods in unity to allow p5js code and syntax to work in unity matching as close as possible.

Note that the 2D/3D rendering for unity is different from P5JS, so there might be some visual bugs.



# Differences

- Replace *let* with public static varirables e.g "let xSpeed" => "public static xSpeed"
- Replace *Arrays[]* with List e.g "tail = []" => "List<Vector2> tail = new()";
