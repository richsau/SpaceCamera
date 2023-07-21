using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _playerCameras;
    [SerializeField]
    private PlayableDirector _inactiveDirector;
    [SerializeField]
    private PlayableDirector _finalDirector;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _creditText;
    [SerializeField]
    private GameObject _instructionText;
    [SerializeField]
    private GameObject _finalText;
    private int _currentCamera;
    private bool _playerActive = true;
    private Vector3 _lastMousePosition;
    [SerializeField] 
    private ParticleSystem _warpEffect;
    private bool _inFinalScene = false;

    void Start()
    {
        _lastMousePosition = Input.mousePosition;
        StartCoroutine(ShowCredits());
        StartCoroutine(PlayerInactive());
    }

    void Update()
    {
        if ((Input.mousePosition != _lastMousePosition) || Input.anyKeyDown)
        {
            _playerActive = true;
            _lastMousePosition = Input.mousePosition;
        }
       
        if (_playerActive)
        {
            if (_inactiveDirector.state == PlayState.Playing)
            {
                _inactiveDirector.Stop();
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

        if (_inFinalScene)
        {
           _warpEffect.Play();
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void SetFinalScene()
    {
        StartCoroutine(FinalScene());
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
                if (_inactiveDirector.state != PlayState.Playing && _finalDirector.state != PlayState.Playing && !_inFinalScene)
                {
                    _canvas.gameObject.SetActive(false);
                    _inactiveDirector.Play();
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

    IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(0.5f);
        _canvas.gameObject.SetActive(true);
        _inactiveDirector.Stop();
        _currentCamera = 1;
        ResetCamPriorities();
        SetCurrentCamera();
        _inFinalScene = true;
        _warpEffect.gameObject.SetActive(true);
        _warpEffect.Play();
        _instructionText.gameObject.SetActive(false);
        _finalText.gameObject.SetActive(true);
    }
}
