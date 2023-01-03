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




        if (Input.GetKeyDown(KeyCode.A))//��ⰴ�»س���ͼ
        {
            isstart = true;
            Camera camera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
            int ratio = 2;
            Rect rect = new Rect(0, 0, (int)Screen.width / ratio, (int)Screen.height / ratio);//ͼƬ��Сȡ����ratio�Ĵ�С
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
        //��ɫ�����ʽ��ÿ��ͨ��8λ��
        screenShot.ReadPixels(rect, 0, 0);//����Ļ��ȡ���ص���������������С�
        screenShot.Apply();//ʵ����Ӧ��������ǰ��SetPixel��SetPixels���ġ�
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();//�����ļ�����
        string filename = Application.dataPath + "/Resources/ScreenShot/screenshot.png";//���·��
        System.IO.File.WriteAllBytes(filename, bytes);//�����ϱߵ������Լ�·��д���ļ�����ȥ
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ�£��ⲽ�ܹؼ�������������ͼƬʱû�С�
#endif

        return screenShot;
    }
}