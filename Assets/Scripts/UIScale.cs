using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScale : MonoBehaviour
{
    public RectTransform pic;
    private float timer = 0;
    bool isstart = false;

    public RawImage image;
    private void Start()
    {
        image.enabled = false;
        
    }

    void Update()
    {
        if (isstart)
        {
            image.enabled = true;
            timer += Time.deltaTime * 0.8f;
            pic.localScale = Vector3.Lerp(new Vector3(1, 1, 0), new Vector3(0.5f, 0.5f, 0), timer);
            pic.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(-450, 250f, 0), timer);
            pic.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -25), timer));
        }




        if (Input.GetKeyDown(KeyCode.A))//检测按下回车截图
        {
            isstart = true;
            Camera camera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
            int ratio = 2;
            Rect rect = new Rect(0, 0, (int)Screen.width / ratio, (int)Screen.height / ratio);//图片大小取决于ratio的大小
            string name = Application.dataPath + "/Resources/ScreenShot/screenshot.png";
            ScreenTool.Instance.ScreenShotFile(name);//(camera, rect,"jietu");
            //image.texture = ScreenShot(camera, rect);
        }
    }
    public Texture2D ScreenShot(Camera camera, Rect rect)
    {
        Debug.Log(rect.width);
        Debug.Log(rect.height);

        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        //颜色纹理格式，每个通道8位。
        screenShot.ReadPixels(rect, 0, 0);//从屏幕读取像素到保存的纹理数据中。
        screenShot.Apply();//实际上应用所有以前的SetPixel和SetPixels更改。
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();//设置文件类型
        string filename = Application.dataPath + "/Resources/ScreenShot/screenshot.png";//存放路径
        System.IO.File.WriteAllBytes(filename, bytes);//根据上边的类型以及路径写入文件夹中去
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新，这步很关键，否则后面调用图片时没有。
#endif

        return screenShot;
    }
}