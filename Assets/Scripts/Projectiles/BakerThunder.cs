using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BakerThunder : MonoBehaviour
{
    private Vector3 _position;
    [SerializeField] private GameObject _bossPrefab;
    
    // Thunder position is a bit above because of the sprite size
    public Vector3 Position { get => _position; set => 
        _position = new Vector3(value.x, value.y + thunder_y_offset, value.z); }
    
    private float thunder_y_offset = 0.75f;

    private void Start()
    {
        // apply the y offset
        this.Position = this.transform.position;
        this.transform.position = this.Position;
    }

    public void FinishThunderAnimation()
    {
        Instantiate(_bossPrefab, 
            new Vector3(this.Position.x, this.Position.y - thunder_y_offset, this.Position.z), 
            Quaternion.identity);
        Destroy(this.gameObject);
    }
}
