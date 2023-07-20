using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _playerCameras;
    [SerializeField]
    private PlayableDirector _director;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _creditText;
    [SerializeField]
    private GameObject _instructionText;
    private int _currentCamera;
    private bool _playerActive = true;
    private Vector3 _lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        _lastMousePosition = Input.mousePosition;
        StartCoroutine(ShowCredits());
        StartCoroutine(PlayerInactive());
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.mousePosition != _lastMousePosition) || Input.anyKeyDown)
        {
            _playerActive = true;
            _lastMousePosition = Input.mousePosition;
        }
       
        if (_playerActive)
        {
            if (_director.state == PlayState.Playing)
            {
                _director.Stop();
                _canvas.gameObject.SetActive(true);
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        } 
        else if (Input.GetKeyDown(KeyCode.R))
        {
            
            _currentCamera++;
            if(_currentCamera >= _playerCameras.Length)
            {
                _currentCamera = 0;
            }
            ResetCamPriorities();
            SetCurrentCamera();
        }
    }

    public void ResetCamPriorities()
    {
        foreach (var c in _playerCameras)
        {
            if (c.GetComponent<CinemachineVirtualCamera>())
            {
                c.GetComponent<CinemachineVirtualCamera>().Priority = 10;
            }
        }
    }

    public void SetCurrentCamera()
    {
        if (_playerCameras[_currentCamera].GetComponent<CinemachineVirtualCamera>())
        {
            _playerCameras[_currentCamera].GetComponent<CinemachineVirtualCamera>().Priority = 15;
        }
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }



    IEnumerator PlayerInactive()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (_playerActive)
            {
                _playerActive = false;
            }
            else
            {
                if (_director.state != PlayState.Playing)
                {
                    _canvas.gameObject.SetActive(false);
                    _director.Play();
                }
            }
        }
    }

    IEnumerator ShowCredits()
    {
        _creditText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        _creditText.gameObject.SetActive(false);
        _instructionText.gameObject.SetActive(true);
    }
}
