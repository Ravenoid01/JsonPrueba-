using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestAPIManager : MonoBehaviour
{
    [SerializeField]
    private RawImage YourRawImage;

    [SerializeField]
    private RawImage YourRawImage1;

    [SerializeField]
    private RawImage YourRawImage2;

    [SerializeField]
    private RawImage YourRawImage3;

    [SerializeField]
    private RawImage YourRawImage4;

    [SerializeField]
    private int userId = 1;

    [SerializeField]
    private Text textUser;

    private string[] characterID = new string[5];

    private string serverApiPath = "https://my-json-server.typicode.com/Ravenoid01/JsonPrueba-";
    private string rickYMortyApiPath = "https://rickandmortyapi.com/api/character";

    void Start()
    {
        StartCoroutine(GetUserInfo());
    }
    public void GetCharactersClick()
    {
        StartCoroutine(GetCharacters());
    }
    public void NextButtom()
    {
        if(userId >= 3)
        {
            userId = 1;
        }
        else
        {
            userId++;
        }
        StartCoroutine(GetUserInfo());
    }
    IEnumerator GetCharacters()
    {
        for(int i = 0;i < characterID.Length; i++)
        {
            UnityWebRequest www = UnityWebRequest.Get(rickYMortyApiPath + "/" + characterID[i]);
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log("NETWORK ERROR :" + www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text)
                if (www.responseCode == 200)
                {
                    Character character = JsonUtility.FromJson<Character>(www.downloadHandler.text);
                    Debug.Log(character.name);

                    StartCoroutine(DownloadImage(character.image, i));

                }
                else
                {
                    string mensaje = "Status: " + www.responseCode;
                    Debug.Log(mensaje);
                }
            }
        }
        
        
    }
    IEnumerator GetUserInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(serverApiPath + "/users/" + userId);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                UserData user = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
                Debug.Log(user.name);
                textUser.text = user.name;
                for(int i = 0;i < user.deck.Length; i++)
                {
                    characterID[i] = user.deck[i].ToString();
                    Debug.Log(characterID[i]);
                }
            }
            else
            {
                string mensaje = "Status: " + www.responseCode;
                Debug.Log(mensaje);
            }
        }
    }
    IEnumerator DownloadImage(string MediaUrl, int textIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else if(textIndex == 0) YourRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        else if (textIndex == 1) YourRawImage1.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        else if (textIndex == 2) YourRawImage2.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        else if (textIndex == 3) YourRawImage3.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        else YourRawImage4.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

    }
}
[System.Serializable]

public class UserData
{
    public int id;
    public string name;
    public int[] deck;
}
public class CharacterList
{
    public CharacterListInfo info;
    public List<Character> results;
}
[System.Serializable]
public class CharacterListInfo
{
    public int count;
    public int pages;
    public string next;
    public string prev;
}
[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}
