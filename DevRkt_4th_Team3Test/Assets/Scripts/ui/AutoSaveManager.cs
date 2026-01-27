using UnityEngine;

using System.Collections;    

public class AutoSaveManager : MonoBehaviour

{

    public static AutoSaveManager Instance;

    public float saveInterval = 60f;

    private SaveData _saveData;

    private void Start()

    {

        StartCoroutine(AutoSaveRoutine());

    }

        

    IEnumerator AutoSaveRoutine()

    {

        while (true)

        {

            yield return new WaitForSeconds(saveInterval);



            if (_saveData != null)

            {

                _saveData.SavePlayerData();

            }

        }

    }

}