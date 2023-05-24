using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCreator : MonoBehaviour
{
    public List<LevelCreator> levelCreators;
    private LevelCreator _currentLevelCreator;
    [SerializeField]
    private GameObject[] backgrounds;
    private int _currentBackground;

    private void Start()
    {
        foreach (var background in backgrounds)
        {
            background.SetActive(false);
        }
        GlobalEvents.OnNextLevelRequest.AddListener(LoadNextLevel);
        LoadNextLevel();

    }
    

    private void LoadNextLevel()
    {
        backgrounds[_currentBackground].SetActive(true);
        if (_currentBackground > 0)
            backgrounds[_currentBackground - 1].SetActive(false);
        _currentBackground += 1;
        if (_currentLevelCreator is not null)
            _currentLevelCreator.gameObject.SetActive(false);
        if (levelCreators.Count > 0)
        {
            _currentLevelCreator = levelCreators.First();
            _currentLevelCreator.enabled = true;
            levelCreators.RemoveAt(0);
        }
    }
}
