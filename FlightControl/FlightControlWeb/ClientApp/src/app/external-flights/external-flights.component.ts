import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventEmitterService } from '../event-emitter.service';

@Component({
  selector: 'external-flights',
  templateUrl: './external-flights.component.html'
})

export class ExternalFlightsComponent  {
  public servers: ServerDTO[];
  public flights: FlightDTO[];
  public extendedFlights: ExtendedFlightDTO[] = [];

  constructor(@Inject('BASE_URL') baseUrl: string,http: HttpClient, private eventEmitterService: EventEmitterService) {

    http.get<ServerDTO[]>(baseUrl + 'api/servers').subscribe(result => {
      this.servers = result;
      http.get<FlightDTO[]>(this.servers[0].serverURL).subscribe(result => {

        this.flights = result;

        this.flights.forEach(item => {

          let extendedFlght: ExtendedFlightDTO = {
            flight_id: item.flight_id,
            longitude: item.longitude,
            latitude: item.latitude,
            passengers: item.passengers,
            company_name: item.company_name,
            date_time: item.date_time,
            is_external: item.is_external,
            serverId: this.servers[0].serverId
          };
          this.extendedFlights.push(extendedFlght);

        });

      }, error => console.error(error));

    }, error => console.error(error));

  }

  flightPlanLoadDetailClick(serverId:string, flightId:string) {
    this.eventEmitterService.onClickLoadFlightDetails([serverId, flightId]);
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

interface FlightDTO {
  flight_id: string;
  longitude: number;
  latitude: number;
  passengers: number;
  company_name: string;
  date_time: Date;
  is_external: boolean;
  //serverId: string;
}

interface ExtendedFlightDTO extends FlightDTO {
  serverId: string
}
