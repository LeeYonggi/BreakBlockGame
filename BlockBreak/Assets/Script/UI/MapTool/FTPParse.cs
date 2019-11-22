using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public static class FTPParse
{
    private static string ftpPath = "ftp://192.168.0.56/SaveMap_BRICKS_BREAKER/TESTMAP/";
    private static string user = "ftpsnc";
    private static string pwd = "ftpsnc001";

    /// <summary>
    /// FTP 서버에 업로드를 한다.
    /// </summary>
    /// <param name="fileName"></param>
    /// 파일 이름
    /// <param name="data"></param>
    /// FTP 서버에 올릴 txt 데이터
    public static void FtpUpload(string fileName, byte[] data)
    {
        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath + fileName);

        req.Method = WebRequestMethods.Ftp.UploadFile;

        req.Credentials = new NetworkCredential(user, pwd);

        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
        }

        using (FtpWebResponse response = (FtpWebResponse)req.GetResponse())
        {
            // 완료 되었을 시
            //Debug.Log(response.StatusDescription);
        }
    }

    /// <summary>
    /// FTP서버 폴더에서 전체 파일 리스트를 가져온다.
    /// </summary>
    /// <returns></returns>
    public static List<FileInfo> GetFTPFileList()
    {
        List<FileInfo> fileList = new List<FileInfo>();

        FtpWebRequest ftpWebRequest = WebRequest.Create(ftpPath) as FtpWebRequest;

        ftpWebRequest.Credentials = new NetworkCredential(user, pwd);
        ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;

        StreamReader streamReader = new StreamReader(ftpWebRequest.GetResponse().GetResponseStream());

        while(true)
        {
            string fileName = streamReader.ReadLine();

            if (string.IsNullOrEmpty(fileName))
                break;

            fileList.Add(new FileInfo(fileName));
        }

        return fileList;
    }

    /// <summary>
    /// FTP 폴더에서 파일을 가져온다.
    /// </summary>
    /// <param name="fileName"></param>
    /// 파일 이름
    /// <returns></returns>
    /// 파일의 전체 데이터
    public static string GetFTPTextFile(string fileName)
    { 
        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath + fileName);
      
        req.Method = WebRequestMethods.Ftp.DownloadFile;
        req.Credentials = new NetworkCredential(user, pwd);

        string data = string.Empty;

        using(FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
        {
            Stream stream = resp.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }
        }

        return data;
    }
}
