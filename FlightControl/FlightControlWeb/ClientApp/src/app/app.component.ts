import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ViewChild, OnInit, ElementRef, AfterViewInit, Inject } from "@angular/core";
import { MapsAPILoader, GoogleMapsAPIWrapper, AgmMap } from "@agm/core";
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
declare var google;


@Component({
  selector: "app-map",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  bounds = null;

  constructor(private http: HttpClient, private mapsAPILoader: MapsAPILoader, @Inject('BASE_URL') private baseUrl: string) {
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

  icon = {
    url: 'assets/images/plane-rojo.png',
    scaledSize: {
      width: 40,
      height: 40
    }
  };

  //From here drag and drop
  public files: NgxFileDropEntry[] = [];

  public json: string;

  public dropped(files: NgxFileDropEntry[]) {
    this.files = files;
    for (const droppedFile of files) {

      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {

          // Here you can access the real file
          console.log(droppedFile.relativePath, file);
          
          
         
          let fileReader: FileReader = new FileReader();
          let strPrueba: string = '{ "flight_id": "A1", "longitude": 3.44, "latitude": 3.45, "passengers": 244, "company_name": "Aerolineas", "date_time": "2020-05-17T12:41:26.483", "is_external": false }';

          let jsondata : string | ArrayBuffer;

          fileReader.onloadend = function (x) {
            jsondata = fileReader.result;
          };

          fileReader.readAsText(file);

          //// You could upload it like this:
          const formData = new FormData();
          formData.append('data', JSON.parse(strPrueba));
          console.log(formData);
         
          //// Headers
          const headers = new HttpHeaders({
            'security-token': 'mytoken'
          });
          console.log(this.baseUrl);
          this.http.post<FlightPlan>(this.baseUrl + 'api/FlightPlan', JSON.parse(strPrueba), { headers: headers, responseType: 'json' })
            .subscribe(data => {
              
            });
         
          

        });
      } else {
        // It was a directory (empty directories are added, otherwise only files)
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event) {
    console.log(event);
  }

  public fileLeave(event) {
    console.log(event);
  }



}

interface FlightPlan {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
