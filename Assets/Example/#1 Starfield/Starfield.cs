using static P5JSExtension;

public class Starfield : P5JSBehaviour
{
    public static Star[] stars = new Star[100];
    public static float speed;

    protected override void setup()
    {
        createCanvas(640, 360);
        for (int i = 0; i < 100; i++)
        {
            stars[i] = new Star();
        }
    }

    protected override void draw()
    {
        speed = map(mouseX, 0, width, 0, 50);
        background(0);
        translate(width / 2, height / 2);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].Update();
            stars[i].Show();
        }
    }
}

public class Star
{
    float x;
    float y;
    float z;
    float pz;

    public Star()
    {
        this.x = random(-width, width);
        this.y = random(-height, height);
        this.z = random(width);
        this.pz = this.z;
    }

    public void Update()
    {
        this.z = this.z - Starfield.speed;
        if (this.z < 1)
        {
            this.z = width;
            this.x = random(-width, width);
            this.y = random(-height, height);
            this.pz = z;
        }
    }

    public void Show()
    {
        fill(255);
        noStroke();

        var sx = map(this.x / this.z, 0, 1, 0, width);
        var sy = map(this.y / this.z, 0, 1, 0, height);

        var r = map(this.z, 0, width, 4, 0);
        //ellipse(sx, sy , r, r);

        var px = map(this.x / this.pz, 0, 1, 0, width);
        var py = map(this.y / this.pz, 0, 1, 0, height);

        this.pz = this.z;

        stroke(255);
        strokeWeight(r);
        line(px, py, sx, sy);
    }
}