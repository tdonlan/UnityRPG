using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

//Class to store camera data for panning, zooming smoothly

    public class BattleSceneCameraData
    {
        public float CameraX { get; set; }
        public float CameraY { get; set; }
        public float Zoom { get; set; }

        public float PanLerp {get;set;}
        public float ZoomLerp {get;set;}

        public BattleSceneCameraData(float x, float y, float zoom, float panLerp, float zoomLerp)
        {
            this.CameraX = x;
            this.CameraY = y;
            this.Zoom = zoom;

            this.PanLerp = panLerp;
            this.ZoomLerp = zoomLerp;

        }

        public void SetCamera(float x, float y)
      {
          this.CameraX = x;
          this.CameraY = y;
      }

        public void SetZoom(float zoom)
        {
            this.Zoom = zoom;
        }

        public Vector3 UpdateCamera(float curX, float curY, float curZ)
      {
          var newX = Mathf.Lerp(curX, CameraX, PanLerp);
          var newY = Mathf.Lerp(curY, CameraY, PanLerp);
          return new Vector3(newX, newY,curZ);

      }
        
        public float UpdateZoom(float curZoom)
        {
            return Mathf.Lerp(curZoom, Zoom, ZoomLerp);
        }

    }
