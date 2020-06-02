import { Injectable, EventEmitter, Output } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

  @Output() invokeFlightPlanComponentFunction: EventEmitter<any> = new EventEmitter<any>(false);
  subsVar: Subscription;

  constructor() { }

  onClickLoadFlightDetails(params: string[]) {
    this.invokeFlightPlanComponentFunction.emit(params);
  }   
}
