import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventEmitterService } from '../event-emitter.service';

@Component({
  selector: 'flight-details',
  templateUrl: './flight-details.component.html'
})

export class FlightDetailsComponent implements OnInit {
  selectedFlightPlan: FlightPlanDTO;
  baseUrl: string;
  http: HttpClient;

   
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private eventEmitterService: EventEmitterService) {
    this.baseUrl = baseUrl;
    this.http = http;
  }
    ngOnInit(): void {
      if (this.eventEmitterService.subsVar == undefined) {
        this.eventEmitterService.subsVar = this.eventEmitterService.
          invokeFlightPlanComponentFunction.subscribe((params: string[]) => {
            this.loadDetails(params);
          });
      }
    }

  public loadDetails(params: string[]) {
    //agregar params[0] + 'api/FlightPlan' + '/' +  params[1]
    //testing params[0] + '/' + params[1]
    console.log(params[0] + '' + params[1]);
    if (params[0] == 'clean') {
      this.selectedFlightPlan = null;
    } else {
      this.http.get<FlightPlanDTO[]>(params[0] + 'api/FlightPlan' + '/' + params[1]).subscribe(result => {
        this.selectedFlightPlan = result[0];
        console.log(this.selectedFlightPlan);
        console.log(this.selectedFlightPlan.initial_location);
      }, error => console.error(error));
    }
    
  }
}

interface FlightPlanDTO {
  passengers: number;
  company_name: string;
  initial_location: LocationDTO;
  segments: LocationDTO[];
  final_location: LocationDTO;
  end_date_flight: Date;
}

interface LocationDTO {
  longitude: number;
  latitude: number;
  date_time: Date;
}
