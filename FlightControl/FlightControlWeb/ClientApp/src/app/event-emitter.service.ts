import { Injectable, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

  invokeFlightPlanComponentFunction = new EventEmitter();
  subsVar: Subscription;

  constructor() { }

  onClickLoadFlightDetails(params: string[]) {
    this.invokeFlightPlanComponentFunction.emit(params);
  }   
}
