using UnityEngine;
using System.Collections;

public delegate void CallBack();//����ί�лص������ȹر�UI����ȡ��û��UI�Ļ���
/// <summary>
/// ��ͼ������
/// </summary>
public class ScreenTool
{
    private static ScreenTool _instance;
    public static ScreenTool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ScreenTool();
            return _instance;
        }
    }

    /// <summary>
    /// UnityEngine�Դ�����Api��ֻ�ܽ�ȫ��
    /// </summary>
    /// <param name="fileName">�ļ���</param>
    public void ScreenShotFile(string fileName)
    {
        UnityEngine.ScreenCapture.CaptureScreenshot(fileName);//��ͼ�������ͼ�ļ�
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName)); 

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif
    }
    /// <summary>
    /// UnityEngine�Դ�����Api��ֻ�ܽ�ȫ��
    /// </summary>
    /// <param name="fileName">�ļ���</param>
    /// <param name="callBack">��ͼ��ɻص�</param>
    /// <returns>Э��</returns>
    public IEnumerator ScreenShotTex(string fileName, CallBack callBack = null)
    {
        yield return new WaitForEndOfFrame();//�ȵ�֡��������Ȼ�ᱨ��
        Texture2D tex = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();//��ͼ����Texture2D����
        byte[] bytes = tex.EncodeToPNG();//���������ݣ�ת����һ��pngͼƬ
        System.IO.File.WriteAllBytes(fileName, bytes);//д������
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName));

        callBack?.Invoke();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif
    }
    /// <summary>
    /// ��ȡ��Ϸ��Ļ�ڵ�����
    /// </summary>
    /// <param name="rect">��ȡ������Ļ���½�Ϊ0��</param>
    /// <param name="fileName">�ļ���</param>
    /// <param name="callBack">��ͼ��ɻص�</param>
    /// <returns></returns>
    public IEnumerator ScreenCapture(Rect rect, string fileName, CallBack callBack = null)
    {
        yield return new WaitForEndOfFrame();//�ȵ�֡��������Ȼ�ᱨ��
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//�½�һ��Texture2D����
        tex.ReadPixels(rect, 0, 0);//��ȡ���أ���Ļ���½�Ϊ0��
        tex.Apply();//����������Ϣ

        byte[] bytes = tex.EncodeToPNG();//���������ݣ�ת����һ��pngͼƬ
        System.IO.File.WriteAllBytes(fileName, bytes);//д������
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName));

        callBack?.Invoke();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif
    }
    /// <summary>
    /// ���������������н�ͼ�������Ҫ���������������ӣ��ɽ�ȡ�������ĵ��ӻ���
    /// </summary>
    /// <param name="camera">����ͼ�����</param>
    /// <param name="width">��ȡ��ͼƬ���</param>
    /// <param name="height">��ȡ��ͼƬ�߶�</param>
    /// <param name="fileName">�ļ���</param>
    /// <returns>����Texture2D����</returns>
    public Texture2D CameraCapture(Camera camera, Rect rect, string fileName)
    {
        RenderTexture render = new RenderTexture((int)rect.width, (int)rect.height, -1);//����һ��RenderTexture���� 

        camera.gameObject.SetActive(true);//���ý�ͼ���
        camera.targetTexture = render;//���ý�ͼ�����targetTextureΪrender
        camera.Render();//�ֶ�������ͼ�������Ⱦ

        RenderTexture.active = render;//����RenderTexture
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//�½�һ��Texture2D����
        tex.ReadPixels(rect, 0, 0);//��ȡ����
        tex.Apply();//����������Ϣ

        camera.targetTexture = null;//���ý�ͼ�����targetTexture
        RenderTexture.active = null;//�ر�RenderTexture�ļ���״̬
        Object.Destroy(render);//ɾ��RenderTexture����

        byte[] bytes = tex.EncodeToPNG();//���������ݣ�ת����һ��pngͼƬ
        System.IO.File.WriteAllBytes(fileName, bytes);//д������
        Debug.Log(string.Format("��ȡ��һ��ͼƬ: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//ˢ��Unity���ʲ�Ŀ¼
#endif

        return tex;//����Texture2D���󣬷�����Ϸ��չʾ��ʹ��
    }
}