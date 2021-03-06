﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailController : MonoBehaviour {

    public float PointsPerSeconds = 0.05f;                                  //Max TrailPoints per Second
    public GameObject Terrain;
    public Transform ParticleEffect;

    private float _NextAdd = 0.0f;                                         //Current Time for next add. If smaller than time, point is added
    private List<Trail> _ActiveTrails = new List<Trail>();                 //List with all active Trails
    private Trail _CurrentTrail = new Trail();
    private float _StartTime = 0;

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {
             //Create new Trail
            _StartTime = Time.time;
            _CurrentTrail = new Trail(_ActiveTrails.Count, "Trail " + _ActiveTrails.Count);
            _ActiveTrails.Add(_CurrentTrail);
        }

        if (Input.GetMouseButton(0) && _CurrentTrail.Id >= 0) {
            if (Time.time > _NextAdd) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Terrain.collider.Raycast(ray, out hit, Mathf.Infinity)) {
                    Instantiate(ParticleEffect, hit.point, Quaternion.identity);
                    _CurrentTrail.addPoint(hit.point);
                    _NextAdd = Time.time + PointsPerSeconds;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            //Release this Trail
            Debug.Log("Anzahl Punkte in " + _CurrentTrail.Name + ": " + _CurrentTrail.getCountPoints() + " nach " + (Time.time - _StartTime));

            /*foreach (Vector3 Point in _CurrentTrail.getTrailPoints()) {
                Debug.Log("showPath() of trail: " + _CurrentTrail.Name);
                Instantiate(ParticleEffect, Point, Quaternion.identity);
            }   */

            _CurrentTrail = new Trail();
        }
	}
}
