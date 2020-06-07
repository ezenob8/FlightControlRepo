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
  public selected_flight_id: string;

  constructor(@Inject('BASE_URL') baseUrl: string,http: HttpClient, private eventEmitterService: EventEmitterService) {

    if (this.eventEmitterService.subsExternal == undefined) {
      this.eventEmitterService.subsExternal = this.eventEmitterService.
        invokeExternalFlightComponentFunction.subscribe((params: string[]) => {
          this.clean();
        });
    }

  }

  flightPlanLoadDetailClick(serverURL: string, flightId: string) {
    this.selected_flight_id = flightId;
    this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
    this.eventEmitterService.onClickInternalClean();
  }
  public clean() {
    this.selected_flight_id = '';
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
