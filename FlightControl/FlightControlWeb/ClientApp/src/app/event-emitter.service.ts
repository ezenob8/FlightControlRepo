import { Injectable, EventEmitter, Output } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {

  @Output() invokeFlightPlanComponentFunction: EventEmitter<any> = new EventEmitter<any>(false);
  @Output() invokeAppComponentFunction: EventEmitter<any> = new EventEmitter<any>(false);
  @Output() invokeInternalFlightComponentFunction: EventEmitter<any> = new EventEmitter<any>(false);
  @Output() invokeExternalFlightComponentFunction: EventEmitter<any> = new EventEmitter<any>(false);
  subsFlightPlan: Subscription;
  subsApp: Subscription;
  subsInternal: Subscription;
  subsExternal: Subscription;

  constructor() { }

  onClickLoadFlightDetails(params: string[]) {
    this.invokeFlightPlanComponentFunction.emit(params);
  }
  onClickShowPoligone(params: string[]) {
    this.invokeAppComponentFunction.emit(params);
  }
  onClickInternalClean() {
    this.invokeInternalFlightComponentFunction.emit();
  }

  onClickExternalClean() {
    this.invokeExternalFlightComponentFunction.emit();
  }  
}
