import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'my-flights',
  templateUrl: './my-flights.component.html'
})

export class MyFlightsComponent {
  public flights: FlightDTO[];


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<FlightDTO[]>(baseUrl + 'api/flights').subscribe(result => {
      this.flights = result;
      console.log(this.flights);
    }, error => console.error(error));

   

  }
  public flightPlanLoadDetailClick(serverId: string, flightId: string) { }
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
