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
  public flights: FlightDTO[];
  public extendedFlights: ExtendedFlightDTO[]=[];
  public servers: ServerDTO[];

  constructor(private http: HttpClient, private mapsAPILoader: MapsAPILoader, @Inject('BASE_URL') private baseUrl: string) {

    this.mapsAPILoader.load().then(() => {
      this.bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(51.130739, -0.868052), // SW
        new google.maps.LatLng(51.891257, 0.559417) // NE
      );
      console.log(this.bounds);
    });

    //this.flights = [
    //  {
    //  flight_id: "ABCD-0123",
    //  longitude: 33.244,
    //  latitude: 31.12,
    //  passengers: 216,
    //  company_name: "SwissAirFinal",
    //  date_time: "2020-12-26T23:56:21",
    //  is_external: true

    //  },
    //  {
    //    flight_id: "ABCD-0123",
    //    longitude: -59.14764404296876,
    //    latitude: -34.57895241036947,
    //    passengers: 216,
    //    company_name: "SwissAirFinal",
    //    date_time: "2020-12-26T23:56:21",
    //    is_external: true

    //  }
    //];

    http.get<ServerDTO[]>(baseUrl + 'api/servers').subscribe(result => {
      this.servers = result;
      console.log(this.servers);
      console.log('x' + this.servers[0].serverURL);

    

      http.get<FlightDTO[]>(this.servers[0].serverURL).subscribe(result => {
        console.log(result);
        this.flights = result;
        console.log('serverid:' + this.servers[0].serverId);
        this.flights.forEach(item => {

          let extendedFlght: ExtendedFlightDTO = {
            flight_id: item.flight_id,
            longitude: item.longitude,
            latitude: item.latitude,
            passengers: item.passengers,
            company_name: item.company_name,
            date_time: item.date_time,
            is_external: item.is_external,
            serverId: this.servers[0].serverId
            
          };
         
          this.extendedFlights.push(extendedFlght);
          
        });
        //this.flights.map((obj) => {
        //  //obj.serverId = this.servers[0].serverId;
        //  // or via brackets
        //  obj['serverId'] = this.servers[0].serverId;
        //  return obj;
        //})
        //for (var i = 0; i < this.flights.length; i++) {
        //  this.flights[i].serverId = this.servers[0].serverId; // Add "total": 2 to all objects in array
        //}
        //this.flights.push({ 'serverId': this.servers[0].serverId  });  

        console.log(this.extendedFlights);

      }, error => console.error(error));

    }, error => console.error(error));

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
                  let self = this;

                  fileReader.onloadend = function (x) {

                    console.log("jsondata:" + fileReader.result.toString());
                    self.postData(fileReader.result.toString());

                  };

                  fileReader.readAsText(file);

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


  public postData(jsondata: string) {

    //// Headers
    const headers = new HttpHeaders({
      'security-token': 'mytoken'
    });
    console.log(this.baseUrl);
    this.http.post<FlightDTO>(this.baseUrl + 'api/FlightPlan', JSON.parse(jsondata), { headers: headers, responseType: 'json' })
      .subscribe(data => {

      });

  }

}

interface FlightDTO {
  flight_id: string;
  longitude: number;
  latitude: number;
  passengers: number;
  company_name: string;
  date_time: Date;
  is_external: boolean;
  //serverId: string;
}

interface ExtendedFlightDTO extends FlightDTO {
  serverId: string
}

interface ServerDTO {
  serverId: string;
  serverURL: string;
}

