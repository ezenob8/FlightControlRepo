import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ViewChild, OnInit, ElementRef, AfterViewInit, Inject } from "@angular/core";
import { MapsAPILoader, GoogleMapsAPIWrapper, AgmMap } from "@agm/core";
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import { Observable, interval } from 'rxjs';
import { EventEmitterService } from './event-emitter.service';


declare var google;


@Component({
  selector: "app-map",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})

export class AppComponent {

  bounds = null;
  internalFlights: FlightDTO[] = [];
  public externalFlights: FlightDTO[] = [];
  public extendedFlights: ExtendedFlightDTO[]=[];
  public servers: ServerDTO[] = [];
  public showDrop: boolean;

  constructor(private http: HttpClient, private mapsAPILoader: MapsAPILoader, @Inject('BASE_URL') private baseUrl: string, private eventEmitterService: EventEmitterService) {
    this.mapsAPILoader.load().then(() => {
      this.bounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(51.130739, -0.868052), // SW
        new google.maps.LatLng(51.891257, 0.559417) // NE
      );
      console.log(this.bounds);
    });

    const self = this;
    const observer = interval(3000)
      .subscribe((val) => {


        //Internal Flights
        http.get<FlightDTO[]>(baseUrl + 'api/flights/activeinternalflights').subscribe(resultInternal => {
          self.internalFlights = resultInternal;
        });

        //External Flights
        http.get<ServerDTO[]>(baseUrl + 'api/servers').subscribe(resultServer => {
          self.servers = resultServer;
          self.servers.forEach(server => {
            //TODO: agregar + 'api/flights'
            http.get<FlightDTO[]>('http://rony1.atwebpages.com/api/flights/?relative_to=2020-06-02T02:54:00Z').subscribe(resultExternal => {
              let ext: ExtendedFlightDTO[] = [];;
              self.externalFlights = resultExternal;
              self.externalFlights.forEach(item => {
                const extendedFlight: ExtendedFlightDTO = {
                  flight_id: item.flight_id,
                  longitude: item.longitude,
                  latitude: item.latitude,
                  passengers: item.passengers,
                  company_name: item.company_name,
                  date_time: item.date_time,
                  is_external: item.is_external,
                  serverId: server.serverId,
                  serverURL: server.serverURL
                };
                ext.push(extendedFlight);
                self.extendedFlights = ext;
              });
            }, error => console.error(error));
          }
          )
          

        }, error => console.error(error));

      });
    
  }

  protected mapReady(map) {
    map.fitBounds(this.bounds);
  }

  icon = {
    url: 'assets/images/plane-rojo.png',
    scaledSize: {
      width: 30,
      height: 30
    }
  };

  iconBlack = {
    url: 'assets/images/plane-black.png',
    scaledSize: {
      width: 30,
      height: 30
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

  public flightPlanLoadDetailClick(serverId: string, flightId: string) {
    console.log(flightId);

    this.eventEmitterService.onClickLoadFlightDetails([serverId, flightId]);
  }

  public clean() {
    this.eventEmitterService.onClickLoadFlightDetails(['', 'clean']);
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
}

interface ExtendedFlightDTO extends FlightDTO {
  serverId: string;
  serverURL: string;
}

interface ServerDTO {
  serverId: string;
  serverURL: string;
}

