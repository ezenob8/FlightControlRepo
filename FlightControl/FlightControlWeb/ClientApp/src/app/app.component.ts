import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ViewChild, OnInit, ElementRef, AfterViewInit, Inject } from "@angular/core";
import { MapsAPILoader, GoogleMapsAPIWrapper, AgmMap } from "@agm/core";
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import { Observable, interval } from 'rxjs';
import { EventEmitterService } from './event-emitter.service';


declare var google: { maps: { LatLngBounds: new (arg0: any, arg1: any) => any; LatLng: new (arg0: number, arg1: number) => any; }; };


@Component({
  selector: "app-map",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})

export class AppComponent {

  public bounds:any;
  internalFlights: FlightDTO[] = [];
  public externalFlights: FlightDTO[] = [];
  public extendedFlights: ExtendedFlightDTO[]=[];
  public servers: ServerDTO[] = [];
  public showDrop: boolean;
  //public selectedFlightPlan$: Observable<FlightPlanDTO>;
  public selectedFlightPlan: FlightPlanDTO;
  public selected_flight_id: string = '';
  public latitud: number;
  public showLine: boolean = false;

  constructor(private http: HttpClient, private mapsAPILoader: MapsAPILoader, @Inject('BASE_URL') private baseUrl: string, private eventEmitterService: EventEmitterService) {
    if (this.eventEmitterService.subsApp == undefined) {
      this.eventEmitterService.subsApp = this.eventEmitterService.
        invokeAppComponentFunction.subscribe((params: string[]) => {
          this.showPoligone(params);
        });
    }

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
        http.get<FlightDTO[]>(baseUrl + 'api/Flights?relative_to=' + new Date().toISOString().substring(0, 19) + 'Z').subscribe(resultInternal => {
          self.internalFlights = resultInternal;
        });

        //External Flights
        http.get<ServerDTO[]>(baseUrl + 'api/servers').subscribe(resultServer => {
          self.servers = resultServer;
          self.servers.forEach(server => {
            http.get<FlightDTO[]>(server.serverURL + 'api/Flights?relative_to=' + new Date().toISOString().substring(0, 19) +'Z').subscribe(resultExternal => {
              let ext: ExtendedFlightDTO[] = [];
              self.extendedFlights = null;
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

  public mapReady(map:any) {
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

      }
    }

  }

  public fileOver(event:any) {
    console.log(event);
  }

  public fileLeave(event:any) {
    console.log(event);
  }


  public postData(jsondata: string) {

    //// Headers
    const headers = new HttpHeaders({
      'security-token': 'mytoken'
    });
    this.http.post<FlightDTO>(this.baseUrl + 'api/FlightPlan', JSON.parse(jsondata), { headers: headers, responseType: 'json' })
      .subscribe(data => {

      });

  }

  public flightPlanLoadDetailClick(serverURL: string, flightId: string) {
    this.eventEmitterService.onClickInternalClean();
    this.eventEmitterService.onClickExternalClean();
    this.selected_flight_id = flightId;
    if (serverURL == undefined)
        serverURL = this.baseUrl;
    this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
  }

  public showPoligoneAndDetail(serverURL: string, flightId: string) {
    this.showPoligone([serverURL, flightId]);
    this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
  }

  public showPoligone(params: string[]) {
    if (params[1] == 'clean') {
      this.selectedFlightPlan = null;
      this.selected_flight_id = '';
    } else {
        this.http.get<FlightPlanDTO>(params[0] + 'api/FlightPlan' + '/' + params[1]).subscribe(result => {
          this.selectedFlightPlan = result;
          this.selected_flight_id = params[1];
        }, error => console.error(error));
        this.selected_flight_id = params[1];
    }

  }



  public clean() {
    this.selectedFlightPlan = null;
    this.selected_flight_id = '';
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

interface FlightPlanDTO {
  flight_id: string;
  passengers: number;
  company_name: string;
  initial_location: InitialLocationDTO;
  segments: LocationDTO[];
}

interface LocationDTO {
  longitude: number;
  latitude: number;
  timespan_seconds: number;
}


interface InitialLocationDTO {
  longitude: number;
  latitude: number;
  date_time: string;
}
