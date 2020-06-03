import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventEmitterService } from '../event-emitter.service';

@Component({
  selector: 'flight-details',
  templateUrl: './flight-details.component.html'
})

export class FlightDetailsComponent implements OnInit {
  selectedFlightPlan: FlightPlanDTO;
  selected_final_location: LocationDTO;
  selected_ini_date_flight: Date;
  selected_end_date_flight: Date;
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
    if (params[1] == 'clean') {
      this.selectedFlightPlan = null;
    } else {
      this.http.get<FlightPlanDTO>(params[0] + 'api/FlightPlan' + '/' + params[1]).subscribe(result => {
        let sum: number = 0;
        result.segments.forEach((segment,index) =>
            sum += segment.timespan_seconds
        );
        console.log(result.initial_location.date_time);
        let ini_date: Date = new Date(result.initial_location.date_time);
        let end_date: Date = new Date(result.initial_location.date_time);
        end_date.setSeconds(end_date.getSeconds() + sum);

        this.selected_ini_date_flight = ini_date;
        this.selected_end_date_flight = end_date;
        
        this.selected_final_location = result.segments[result.segments.length - 1];
        this.selectedFlightPlan = result;
      }, error => console.error(error));
    }
    
  }

}

interface FlightPlanDTO {
  passengers: number;
  company_name: string;
  initial_location: InitialLocationDTO;
  segments: LocationDTO[];
}

interface LocationDTO {
  longitude: number;
  latitude: number;
  timespan_seconds: number;
}


interface InitialLocationDTO {
  longitude: number;
  latitude: number;
  date_time: string;
}
