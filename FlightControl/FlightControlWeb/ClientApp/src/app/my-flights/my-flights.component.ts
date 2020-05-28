import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'my-flights',
  templateUrl: './my-flights.component.html'
})

export class MyFlightsComponent {
  public flightPlans: FlightPlan[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<FlightPlan[]>(baseUrl + 'api/flightPlan').subscribe(result => {
      this.flightPlans = result;
      console.log(this.flightPlans);
    }, error => console.error(error));
  }
}

interface FlightPlan {
  passengers: number;
  company_name: string;
  initial_location: Location;
  segments: Location[];
}

interface Location {
  longitude: number;
  latitude: number;
  date_time: Date;
}
