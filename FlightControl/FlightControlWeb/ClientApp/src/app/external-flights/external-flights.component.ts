import { Component, Inject, Input  } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventEmitterService } from '../event-emitter.service';

@Component({
  selector: 'external-flights',
  templateUrl: './external-flights.component.html',
  styleUrls: ['./external-flights.component.css']
})

export class ExternalFlightsComponent  {


  @Input() extendedFlights: FlightDTO[] = [];

  constructor(@Inject('BASE_URL') baseUrl: string,http: HttpClient, private eventEmitterService: EventEmitterService) {

   

  }

  flightPlanLoadDetailClick(serverURL:string, flightId:string) {
    this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
  }

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

interface ExtendedFlightDTO extends FlightDTO {
  serverId: string;
  serverURL: string;
}
