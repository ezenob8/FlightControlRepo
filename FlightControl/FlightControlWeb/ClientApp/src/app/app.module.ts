import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { FlightDetailsComponent } from './flight-details/flight-details.component';
import { MyFlightsComponent } from './my-flights/my-flights.component';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    AgmCoreModule.forRoot({
      // please get your own API key here:
      // https://developers.google.com/maps/documentation/javascript/get-api-key?hl=en
      apiKey: 'AIzaSyC0cZhn4jFA7YotLo4msJAyprA6xE0tunk'
    })
  ],
  declarations: [AppComponent, FlightDetailsComponent, MyFlightsComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
