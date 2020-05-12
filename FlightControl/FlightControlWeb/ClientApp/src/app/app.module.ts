import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { FlightDetailsComponent } from './flight-details/flight-details.component';
import { MyFlightsComponent } from './my-flights/my-flights.component';
import { AgmCoreModule } from '@agm/core';
import { RouterModule } from '@angular/router';
import { InjectionToken } from '@angular/core';
export const BASE_URL = new InjectionToken<string>('BASE_URL');

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    HttpClientModule,
    AgmCoreModule.forRoot({
      // please get your own API key here:
      // https://developers.google.com/maps/documentation/javascript/get-api-key?hl=en
      apiKey: 'AIzaSyC0cZhn4jFA7YotLo4msJAyprA6xE0tunk'
    })
  ],
  declarations: [AppComponent, FlightDetailsComponent, MyFlightsComponent],
  providers: [{ provide: BASE_URL, useValue: "http://localhost:21431" }],
  bootstrap: [AppComponent]
})
export class AppModule { }
