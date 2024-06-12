using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//extension methods to make it look like using PSJS function in unity

//new vector 2 class is made, because unity Vector2 is a struct, and struct only gets pass by value and not by refrence
public class PVector
{
    public float x;
    public float y;
    public PVector(float inx, float iny)
    {
        x = inx;
        y = iny;
    }
    public static PVector operator -(PVector v1, PVector v2)
    {
        return new PVector(v1.x - v2.x, v1.y - v2.y);
    }
    public static PVector operator +(PVector v1, PVector v2)
    {
        return new PVector(v1.x + v2.x, v1.y + v2.y);

    }
    public static PVector operator *(PVector v1, float v)
    {
        return new PVector(v1.x * v, v1.y * v);
    }
    public static PVector operator /(PVector v1, float v)
    {
        return new PVector(v1.x / v, v1.y / v);
    }
    public PVector copy()
    {
        return new PVector(x, y);
    }
}

public static class P5JSExtension
{
    public static int width => Screen.width;
    public static int height => Screen.height;
    public static float mouseX => Mathf.Clamp(0, Input.mousePosition.x, Screen.width);
    public static float mouseY => Mathf.Clamp(0, Input.mousePosition.y, Screen.height);

    //Math functions extended
    public static float radians(float deg) => deg * Mathf.Deg2Rad;
    public static float map
        (float value,
        float start1, float stop1,
        float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
    public static int floor(float value)
    {
        return Mathf.FloorToInt(value);
    }
    public static float constrain(float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
    public static float dist(float x1, float y1, float x2, float y2)
    {
        return Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));
    }
    public static float dist(Vector2 v1, Vector2 v2)
    {
        return Vector2.Distance(v1, v2);
    }


    //random noise
    public static int random(int min, int max)
    {
        return Random.Range(min, max);
    }
    public static float random(float min, float max)
    {
        return Random.Range(min, max);
    }
    public static int random(int value)
    {
        if (value > 0)
            return random(0, value);
        else
            return random(value, 0);
    }
    public static T random<T>(List<T> list)
    {
        int value = random(list.Count);
        return list[value];
    }
    public static float random(float value)
    {
        if (value > 0)
            return random(0, value);
        else
            return random(value, 0);
    }
    public static Vector2 random2D()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    public static Vector3 random3D()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    public static float noise(float x)
    {
        return noise(x,0);
    }
    public static float noise(float x, float y)
    {
        return Mathf.PerlinNoise(DateTime.UtcNow.DayOfYear + x, DateTime.UtcNow.DayOfYear + y);
    }




