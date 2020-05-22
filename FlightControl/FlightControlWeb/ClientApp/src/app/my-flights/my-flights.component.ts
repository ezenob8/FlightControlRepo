import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'my-flights',
  templateUrl: './my-flights.component.html'
})

export class MyFlightsComponent {
  public flights: Flight[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    // Get array of flights from the flights component
    http.get<Flight[]>(baseUrl + 'api/Flights').subscribe(result => {
      this.flights = result;
    }, error => console.error(error));
  }
}

interface Flight {
  passengers: number;
  company_name: string;
  initial_location: Location;
  //segments: Location[];
}

interface Location {
  longitude: number;
  latitude: number;
  date_time: Date;
  //segments: Location[];
}
