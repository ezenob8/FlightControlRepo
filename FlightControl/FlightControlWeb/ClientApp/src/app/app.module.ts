import { NgModule, forwardRef } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AgmCoreModule, AgmMarker, FitBoundsAccessor } from '@agm/core';
import { NgxFileDropModule } from 'ngx-file-drop';

import { EventEmitterService } from './event-emitter.service';

import { AppComponent } from './app.component';
import { FlightDetailsComponent } from './flight-details/flight-details.component';
import { MyFlightsComponent } from './my-flights/my-flights.component';
import { ExternalFlightsComponent } from './external-flights/external-flights.component';
import { ToggleButtonComponent } from './toggle-button.component';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    HttpClientModule,
    AgmCoreModule.forRoot({
      // please get your own API key here:
      // https://developers.google.com/maps/documentation/javascript/get-api-key?hl=en
      apiKey: 'AIzaSyC0cZhn4jFA7YotLo4msJAyprA6xE0tunk'
    }),
    NgxFileDropModule
  ],
  declarations: [AppComponent, FlightDetailsComponent, MyFlightsComponent, ExternalFlightsComponent, ToggleButtonComponent],
  providers: [
    { provide: 'BASE_URL', useFactory: getBaseUrl },
    { provide: FitBoundsAccessor, useExisting: forwardRef(() => AgmMarker) },
    EventEmitterService
  ],
  bootstrap: [AppComponent]
})


export class AppModule { }
