import { Component, ViewChild, OnInit, ElementRef, AfterViewInit } from "@angular/core";
import { MapsAPILoader, GoogleMapsAPIWrapper, AgmMap } from "@agm/core";
// import "scriptjs";
// import { get } from "scriptjs";
// import { ClusterManager } from "@agm/js-marker-clusterer";
declare var google;

@Component({
  selector: "app-map",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  bounds = null;

  constructor(private mapsAPILoader: MapsAPILoader) {
    this.mapsAPILoader.load().then(() => {
      this.bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(51.130739, -0.868052), // SW
        new google.maps.LatLng(51.891257, 0.559417) // NE
      );
      console.log(this.bounds);
    });

  }

  protected mapReady(map) {
    map.fitBounds(this.bounds);
  }
}
