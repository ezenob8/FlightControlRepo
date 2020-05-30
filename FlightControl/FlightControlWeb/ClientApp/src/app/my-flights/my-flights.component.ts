import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'my-flights',
  templateUrl: './my-flights.component.html'
})

export class MyFlightsComponent {
  public flightPlans: FlightPlanDTO[];
  public servers: ServerDTO[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<FlightPlanDTO[]>(baseUrl + 'api/flightPlan').subscribe(result => {
      this.flightPlans = result;
      console.log(this.flightPlans);
    }, error => console.error(error));

    http.get<ServerDTO[]>(baseUrl + 'api/servers').subscribe(result => {
      this.servers = result;
      console.log(this.servers);
      console.log('x' + this.servers[0].serverURL);
      http.get<FlightDTO[]>(this.servers[0].serverURL).subscribe(result => {
        console.log(result);



      }, error => console.error(error));

    }, error => console.error(error));

  }
}

interface FlightPlanDTO {
  passengers: number;
  company_name: string;
  initial_location: LocationDTO;
  segments: LocationDTO[];
}

interface LocationDTO {
  longitude: number;
  latitude: number;
  date_time: Date;
}

interface ServerDTO {
  serverId: string;
  serverURL: string;
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
