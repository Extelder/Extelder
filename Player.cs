using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int line;
    public int _score;
    private float _timeBtwScoreAdd;

    [SerializeField] private float _switchLineSpeed;
    [SerializeField] private float _speed;

    [SerializeField] private Text _crystalText;
    [SerializeField] private Text _starText;
    [SerializeField] private Text _fuletext;
    [SerializeField] private Text _scoreText;

    [SerializeField] private _camera _camera;

    [SerializeField] private GameObject _skin;
    [SerializeField] private GameObject _rocket;
    [SerializeField] private GameObject _shieldGFX;
    [SerializeField] private GameObject _deathPanel;

    public float stars;
    private int _crystals;

    public static Player _singleton { get; private set; }

    private bool _isInRocket;
    public bool _canDestroy;

    public UnityEvent OnStarCollected;
    public UnityEvent OnCrystalCollected;

    private void Awake()
    {
        _crystals = PlayerPrefs.GetInt("Crystals");
        stars = PlayerPrefs.GetFloat("Stars");

        _timeBtwScoreAdd = 0.5f;
        _score = 1; 
        line = 2;
      
        _starText.text = stars.ToString();
        _crystalText.text = _crystals.ToString();
        StartCoroutine(AddSpeed());
        StartCoroutine(AddScore());

        _canDestroy = true;
        _singleton = this;
    }
    
    private void FixedUpdate()
    {
        
        transform.Translate(_speed * Time.deltaTime, 0f, 0f);
        switch (line)
        {
            case 1:
             {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 5f, transform.position.z), _switchLineSpeed);
               break;
             }
            case 2:
             {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 10f, transform.position.z), _switchLineSpeed);
                    break;
             }
            case 3:
             {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 15f, transform.position.z), _switchLineSpeed);
                    break;
             }
        }
            
    }
   public void switchLineDown()
    {
        if (line != 1)
        {
            line -= 1;
        }
    }

   public void SwitchLineUp()
    {
        if(line != 3)
        {
            line += 1;
        }
    }

    public void OnRocketEnable()
    {
        OnRocket(2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Star>(out Star star))
        {
            OnStarCollected.Invoke();
            stars += 1f;
            PlayerPrefs.SetFloat("Stars", stars);
            _starText.text = stars.ToString();
            Destroy(other.gameObject);
        }
        if (other.TryGetComponent<_meteor>(out _meteor _meteor))
        {
            if (_isInRocket == false && _canDestroy == true)
            {
                _deathPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            if(_canDestroy == false || _isInRocket == true)
            {
                _shieldGFX.SetActive(false);
                _canDestroy = true;
            }
        }
        if(other.TryGetComponent<Shield>(out Shield _shield))
        {
            _shieldGFX.SetActive(true);
            Destroy(_shield.gameObject);
            _canDestroy = false;
            StartCoroutine(ResetBool(_canDestroy, 11f));
        }
        if(other.TryGetComponent<Crystal>(out Crystal _crystal))
        {
            OnCrystalCollected.Invoke();
            _crystals++;
            _crystalText.text = _crystals.ToString();
            PlayerPrefs.SetInt("Crystals", _crystals);
            Destroy(_crystal.gameObject);

        }
    }
    private IEnumerator ResetBool( bool _bool, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _shieldGFX.SetActive(false);
        _bool = false;
    }
    private void DisableRocket()
    {
        OnRocket(1);
    }

    private IEnumerator AddSpeed()
    {
        while (true)
        {
            _timeBtwScoreAdd -= 0.05f;
            _speed -= 0.3f;
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator AddScore()
    {
        while (true)
        { 
          _score++;
          _scoreText.text = _score.ToString();
          yield return new WaitForSeconds(_timeBtwScoreAdd);
        }
    }
   private void OnRocket(int variant)
    {
        if(variant == 1)
        {
        _skin.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        _rocket.SetActive(false);
        _isInRocket = false;
        _speed += 10f;
        _camera._followSpeed -= 10f;
        }
        if(variant == 2)
        {
        _rocket.SetActive(true);
        _isInRocket = true;
        _speed -= 10f;
        _camera._followSpeed += 10f;
        Invoke(nameof(DisableRocket), 10f);
        _skin.transform.rotation = Quaternion.Euler( 0f, 180f, 90f);
         
        }
        
    }
}
