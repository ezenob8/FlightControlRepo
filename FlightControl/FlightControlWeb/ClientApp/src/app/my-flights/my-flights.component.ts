import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventEmitterService } from '../event-emitter.service';

@Component({
  selector: 'my-flights',
  templateUrl: './my-flights.component.html',
  styleUrls: ['./my-flights.component.css']
})

export class MyFlightsComponent {
  @Input() flights: FlightDTO[];
  

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,private eventEmitterService: EventEmitterService) {
    //http.get<FlightDTO[]>(baseUrl + 'api/flights/activeinternalflights').subscribe(result => {
    //  this.flights = result;
    //  console.log(this.flights);
    //}, error => console.error(error));

   

  }
  public flightPlanLoadDetailClick(serverId: string, flightId: string)
  {
    console.log(flightId);

    this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, flightId]);
  }
  public delete(flightId: string) {
    console.log('aaa' + flightId);
    this.http.delete(this.baseUrl + 'api/flights/' + flightId ).subscribe(result => {
    }, error => console.error(error));
    this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, 'clean']);
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