    //Scene, canvas
    public static void createCanvas(int width, int height)
    {
        GameViewResolutionSetter.SetGameView(width, height);
    }
    public static void background(byte rgb)
    {
        background(rgb, rgb, rgb);
    }
    public static void background(byte r, byte g, byte b)
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = new Color32(r, g, b, 255);
    }
    public static void dontclear()
    {
        Camera.main.clearFlags = CameraClearFlags.Nothing;
    }
    public static void frameRate(int rate)
    {
        Application.targetFrameRate = rate;
    }





    //Colors
    static float strokethickness = 1;
    static Color32 strokecolor = new Color32(255, 255, 255, 255);
    public static void strokeWeight(float value)
    {
        value = value * 2;// *2 because p5js render at a larger size
        strokethickness = Mathf.Max(1,value);
    }
    public static void noStroke()
    {
        strokethickness = 0;
    }
    public static void stroke(byte r, byte g, byte b,byte a)
    {
        if (UsingHSB)
        {
            strokecolor = Color.HSVToRGB(r / 255f, g / 255f, b / 255f);
        }
        else
        {
            strokecolor = new Color32(r, g, b, a);
        }
        if(strokethickness == 0)
        {
            strokethickness = 1;
        }
    }
    public static void stroke(byte r, byte g, byte b)
    {
        stroke(r,g,b,255);
    }
    public static void stroke(byte rgb,byte a)
    {
        stroke(rgb, rgb, rgb,a);
    }
    public static void stroke(byte rgb)
    {
        stroke(rgb, rgb, rgb);
    }
    public static void noFill()
    {
        textureColor.a = 0;
    }
    public static void fill(float r, float g, float b, float a)
    {
        if (UsingHSB)
        {
            textureColor = Color.HSVToRGB(r / 255f, g / 255f, b / 255f);
        }
        else
        {
            textureColor = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
        }
    }
    public static void fill(float r, float g, float b)
    {
        fill(r, g, b, 255);
    }
    public static void fill(float rgb,float a)
    {
        fill(rgb, rgb, rgb, a);
    }
    public static void fill(float rgb)
    {
        fill(rgb, rgb, rgb);
    }

    //color mode
    public static bool HSB = true;
    static bool UsingHSB = false;
    public static void colorMode(bool ishsb)
    {
        UsingHSB = ishsb;
    }


    //2d draw
    static Texture2D texture = new Texture2D(4, 4);
    static Color32 textureColor = new Color32(255, 255, 255, 255);
    public static void ellipse(float px, float py, float sx, float sy)
    {
        px *= originscale.x;
        py *= originscale.y;

        px += originpos.x;
        py += originpos.y;

        Vector2 pos = new Vector2(px, py);
        pos = rotateWithRespectToOrigin(pos);

        Rect position = new Rect(pos.x - sx / 2, pos.y - sy / 2, sx, sy);
        GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, false, 0, textureColor, 0, sx);
        if(strokethickness > 0)
        {
            GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, false, 0, strokecolor, strokethickness, sx);
        }
    }
    public static void arc(float px, float py, float sx, float sy,float startangle,float endangle)
    {
        //todo
        px *= originscale.x;
        py *= originscale.y;

        px += originpos.x;
        py += originpos.y;

        Vector2 pos = new Vector2(px, py);
        pos = rotateWithRespectToOrigin(pos);

        Rect position = new Rect(pos.x - sx / 2, pos.y - sy / 2, sx, sy);
        GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, false, 0, textureColor, 0, sx);
        if (strokethickness > 0)
        {
            GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, false, 0, strokecolor, strokethickness, sx);
        }
    }
    public static void circle(float px,float py,float s)
    {
        ellipse(px, py, s);
    }
    public static void ellipse(float px, float py, float s)
    {
        ellipse(px, py, s, s);
    }
    public static void point(float px, float py)
    {
        ellipse(px, py, strokethickness, strokethickness);
    }
    public static bool CENTER = true;
    static bool rectdrawmodecenter = false;
    public static void rectMode(bool value)
    {
        rectdrawmodecenter = value;
    }
    public static void rect(float px, float py, float sx, float sy)
    {
        px *= originscale.x;
        py *= originscale.y;

        px += originpos.x;
        py += originpos.y;

        Vector2 pos = new Vector2(px, py);
        pos = rotateWithRespectToOrigin(pos);

        Rect position;
        if (rectdrawmodecenter)
        {
            position = new Rect(pos.x - sx/2, pos.y-sy/2, sx, sy);
        }
        else
        {
            position = new Rect(pos.x, pos.y, sx, sy);
        }
        GUI.DrawTexture(position, texture, ScaleMode.StretchToFill, false, 0, textureColor, 0, 0);
        if (strokethickness > 0)
        {
            GUI.DrawTexture(position, texture, ScaleMode.StretchToFill, false, 0, strokecolor, strokethickness, 0);
        }
    }
    public static void line(float p1x, float p1y, float p2x, float p2y)
    {
        p1x *= originscale.x;
        p1y *= originscale.x;
        p2x *= originscale.y;
        p2y *= originscale.y;

        p1x += originpos.x;
        p1y += originpos.y;
        p2x += originpos.x;
        p2y += originpos.y;

        Vector2 pos1 = new Vector2(p1x, p1y);
        Vector2 pos2 = new Vector2(p2x, p2y);
        pos1 = rotateWithRespectToOrigin(pos1);
        pos2 = rotateWithRespectToOrigin(pos2);

        Drawing.DrawLine(pos1, pos2, strokecolor, strokethickness * originscale.x);
    }
    public static void text(string s,float px,float py)
    {
        GUI.Label(new Rect(px, py, 500, 100), s);
    }
    public static void text(float f, float px, float py)
    {
        GUI.Label(new Rect(px, py, 500, 100), f.ToString());
    }


    //3d draw meshes
    static List<GameObject> gameobjects = new List<GameObject>();
    static int submeshcount = 0;
    static int lastsubmeshverticescount = 0;


    static bool hasbeginShape = false;
    static Mesh mesh = new Mesh();
    static MeshTopology meshtopology;
    static List<Vector3> vertices = new List<Vector3>();
    static List<int> indices = new List<int>();

    public static void resetShape()
    {
        submeshcount = 0;
        vertices.Clear();
        mesh.Clear();
    }
    public static void beginShape(MeshTopology meshTopology = MeshTopology.LineStrip)
    {
        submeshcount++;
        mesh.subMeshCount = submeshcount;
        meshtopology = meshTopology;
        hasbeginShape = true;
    }
    public static void vertex(float x, float y, float z)
    {
        if (hasbeginShape)
        {
            vertices.Add(new Vector3(x, y, z));
            if (meshtopology == MeshTopology.LineStrip ||
                meshtopology == MeshTopology.Points ||
                meshtopology == MeshTopology.Lines)
            {
                indices.Add(vertices.Count - 1);
            }
            else //triangle strip hardcode
            {
                if(vertices.Count>2)
                {
                    if(vertices.Count%2 == 0)
                    {
                        indices.Add(vertices.Count - 1);
                        indices.Add(vertices.Count - 1 - 1);
                        indices.Add(vertices.Count - 1 - 2);
                    }
                    else
                    {
                        indices.Add(vertices.Count - 1 - 2);
                        indices.Add(vertices.Count - 1 - 1);
                        indices.Add(vertices.Count - 1);
                    }
                }
            }
        }
    }
    public static void vertex(float x, float y)
    {
        vertex(x, y, 0);
    }
    public static bool CLOSED = true;
    public static Mesh endShape(bool closed = false)
    {
        lastsubmeshverticescount = mesh.vertices.Length;
        if (closed)
        {
            indices.Add(0);
        }
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, meshtopology, submeshcount-1);
        hasbeginShape = false;

        indices.Clear();
        if (meshtopology == MeshTopology.Points || meshtopology == MeshTopology.LineStrip || meshtopology == MeshTopology.Lines)
        {
            return mesh;
        }
        mesh.RecalculateNormals();

        return mesh;
    }




    





    //matrix ,scene translation rotate,origin
    static Vector2 originpos = new Vector2(0, 0);
    static float rotation = 0;
    static Vector2 originscale = new Vector2(1, 1);

    static Stack<Vector2> savedorigin = new Stack<Vector2>();
    static Stack<float> savedrotation = new Stack<float>();
    static Stack<Vector2> savedscale = new Stack<Vector2>();

    public static void translate(float x, float y)
    {
        Vector2 neworigin = new Vector2(originpos.x + x, originpos.y + y);
        Vector2 rotatedvector = rotateWithRespectToOrigin(new Vector2(neworigin.x, neworigin.y));
        originpos.x = rotatedvector.x;
        originpos.y = rotatedvector.y;
    }
    public static void rotate(float radians)
    {
        rotation += radians;
    }
    public static void scale(float inxscale,float inyscale)
    {
        originscale = new Vector2(inxscale,inyscale);
    }
    public static void scale(float inscale)
    {
        scale(inscale, inscale);
    }
    static Vector2 rotateWithRespectToOrigin(Vector2 inv)
    {
        inv -= originpos;
        inv = new Vector2(Mathf.Cos(rotation) * inv.x - Mathf.Sin(rotation) * inv.y, Mathf.Sin(rotation) * inv.x + Mathf.Cos(rotation) * inv.y);
        return inv + originpos;
    }

    public static void push()
    {
        savedorigin.Push(originpos);
        savedrotation.Push(rotation);
        savedscale.Push(originscale);
    }
    public static void pop()
    {
        originpos = savedorigin.Pop();
        rotation = savedrotation.Pop();
        originscale = savedscale.Pop();
    }
    public static void resetMatrix()
    {
        originpos = Vector2.zero;
        rotation = 0;
        originscale = Vector2.one;
    }




    //vector extensions
    public static Vector2 fromAngle(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public static float heading(this Vector2 v)
    {
        if(v.x>0)
        {
            return Mathf.Atan(v.y / v.x);
        }
        else
        {
            return Mathf.Atan(v.y / v.x)+Mathf.PI;
        }
    }
    public static Vector2 rotate(this Vector2 v, float radians)
    {
        return new Vector2(Mathf.Cos(radians) * v.x - Mathf.Sin(radians) * v.y, Mathf.Sin(radians) * v.x + Mathf.Cos(radians) * v.y);
    }
    public static Vector2 limit(this Vector2 v, float value)
    {
        if(v.magnitude > value)
        {
            v.Normalize();
            v *= value;
        }
        return v;
    }
    public static Vector2 setMag(this Vector2 v, float value)
    {
        v.Normalize();
        v *= value;
        return v;
    }
    public static Vector3 setMag(this Vector3 v, float value)
    {
        v.Normalize();
        v *= value;
        return v;
    }

    public static PVector rotate(this PVector v, float radians)
    {
        return new PVector(Mathf.Cos(radians) * v.x - Mathf.Sin(radians) * v.y, Mathf.Sin(radians) * v.x + Mathf.Cos(radians) * v.y);
    }
    public static float dist(PVector a, PVector b)
    {
        return Vector2.Distance(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
    }
}