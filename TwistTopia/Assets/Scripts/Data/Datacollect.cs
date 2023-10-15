using UnityEngine;
using System.Collections;
using Proyecto26;
using UnityEngine.SceneManagement;

public class Datacollect : MonoBehaviour
{
    private const string DATABASE_URL = "https://project-fc340-default-rtdb.firebaseio.com/.json";
    private string scene = "";

    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        scene = currentSceneName;

        DirectionManager.OnRotateKeyPressed += HandleRotateKeyPress;
        FrameAction.OnEnemyNotCatched += HandleEnemyNotCatch;
        //StartCoroutine(SendTestData());
    }
    void HandleRotateKeyPress(Vector3 playerPosition)
    {
        
        float startY = playerPosition.y;
        float endY = -12f;
        float stepY = 0.5f;

        for (float y = startY; y >= endY; y -= stepY)
        {
            for (float x = playerPosition.x - 1; x <= playerPosition.x + 1; x += 0.5f)
            {
                for (float z = playerPosition.z - 1; z <= playerPosition.z + 1; z += 0.5f)
                {
                    Vector3 rayOrigin = new Vector3(x, y, z); 
                    Vector3 rayDirection = Vector3.down; 

                    RaycastHit hit;

             
                    if (Physics.Raycast(rayOrigin, rayDirection, out hit))
                    {
                        /*Debug.Log("����" + hit.collider.gameObject.tag);*/
                        
                        if (hit.collider.gameObject.tag.Contains("Platform"))
                        {
                            /*Debug.Log("������ (" + x + ", " + y + ", " + z + ") ������" + hit.collider.gameObject.tag);*/
                            
                            string jsonData = "{\"Scene\": \"" + scene + "\", \"Platform\":\"" + hit.collider.gameObject.tag + "\"}";

                            RestClient.Post(DATABASE_URL, jsonData).Then(response =>
                            {
                                Debug.Log("Data sent successfully!");
                            }).Catch(error =>
                            {
                                Debug.LogError("Error sending data: " + error.Message);
                            });
                            return; 
                        }
                    }
                }
            }
        }

  
    }
    void HandleEnemyNotCatch(Vector3 player, Vector3 frame, Vector3 enemy)
    {
        /*string jsonData = "{ \"playerx\":" + player.x + ", \"playery\":" + player.y + ", \"playerz\":" + player.z + ",
                             \"playerx\":" + player.x + ", \"playery\":" + player.y + ", \"playerz\":" + player.z"}";*/
        string jsonData = $@"{{
            ""playerx"": {player.x},
            ""playery"": {player.y},
            ""playerz"": {player.z},
            ""framex"": {frame.x},
            ""framey"": {frame.y},
            ""framez"": {frame.z},
            ""enemyx"": {enemy.x},
            ""enemyy"": {enemy.y},
            ""enemyz"": {enemy.z}
        }}";

        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });
    }

    IEnumerator SendTestData()
    {
        //����
        //�û������ ƽ̨
        //��ÿ��ƽ̨��ͣ��ʱ��
        //ŵŵ
        //ת���ӽǴ���������
        //ÿһ����Ϸʱ��

        //��enemyλ�ò����
        //���س��������ܹ��� ���õģ�

        //����
        //����enemy�Ĵ���
        //������Ϸ����
        Debug.Log("begin!");

        string jsonData = "{\"playerName\":\"Player1\", \"score\":100}";


        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });

        Debug.Log("over!");
        yield break;


    }
}

