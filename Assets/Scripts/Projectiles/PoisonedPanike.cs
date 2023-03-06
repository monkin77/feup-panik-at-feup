using System.Collections;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class PoisonedPanike : MonoBehaviour
{
    private Vector3 _finalPosition;
    public Vector3 FinalPosition { get => _finalPosition; set => _finalPosition = value; }
    
    private Vector3 _initialPosition;
    private float _parabolDuration;
    public float ParabolDuration { get => _parabolDuration; set => _parabolDuration = value; }
    
    private float timeElapsed = 0.0f;
    
    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _poison;
    
    // Start is called before the first frame update
    void Start() {
        this._initialPosition = transform.position;
    }

    private void FixedUpdate() {
        if (this._finalPosition == null || this._initialPosition == null || this.ParabolDuration == 0)
            return;
            
        timeElapsed += Time.deltaTime;

        if (timeElapsed <= this.ParabolDuration) {
            float t = timeElapsed / this.ParabolDuration;
            Vector3 currentPos = transform.position;
            if (_initialPosition.x != _finalPosition.x) {
                // moving in the x axis
                Vector3 middlePoint = new Vector3(_initialPosition.x + (_finalPosition.x - _initialPosition.x) / 2.0f, 
                                                  _initialPosition.y +  1, 0f);
                
                currentPos = Mathf.Pow(1 - t, 2) * _initialPosition + // initial point
                             2 * (1 - t) * t * middlePoint + // middle point 
                             Mathf.Pow(t, 2) * _finalPosition; // final point
                
                transform.position = currentPos;
                _trail.transform.position = currentPos;
            }
            else {
                // moving in the y axis
                Vector3 middlePoint = new Vector3(_initialPosition.x + 1, 
                    _initialPosition.y + (_finalPosition.y - _initialPosition.y) / 2.0f, 0f);
                
                currentPos = Mathf.Pow(1 - t, 2) * _initialPosition + // initial point
                             2 * (1 - t) * t * middlePoint + // middle point 
                             Mathf.Pow(t, 2) * _finalPosition; // final point
                
                transform.position = currentPos;
                _trail.transform.position = currentPos;
            }
            
            transform.position = currentPos;
        }
        else {
            _poison = Instantiate(_poison, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
