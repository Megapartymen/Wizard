using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCreator : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _runeIllustration;
    [SerializeField] private ParticleSystem _sucessFlash;
    [SerializeField] private ParticleSystem _spellCreationEffect;
    [SerializeField] private GameObject _smallFireSpell;
    [SerializeField] private GameObject _medFireSpell;
    [SerializeField] private GameObject _bigFireSpell;
    [SerializeField] private GameObject _smallAirSpell;
    [SerializeField] private GameObject _medAirSpell;
    [SerializeField] private GameObject _bigAirSpell;
    [SerializeField] private GameObject _smallWaterSpell;
    [SerializeField] private GameObject _medWaterSpell;
    [SerializeField] private GameObject _bigWaterSpell;
    [SerializeField] private GameObject _smallEarthSpell;
    [SerializeField] private GameObject _medEarthSpell;
    [SerializeField] private GameObject _bigEarthSpell;

    [Header("Sequences")]
    [SerializeField] private List<Material> _runeImeges;
    [SerializeField] private List<GameObject> _allTriggers;
    [SerializeField] private List<Vector3> _correctionAdressesForAllTriggers;
    [SerializeField] private List<GameObject> _fireFirstSequence;
    [SerializeField] private List<GameObject> _fireSecondSequence;
    [SerializeField] private List<GameObject> _fireThirdSequence;
    [SerializeField] private List<GameObject> _airFirstSequence;
    [SerializeField] private List<GameObject> _airSecondSequence;
    [SerializeField] private List<GameObject> _airThirdSequence;
    [SerializeField] private List<GameObject> _waterFirstSequence;
    [SerializeField] private List<GameObject> _waterSecondSequence;
    [SerializeField] private List<GameObject> _waterThirdSequence;
    [SerializeField] private List<GameObject> _earthFirstSequence;
    [SerializeField] private List<GameObject> _earthSecondSequence;
    [SerializeField] private List<GameObject> _earthThirdSequence;

    private bool _armed;
    private float _bulletVelocity;
    private GameObject _activeSpell;
    private int _indexOfSpellForCreation;
    private List<GameObject> _triggersForComparsion;
    private GameObject _tempForParentTrigger;
    private bool _readyForReadSpell;
    private float _distanceToSpaceForCreationSpell;

    private void Start()
    {
        _bulletVelocity = 20f;
        _distanceToSpaceForCreationSpell = 0.5f;
        _triggersForComparsion = new List<GameObject>();
        _tempForParentTrigger = null;
        _readyForReadSpell = false;
        _indexOfSpellForCreation = 0;
        _activeSpell = null;
        _armed = false;
        StopCreationSpellEffect();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.right);

        TryStartCreationSpell(ray);
        TryStopCreationSpell();
        
        TryDrawRune(ray);
        TryCastActiveSpell(); 
    }

    private void TryCastActiveSpell()
    {
        if (Input.GetMouseButton(1) && _armed)
        {
            GameObject bullet = _activeSpell;

            bullet.transform.parent = null;
            bullet.GetComponent<Rigidbody>().velocity = _player.transform.forward * _bulletVelocity;
            _armed = false;
        }
    }

    private void TryStartCreationSpell(Ray ray)
    {
        if (Input.GetMouseButtonDown(0) && !_armed)
        {
            PlayCreationSpellEffect();
            _tempForParentTrigger = Instantiate(_allTriggers[0], ray.GetPoint(_distanceToSpaceForCreationSpell) + new Vector3(0, 0.2f, 0), Quaternion.LookRotation(ray.direction));
            _tempForParentTrigger.name = _allTriggers[0].name;

            for (int i = 1; i < _allTriggers.Count; i++)
            {
                GameObject tempForChildTrigger;
                tempForChildTrigger = Instantiate(_allTriggers[i], _tempForParentTrigger.transform);
                tempForChildTrigger.transform.localPosition += _correctionAdressesForAllTriggers[i];
                tempForChildTrigger.name = _allTriggers[i].name;
            }

            _tempForParentTrigger.transform.position = ray.GetPoint(_distanceToSpaceForCreationSpell);
            _readyForReadSpell = true;
            _triggersForComparsion.Add(_allTriggers[0]);
        }
    }

    private void TryStopCreationSpell()
    {
        if (Input.GetMouseButtonUp(0) && !_armed)
        {
            StopCreationSpellEffect();
            CheckComparsionListCount();
            _triggersForComparsion.Clear();
            Destroy(_tempForParentTrigger);
            _readyForReadSpell = false;
        }
    }

    private void TryDrawRune(Ray ray)
    {
        if (_readyForReadSpell)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _distanceToSpaceForCreationSpell) && _triggersForComparsion[_triggersForComparsion.Count - 1] != hit.collider.gameObject)
            {
                hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
                _triggersForComparsion.Add(hit.collider.gameObject);
            }
        }
    }

    private void TryComparsionSequence(int indexOfSpellThisSequence, List<GameObject> sequenceForComparsion)
    {
        if (_indexOfSpellForCreation == 0)
        {
            for (int i = 0; i < sequenceForComparsion.Count; i++)
            {
                if (sequenceForComparsion[i].name != _triggersForComparsion[i].name)
                {
                    _indexOfSpellForCreation = 0;
                    break;
                }
                else
                {
                    _indexOfSpellForCreation = indexOfSpellThisSequence;
                    _runeIllustration.gameObject.GetComponent<ParticleSystemRenderer>().material = _runeImeges[indexOfSpellThisSequence - 1];
                }
                
                //присвоить метриал
            }
        }
    }

    private void CheckComparsionListCount()
    {
        _indexOfSpellForCreation = 0;
        _triggersForComparsion.RemoveAt(0);

        switch (_triggersForComparsion.Count)
        {
            case 3:
                TryComparsionSequence(1, _fireFirstSequence);
                TryComparsionSequence(7, _waterFirstSequence);
                break;
            case 4:
                TryComparsionSequence(2, _fireSecondSequence);
                TryComparsionSequence(4, _airFirstSequence);
                break;
            case 5:
                TryComparsionSequence(5, _airSecondSequence);
                TryComparsionSequence(8, _waterSecondSequence);
                break;
            case 6:
                TryComparsionSequence(3, _fireThirdSequence);
                TryComparsionSequence(10, _earthFirstSequence);
                break;
            case 7:
                TryComparsionSequence(6, _airThirdSequence);
                TryComparsionSequence(9, _waterThirdSequence);
                TryComparsionSequence(11, _earthSecondSequence);
                TryComparsionSequence(12, _earthThirdSequence);
                break;
            default:
                break;
        }

        PlaySpellEffect();
    }

    private void PlaySpellEffect()
    {
        switch (_indexOfSpellForCreation)
        {
            case 1:
                SetActiveSpell(_smallFireSpell);
                break;
            case 2:
                SetActiveSpell(_medFireSpell);
                break;
            case 3:
                SetActiveSpell(_bigFireSpell);
                break;
            case 4:
                SetActiveSpell(_smallAirSpell);
                break;
            case 5:
                SetActiveSpell(_medAirSpell);
                break;
            case 6:
                SetActiveSpell(_bigAirSpell);
                break;
            case 7:
                SetActiveSpell(_smallWaterSpell);
                break;
            case 8:
                SetActiveSpell(_medWaterSpell);
                break;
            case 9:
                SetActiveSpell(_bigWaterSpell);
                break;
            case 10:
                SetActiveSpell(_smallEarthSpell);
                break;
            case 11:
                SetActiveSpell(_medEarthSpell);
                break;
            case 12:
                SetActiveSpell(_bigEarthSpell);
                break;
            default:
                _activeSpell = null;
                break;
        }
    }

    private void PlayCreationSpellEffect()
    {
        _spellCreationEffect.gameObject.SetActive(true);
        _spellCreationEffect.Play();
    }

    private void StopCreationSpellEffect()
    {
        _spellCreationEffect.gameObject.SetActive(false);
        _spellCreationEffect.Stop();
    }

    private void SetActiveSpell(GameObject spell)
    {
        Instantiate(_sucessFlash, transform.position + new Vector3(0, 0, 0.2f), Quaternion.identity).Play();

        _runeIllustration.Play();
        
        _activeSpell = null;
        _activeSpell = Instantiate(spell, gameObject.transform);
        _activeSpell.transform.localPosition += new Vector3(0.5f, 0, 0);

        _armed = true;
    }
}

