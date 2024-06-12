using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class GameViewResolutionSetter
{
    public static int TargetWidth = 1920;
    public static int TargetHeight = 1080;
    public static void SetGameView(int width, int height)
    {
        TargetWidth = width;
        TargetHeight = height;
    }

    static GameViewResolutionSetter()
    {
        EditorApplication.playModeStateChanged += SetGameViewResolution;
    }

    private static void SetGameViewResolution(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SetGameViewSize(TargetWidth, TargetHeight);
        }
    }

    private static void SetGameViewSize(int width, int height)
    {
        var gameViewSizesInstance = GetGameViewSizesInstance();
        var customSize = FindGameViewSizeIndex(width, height);

        if (customSize == -1)
        {
            AddCustomGameViewSize(width, height);
            customSize = FindGameViewSizeIndex(width, height);
        }

        SetGameViewSizeIndex(customSize);
    }

    private static object GetGameViewSizesInstance()
    {
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        return instanceProp.GetValue(null, null);
    }

    private static int FindGameViewSizeIndex(int width, int height)
    {
        var group = GetCurrentGameViewSizeGroupType();
        var gameViewSizesInstance = GetGameViewSizesInstance();
        var getGroup = gameViewSizesInstance.GetType().GetMethod("GetGroup");
        var groupInstance = getGroup.Invoke(gameViewSizesInstance, new object[] { (int)group });

        var getBuiltinCount = groupInstance.GetType().GetMethod("GetBuiltinCount");
        var getCustomCount = groupInstance.GetType().GetMethod("GetCustomCount");
        var getGameViewSize = groupInstance.GetType().GetMethod("GetGameViewSize");

        int totalSizes = (int)getBuiltinCount.Invoke(groupInstance, null) + (int)getCustomCount.Invoke(groupInstance, null);

        for (int i = 0; i < totalSizes; i++)
        {
            var size = getGameViewSize.Invoke(groupInstance, new object[] { i });
            var widthProp = size.GetType().GetProperty("width");
            var heightProp = size.GetType().GetProperty("height");

            if ((int)widthProp.GetValue(size, null) == width && (int)heightProp.GetValue(size, null) == height)
            {
                return i;
            }
        }
        return -1;
    }

    private static void AddCustomGameViewSize(int width, int height)
    {
        var gameViewSizesInstance = GetGameViewSizesInstance();
        var group = GetCurrentGameViewSizeGroupType();
        var getGroup = gameViewSizesInstance.GetType().GetMethod("GetGroup");
        var groupInstance = getGroup.Invoke(gameViewSizesInstance, new object[] { (int)group });

        var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var gameViewSizeTypeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
        var ctor = gameViewSizeType.GetConstructor(new System.Type[] { gameViewSizeTypeType, typeof(int), typeof(int), typeof(string) });

        var newSize = ctor.Invoke(new object[] { (int)gameViewSizeTypeType.GetEnumValues().GetValue(1), width, height, width + "x" + height });
        var addCustomSize = groupInstance.GetType().GetMethod("AddCustomSize");
        addCustomSize.Invoke(groupInstance, new object[] { newSize });
    }

    private static void SetGameViewSizeIndex(int index)
    {
        var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gameViewType.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gameViewWindow = EditorWindow.GetWindow(gameViewType);
        selectedSizeIndexProp.SetValue(gameViewWindow, index, null);
    }

    private static object GetCurrentGameViewSizeGroupType()
    {
        var gameViewSizesInstance = GetGameViewSizesInstance();
        var currentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
        return currentGroupTypeProp.GetValue(gameViewSizesInstance, null);
    }
}