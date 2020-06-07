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
  public selected_flight_id: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,private eventEmitterService: EventEmitterService) {
    if (this.eventEmitterService.subsInternal == undefined) {
      this.eventEmitterService.subsInternal = this.eventEmitterService.
        invokeInternalFlightComponentFunction.subscribe((params: string[]) => {
          this.clean();
        });
    }

  }
  public flightPlanLoadDetailClick(serverId: string, flightId: string)
  {
    this.selected_flight_id = flightId;
    this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, flightId]);
    this.eventEmitterService.onClickExternalClean();
  }
  public delete(flightId: string) {
    console.log('aaa' + flightId);
    this.http.delete(this.baseUrl + 'api/flights/' + flightId ).subscribe(result => {
    }, error => console.error(error));
    this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, 'clean']);
    
  }
  public clean() {
    this.selected_flight_id = '';
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
